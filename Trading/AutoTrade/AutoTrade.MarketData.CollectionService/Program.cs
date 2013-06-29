using System;
using System.ServiceProcess;
using AutoTrade.MarketData.CollectionService.Properties;

namespace AutoTrade.MarketData.CollectionService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            RunConsole();
#else
            // run the service
            ServiceBase.Run(new ServiceBase[] { new CollectionService() });
#endif
        }

        /// <summary>
        /// Runs the service as a console app
        /// </summary>
        private static void RunConsole()
        {
            // create service
            var service = new CollectionService();

            try
            {
                while (true)
                {
                    // start service
                    Console.WriteLine(Resources.ServiceStartMessage);
                    service.StartService();
                    Console.WriteLine(Resources.ServiceStartedMessage);

                    // read key to stop service
                    Console.WriteLine(Resources.AnyKeyToStopMessage);
                    Console.ReadKey();

                    // stop service
                    Console.WriteLine(Resources.StoppingServiceMessage);
                    service.StopService();
                    Console.WriteLine(Resources.ServiceStoppedMessage);

                    // read key to stop service
                    Console.WriteLine(Resources.AnyKeyToStartEscToQuitMessage);

                    // read key and perform action
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }
            }
            catch (Exception exception)
            {
                // write error message to the console
                Console.WriteLine(Resources.ServiceErrorMessage);
                Console.WriteLine(exception.ToString());
                Console.WriteLine();

                // indicate any key to exit
                Console.WriteLine(Resources.AnyKeyToExitMessage);
                Console.ReadKey();
            }
        }
    }
}
