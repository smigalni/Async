using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Service1
{
    public class Service : ServiceBase
    {
        public string Test { get; set; }
        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            var worker = new Thread(Start);
            worker.Name = "MyWorker";
            worker.IsBackground = false;
            worker.Start();

            //StartAsync().Wait();
        }

        protected override void OnStop()
        {
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return true;
        }

        protected override void OnShutdown()
        {
        }

        public void Start()
        {
            while (true)
            {
                //DoSomeWork();
                MainAsync().Wait();
            }
        }

        private async Task MainAsync()
        {
            Console.WriteLine("Hei vi skal gjøre noe jobb og så sove");
            Test = "setaste";

           await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
        }

        public async Task StartAsync()
        {
            while (true)
            {
               await DoSomeWorkAsync();
            }
        }

        private void DoSomeWork()
        {
            Console.WriteLine("Hei vi skal gjøre noe jobb og så sove");

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        private async Task DoSomeWorkAsync()
        {
            Console.WriteLine("Hei vi skal gjøre noe jobb og så sove");

           await Task.Delay(TimeSpan.FromSeconds(10));
        }


    }
}