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
        /// <param name="unityContainer"></param>
        /// <param name="assemblyConfigurationManager"></param>
        public YahooMarketDataModule(IUnityContainer unityContainer,
            IAssemblyConfigurationManager assemblyConfigurationManager)
            : base(unityContainer, assemblyConfigurationManager)
        {
        }
    }
}
