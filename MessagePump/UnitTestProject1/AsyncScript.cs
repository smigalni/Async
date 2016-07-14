using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTestProject1
{
    [TestFixture]
    public class AsyncScript
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

            var pumpTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    #region Output

                    Debug.WriteLine("Pumping...");
                    //"test".Output();

                    #endregion

                    await HandleMessage().ConfigureAwait(false);
                }
            });

            await pumpTask.ConfigureAwait(false);

            tokenSource.Dispose();
        }
    }
}