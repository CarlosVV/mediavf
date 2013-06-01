namespace AutoTrade.MarketData.Yahoo
{
    public interface IYahooMarketDataSettings
    {
        /// <summary>
        /// Gets flag indicating whether or not to include diagnostic information in the YQL response
        /// </summary>
        bool YqlIncludeDiagnostics { get; }

        /// <summary>
        /// Gets the url format for executing a YQL query
        /// </summary>
        string YqlUrlFormat { get; }

        /// <summary>
        /// Gets the url format for retrieving a CSV from Yahoo Finance
        /// </summary>
        string CsvUrlFormat { get; }
    }
}
