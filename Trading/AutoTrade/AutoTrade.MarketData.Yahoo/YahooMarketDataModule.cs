using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace AutoTrade.MarketData.Yahoo
{
    class YahooMarketDataModule : IModule
    {
        private readonly IUnityContainer _container;

        public YahooMarketDataModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            //_container.RegisterType()
        }
    }
}
