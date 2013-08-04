using AutoTrade.Core.StockData;

namespace AutoTrade.MarketData.Yahoo
{
    public interface IYahooMarketDataProvider : IMarketDataProvider, IStockDataProvider
    {
        /// <summary>
        /// Gets the precedence of the market provider
        /// </summary>
        int Precedence { get; }
    }
}