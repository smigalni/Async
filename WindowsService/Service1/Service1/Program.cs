using System;
using System.ServiceProcess;

namespace Service1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                new Service().Start();
            }
            else
            {
                ServiceBase.Run(new Service());

            }
        }
    }
}
