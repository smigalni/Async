using System;
using System.Diagnostics;
using System.Threading.Tasks;

// ReSharper disable AccessToDisposedClosure
// ReSharper disable MethodSupportsCancellation
#pragma warning disable CS4014 

namespace Crawler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            MainAsync().Wait();

            HandleUpdates().Wait();




            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);
        }

        private static async Task MainAsync()
        {
            try
            {
                // Asynchronous implementation.
                //********** måte 1
                var task1 = new Application().Start();

                #region task wrapper

                //**********måte 2
                //Task taskResultat = default(Task);
                //var task1 = Task.Run(() =>
                //{
                //    taskResultat = new Application().Start();
                //});


                //***********måte 3
                //Task taskResultat = default(Task);
                //var task1 = Task.Factory.StartNew(() =>
                //{
                //    taskResultat = new Application().Start();
                //});

                #endregion

                Console.WriteLine($"task1 har id {task1.Id} og status {task1.Status}");

               
                await Task.WhenAll(task1).ConfigureAwait(false);

                //await taskResultat.ConfigureAwait(false);

                Console.WriteLine($"task1 har id {task1.Id} og status {task1.Status}");
            }
            catch (Exception ex)
            {
                // Handle exceptions.
                Console.ReadKey();
            }
        }

        public static async Task HandleUpdates()
        {
            new RethinkDbStore().Test();
            
        }
    }
}