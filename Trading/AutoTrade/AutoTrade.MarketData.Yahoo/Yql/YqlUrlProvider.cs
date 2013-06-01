using System;
using System.Collections.Generic;
using System.Web;
using AutoTrade.MarketData.Yahoo.Yql.Exceptions;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    public class YqlUrlProvider : IUrlProvider
    {
        #region Constants

        /// <summary>
        /// The url part for including diagnostics in the results of the query
        /// </summary>
        private const string IncludeDiagnosticsUrlPart = "&diagnostics=true";

        #endregion

        #region Fields

        /// <summary>
        /// The app settings provider
        /// </summary>
        private readonly IYahooMarketDataSettings _settings;

        /// <summary>
        /// Provides YQL queries for execution
        /// </summary>
        private readonly IYqlQueryProvider _queryProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YqlUrlProvider"/>
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="queryProvider"></param>
        public YqlUrlProvider(IYahooMarketDataSettings settings, IYqlQueryProvider queryProvider)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (queryProvider == null)
                throw new ArgumentNullException("queryProvider");

            _settings = settings;
            _queryProvider = queryProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a YQL query and returns the raw xml result
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public string GetUrl(IEnumerable<string> symbols)
        {
            // get the yql to execute
            string yql = _queryProvider.GetMultiStockQuoteSelect(symbols);
            if (string.IsNullOrWhiteSpace(yql))
                throw new EmptyYqlQueryException();

            // get the url for executing the yql
            return GetYqlUrl(yql);
        }

        /// <summary>
        /// Gets the url for the YQL query
        /// </summary>
        /// <param name="yql"></param>
        /// <returns></returns>
        private string GetYqlUrl(string yql)
        {
            // create the endpoint to use by encoding the yql and inserting it into the format string
            var endpoint = string.Format(_settings.YqlUrlFormat, HttpUtility.UrlPathEncode(yql).Replace("\"", "%22").Replace(",", "%2C"));

            // add diagnostics url part, if indicated
            if (_settings.YqlIncludeDiagnostics)
                endpoint += IncludeDiagnosticsUrlPart;

            // parse to uri
            Uri uri;
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out uri))
                throw new InvalidYqlUrlException(endpoint);

            return endpoint;
        }

        #endregion
    }
}