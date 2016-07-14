using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
// ReSharper disable AccessToDisposedClosure
// ReSharper disable MethodSupportsCancellation




namespace UnitTestProject1
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public async Task Method()
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(500));
            var token = tokenSource.Token;

            var numberOfTasks = 0;

            var runningTasks = new ConcurrentDictionary<Task, Task>();

            var semaphoreSlim = new SemaphoreSlim(2000);

            var pumpTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    //Console.WriteLine("Pumping...");
                    await semaphoreSlim.WaitAsync(token).ConfigureAwait(false);
                    Interlocked.Increment(ref numberOfTasks);

                    var task = HandleMessageWithCancellation(token);

                    runningTasks.TryAdd(task, task);

                    task.ContinueWith(t =>
                    {
                        semaphoreSlim.Release();
                        Task taskToRemove;
                        runningTasks.TryRemove(task, out taskToRemove);

                    }, TaskContinuationOptions.ExecuteSynchronously).Ignore();
                }
            });

            await pumpTask.IgnoreCancellation().ConfigureAwait(false);

            await Task.WhenAll(runningTasks.Values).IgnoreCancellation().ConfigureAwait(false);

            tokenSource.Dispose();
            semaphoreSlim.Dispose();

            Console.Write(
                $"Consumed {numberOfTasks} messages with concurrency {semaphoreSlim.CurrentCount} " +
                $"in 5 seconds. Number Of task {numberOfTasks} Throughput {numberOfTasks / 5} msgs/s. Number in the dic: {runningTasks.Count}");
        }

        private Task HandleMessageWithCancellation(CancellationToken token = default(CancellationToken))
        {
            return Task.Delay(1000, token);
        }
    }
}