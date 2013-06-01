using System.Collections.Generic;

namespace AutoTrade.MarketData.Yahoo.Csv
{
    public interface ICsvUrlProvider
    {
        /// <summary>
        /// Gets the url for a csv of quotes from Yahoo
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        string GetUrl(IEnumerable<string> symbols);
    }
}
