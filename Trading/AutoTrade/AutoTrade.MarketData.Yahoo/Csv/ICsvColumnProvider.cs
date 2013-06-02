using System.Collections.Generic;

namespace AutoTrade.MarketData.Yahoo.Csv
{
    public interface ICsvColumnProvider
    {
        /// <summary>
        /// Gets the number of columns enabled for the csv
        /// </summary>
        int EnabledColumnCount { get; }

        /// <summary>
        /// Gets the tags for providing data returned in CSV from Yahoo
        /// </summary>
        /// <returns></returns>
        string GetTagsString();

        /// <summary>
        /// Gets the tags as a list of column names
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetProperties();
    }
}