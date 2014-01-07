using System;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace log4net.Kinesis.ExampleCs
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger("Test");

        static void Main()
        {
            Console.WriteLine("Press any key to stop...");

            Task.Run(() => Loop());

            Console.ReadKey();
        }

        private async static Task Loop()
        {
            while (true)
            {
                Logger.Debug("Debug");
                Logger.Info("Info");
                Logger.Error("Errors");

                await Task.Delay(1);
            }
        }
    }
}
