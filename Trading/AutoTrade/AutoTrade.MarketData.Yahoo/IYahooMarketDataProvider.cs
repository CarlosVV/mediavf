namespace AutoTrade.MarketData.Yahoo
{
    public interface IYahooMarketDataProvider : IMarketDataProvider
    {
        /// <summary>
        /// Gets the precedence of the market provider
        /// </summary>
        int Precedence { get; }
    }
}