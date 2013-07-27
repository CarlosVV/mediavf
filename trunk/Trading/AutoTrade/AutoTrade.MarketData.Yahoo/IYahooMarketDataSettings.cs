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
        /// Gets the message returned by a YQL query indicating the table is blocked
        /// </summary>
        string YqlTableBlockedMessage { get; }

        /// <summary>
        /// Gets the YQL for selecting stock data
        /// </summary>
        string YqlStockSelect { get; }

        /// <summary>
        /// Gets the YQL for selecting quotes
        /// </summary>
        string YqlMultiStockQuoteSelect { get; }

        /// <summary>
        /// Gets the url format for retrieving a CSV from Yahoo Finance
        /// </summary>
        string CsvUrlFormat { get; }
    }
}
