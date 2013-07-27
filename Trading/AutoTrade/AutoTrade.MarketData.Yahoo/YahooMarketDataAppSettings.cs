using System;
using AutoTrade.Core.Modularity.Configuration;
using AutoTrade.MarketData.Yahoo.Exceptions;

namespace AutoTrade.MarketData.Yahoo
{
    public class YahooMarketDataAppSettings : IYahooMarketDataSettings
    {
        #region Constants

        /// <summary>
        /// The name of the setting in config for the YQL endpoint
        /// </summary>
        private const string YqlEndpointSettingName = "YqlEndpoint";

        /// <summary>
        /// The default endpoint to use for executing YQL queries
        /// </summary>
        private const string DefaultYqlEndpoint =
            "http://query.yahooapis.com/v1/public/yql?q={0}&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

        /// <summary>
        /// The name of the setting that indicates whether or not to include diagnostic information in the query results
        /// </summary>
        private const string IncludeDiagnosticsSettingName = "YqlIncludeDiagnostics";

        /// <summary>
        /// The name of the setting that provides the message returned by a YQL query that indicates the table is blocked
        /// </summary>
        private const string YqlTableBlockedMessageSettingName = "YqlTableBlockedMessage";

        /// <summary>
        /// The default message indicating that a YQL table is blocked
        /// </summary>
        private const string DefaultYqlTableBlockedMessage = "The current table 'yahoo.finance.quotes' has been blocked.";

        /// <summary>
        /// The name of the setting for getting the YQL query for getting stock data
        /// </summary>
        private const string YqlStockSelectSettingName = "YqlStockSelect";

        /// <summary>
        /// The format for creating YQL query for getting stock data
        /// </summary>
        private const string DefaultYqlStockSelectFormat = @"select * from yahoo.finance.stocks where symbol in ({0})";

        /// <summary>
        /// The name of the setting for getting the YQL query for quotes for multiple stocks
        /// </summary>
        private const string YqlMultiQuoteStockSelectSettingName = "YqlMultiStockQuoteSelect";

        /// <summary>
        /// The format for creating YQL query for getting quotes for multiple stocks
        /// </summary>
        private const string DefaultYqlMultiStockQuoteSelectFormat = @"select * from yahoo.finance.quotes where symbol in ({0})";

        /// <summary>
        /// The name of the setting in config for the CSV endpoint
        /// </summary>
        private const string CsvEndpointSettingName = "CsvEndpoint";

        /// <summary>
        /// The default endpoint for retrieving CSVs from Yahoo
        /// </summary>
        private const string DefaultCsvEndpoint = "http://finance.yahoo.com/d/quotes.csv?s={0}&f={1}";

        #endregion

        #region Fields

        /// <summary>
        /// Flag indicating whether YQL results should include diagnostics data
        /// </summary>
        private readonly bool _yqlIncludeDiagnostics;

        /// <summary>
        /// The format for the url to execute YQL queries
        /// </summary>
        private readonly string _yqlUrlFormat;

        /// <summary>
        /// The format for the url to execute YQL queries
        /// </summary>
        private readonly string _yqlTableBlockedMessage;

        /// <summary>
        /// The YQL query for retrieving stock data
        /// </summary>
        private readonly string _yqlStockSelect;

        /// <summary>
        /// The YQL query for retrieving quotes
        /// </summary>
        private readonly string _yqlMultiStockQuoteSelect;

        /// <summary>
        /// The format for the url to retrieve CSVs
        /// </summary>
        private readonly string _csvUrlFormat;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YahooMarketDataAppSettings"/>
        /// </summary>
        /// <param name="assemblyConfigurationManager"></param>
        public YahooMarketDataAppSettings(IAssemblyConfigurationManager assemblyConfigurationManager)
        {
            // check that the app settings provider is set
            if (assemblyConfigurationManager == null)
                throw new ArgumentNullException("assemblyConfigurationManager");

            var assemblyConfig = assemblyConfigurationManager.GetAssemblyConfiguration(GetType());
            if (assemblyConfig != null)
            {
                // get the format of the yql url
                _yqlUrlFormat = assemblyConfig.Settings.GetSetting(YqlEndpointSettingName, DefaultYqlEndpoint);
                if (string.IsNullOrWhiteSpace(_yqlUrlFormat))
                    throw new EndpointFormatNotFoundException("YQL");

                // get flag indicating whether or not to include diagnostics data in the results of a YQL query
                _yqlIncludeDiagnostics = assemblyConfig.Settings.GetSetting(IncludeDiagnosticsSettingName, true);

                // get flag indicating whether or not to include diagnostics data in the results of a YQL query
                _yqlTableBlockedMessage =
                    assemblyConfig.Settings.GetSetting(YqlTableBlockedMessageSettingName,
                                                       DefaultYqlTableBlockedMessage);

                // get the YQL select for stocks
                _yqlStockSelect =
                    assemblyConfig.Settings.GetSetting(YqlStockSelectSettingName,
                                                       DefaultYqlStockSelectFormat);

                // get the YQL select for quotes
                _yqlMultiStockQuoteSelect =
                    assemblyConfig.Settings.GetSetting(YqlMultiQuoteStockSelectSettingName,
                                                       DefaultYqlMultiStockQuoteSelectFormat);

                // get the format of the csv url
                _csvUrlFormat = assemblyConfig.Settings.GetSetting(CsvEndpointSettingName, DefaultCsvEndpoint);
                if (string.IsNullOrWhiteSpace(_csvUrlFormat))
                    throw new EndpointFormatNotFoundException("CSV");
            }
            else
            {
                // use defaults for all settings
                _yqlUrlFormat = DefaultYqlEndpoint;
                _yqlIncludeDiagnostics = true;
                _yqlTableBlockedMessage = DefaultYqlTableBlockedMessage;
                _csvUrlFormat = DefaultCsvEndpoint;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets flag indicating whether YQL results should include diagnostics data
        /// </summary>
        public bool YqlIncludeDiagnostics
        {
            get { return _yqlIncludeDiagnostics; }
        }

        /// <summary>
        /// Gets the format for the url to execute YQL queries
        /// </summary>
        public string YqlUrlFormat
        {
            get { return _yqlUrlFormat; }
        }

        /// <summary>
        /// Gets the message indicating the YQL table is blocked
        /// </summary>
        public string YqlTableBlockedMessage
        {
            get { return _yqlTableBlockedMessage; }
        }

        /// <summary>
        /// Gets the YQL for selecting stock data
        /// </summary>
        public string YqlStockSelect
        {
            get { return _yqlStockSelect; }
        }

        /// <summary>
        /// Gets the YQL for selecting quotes for multiple stocks
        /// </summary>
        public string YqlMultiStockQuoteSelect
        {
            get { return _yqlMultiStockQuoteSelect; }
        }

        /// <summary>
        /// Gets the format for the url to retrieve CSVs
        /// </summary>
        public string CsvUrlFormat
        {
            get { return _csvUrlFormat; }
        }

        #endregion
    }
}
