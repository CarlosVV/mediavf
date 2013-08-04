using AutoTrade.Core.Modularity;
using AutoTrade.Core.Modularity.Configuration;
using Microsoft.Practices.Unity;

namespace AutoTrade.MarketData.Yahoo
{
    class YahooMarketDataModule : ConfigurableModule
    {
        /// <summary>
        /// Instantiates a <see cref="YahooMarketDataModule"/>
        /// </summary>
        /// <param name="container"></param>
        /// <param name="assemblyConfigurationManager"></param>
        public YahooMarketDataModule(IUnityContainer container,
            IAssemblyConfigurationManager assemblyConfigurationManager)
            : base(container, assemblyConfigurationManager)
        {
        }
    }
}
