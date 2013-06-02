using System.ServiceProcess;

namespace AutoTrade.MarketData.CollectionService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // run the service
            ServiceBase.Run(new ServiceBase[] { new CollectionService() });
        }
    }
}
