using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace log4net.Kinesis.ExampleCs
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger("Test");

            Console.WriteLine("Press any key to stop...");

            Task.Run(() =>
                {
                    while (true)
                    {
                        logger.Debug("Debug");
                        logger.Info("Info");
                        logger.Error("Errors");
                    }
                });

            Console.ReadKey();
        }
    }
}
