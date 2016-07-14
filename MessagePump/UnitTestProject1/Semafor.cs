using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTestProject1
{
    public class Semafor
    {
        private Task HandleMessage()
        {
            //simulate work
            return Task.Delay(1000);
        }

        [Test]
        public async Task ThePump()
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(1));

            var token = tokenSource.Token;

            var semaphoreSlim = new SemaphoreSlim(2);

            var runningTasks = new ConcurrentDictionary<Task, Task>();

            var pumpTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine("Pumping...");

                    await semaphoreSlim.WaitAsync().ConfigureAwait(false);

                    var runnigTask = HandleMessage();
                    runningTasks.TryAdd(runnigTask, runnigTask);

                    runnigTask.ContinueWith(t =>
                    {
                        semaphoreSlim.Release();
                        Task taskToBeRemoved;
                        runningTasks.TryRemove(t, out taskToBeRemoved);
                    }, TaskContinuationOptions.ExecuteSynchronously).Ignore();
                }
            });

            await pumpTask.ConfigureAwait(false);

            await Task.WhenAll(runningTasks.Values).ConfigureAwait(false);

            tokenSource.Dispose();
        }

    }
}