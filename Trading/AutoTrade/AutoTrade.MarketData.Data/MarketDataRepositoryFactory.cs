namespace AutoTrade.MarketData.Data
{
    public class MarketDataRepositoryFactory : IMarketDataRepositoryFactory
    {
        /// <summary>
        /// Creates a repository
        /// </summary>
        /// <returns></returns>
        public IMarketDataRepository CreateRepository()
        {
            return new MarketDataRepository();
        }
    }
}