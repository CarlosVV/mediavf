using System.Collections.Generic;

namespace AutoTrade.MarketData.Yahoo.Csv
{
    public interface ICsvTagProvider
    {
        /// <summary>
        /// Gets the tags for providing data returned in CSV from Yahoo
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetTags();
    }
}