using System;
using AutoTrade.Core;
using AutoTrade.Core.Settings;
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
        /// The format for the url to retrieve CSVs
        /// </summary>
        private readonly string _csvUrlFormat;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YahooMarketDataAppSettings"/>
        /// </summary>
        /// <param name="appSettingsProvider"></param>
        public YahooMarketDataAppSettings(IAppSettingsProvider appSettingsProvider)
        {
            // check that the app settings provider is set
            if (appSettingsProvider == null)
                throw new ArgumentNullException("appSettingsProvider");

            // get the format of the yql url
            _yqlUrlFormat = appSettingsProvider.GetSetting(YqlEndpointSettingName, DefaultYqlEndpoint);
            if (string.IsNullOrWhiteSpace(_yqlUrlFormat))
                throw new EndpointFormatNotFoundException("YQL");

            // get flag indicating whether or not to include diagnostics data in the results of a YQL query
            _yqlIncludeDiagnostics = appSettingsProvider.GetSetting(IncludeDiagnosticsSettingName, false);

            // get the format of the csv url
            _csvUrlFormat = appSettingsProvider.GetSetting(CsvEndpointSettingName, DefaultCsvEndpoint);
            if (string.IsNullOrWhiteSpace(_csvUrlFormat))
                throw new EndpointFormatNotFoundException("CSV");
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
        /// Gets the format for the url to retrieve CSVs
        /// </summary>
        public string CsvUrlFormat
        {
            get { return _csvUrlFormat; }
        }

        #endregion
    }
}
