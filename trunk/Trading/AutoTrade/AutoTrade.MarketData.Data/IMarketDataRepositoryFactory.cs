namespace AutoTrade.MarketData.Data
{
    public interface IMarketDataRepositoryFactory
    {
        /// <summary>
        /// Creates a repository
        /// </summary>
        /// <returns></returns>
        IMarketDataRepository CreateRepository();
    }
}
