using System;
using System.IO;
using System.Net;
using System.Web;
using AutoTrade.MarketData.Yahoo.Exceptions;
using AutoTrade.MarketData.Yahoo.Yql.Exceptions;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    public class YqlExecutor : IYqlExecutor
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

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YqlExecutor"/>
        /// </summary>
        /// <param name="settings"></param>
        public YqlExecutor(IYahooMarketDataSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a YQL query and returns the raw xml result
        /// </summary>
        /// <param name="yql"></param>
        /// <returns></returns>
        public string ExecuteYqlQuery(string yql)
        {
            // get the url
            string yqlUrl = GetYqlUrl(yql);

            try
            {
                // create web request for url
                var request = WebRequest.Create(yqlUrl);

                // read response
                using (var response = request.GetResponse())
                {
                    // get the response stream
                    var responseStream = response.GetResponseStream();
                    if (responseStream == null)
                        throw new NullYqlResponseException();

                    // read the stream to the end
                    using (var reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (NullYqlResponseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new YqlQueryFailedException(ex);
            }
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