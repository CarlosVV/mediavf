using System.Collections.Generic;

namespace AutoTrade.MarketData.Yahoo
{
    public interface IUrlProvider
    {
        /// <summary>
        /// Gets the url for selecting a stock
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        string GetStockUrl(IEnumerable<string> symbols);

        /// <summary>
        /// Gets the url for selecting quotes for a collection of symbols
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        string GetQuotesUrl(IEnumerable<string> symbols);
    }
}
