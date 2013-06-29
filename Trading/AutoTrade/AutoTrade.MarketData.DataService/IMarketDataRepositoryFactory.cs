using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.DataService
{
    public interface IMarketDataRepositoryFactory
    {
        IMarketDataRepository GetRepository();
    }

    class MarketDataRepositoryFactory : IMarketDataRepositoryFactory
    {
        public IMarketDataRepository GetRepository()
        {
            return new MarketDataRepository();
        }
    }
}
