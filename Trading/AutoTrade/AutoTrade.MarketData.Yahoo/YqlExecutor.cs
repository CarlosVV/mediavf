using System;
using System.IO;
using System.Net;
using System.Web;
using AutoTrade.Core;
using AutoTrade.MarketData.Yahoo.Exceptions;

namespace AutoTrade.MarketData.Yahoo
{
    public class YqlExecutor : IYqlExecutor
    {
        #region Constants

        /// <summary>
        /// The name of the setting in config for the YQL endpoint
        /// </summary>
        private const string YqlEndpointSettingName = "YqlEndpoint";

        /// <summary>
        /// The default endpoint to use for executing YQL queires
        /// </summary>
        private const string DefaultYqlEndpoint =
            "http://query.yahooapis.com/v1/public/yql?q={0}&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

        /// <summary>
        /// The name of the setting that indicates whether or not to include diagnostic information in the query results
        /// </summary>
        private const string IncludeDiagnosticsSettingName = "YqlIncludeDiagnostics";

        /// <summary>
        /// The url part for including diagnostics in the results of the query
        /// </summary>
        private const string IncludeDiagnosticsUrlPart = "&diagnostics=true";

        #endregion

        #region Fields

        /// <summary>
        /// The app settings provider
        /// </summary>
        private readonly IAppSettingsProvider _appSettingsProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YqlExecutor"/>
        /// </summary>
        /// <param name="appSettingsProvider"></param>
        public YqlExecutor(IAppSettingsProvider appSettingsProvider)
        {
            _appSettingsProvider = appSettingsProvider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets flag indicating whether or not to include diagnostic information in the YQL response
        /// </summary>
        private bool IncludeDiagnostics
        {
            get { return _appSettingsProvider.GetSetting(IncludeDiagnosticsSettingName, false); }
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
            Uri yqlUrl = GetYqlUrl(yql);

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
        private Uri GetYqlUrl(string yql)
        {
            // get the format of the endpoint url
            string endpointFormat = _appSettingsProvider.GetSetting(YqlEndpointSettingName, DefaultYqlEndpoint);
            if (string.IsNullOrWhiteSpace(endpointFormat))
                throw new YqlEndpointFormatNotFoundException();

            // create the endpoint to use by encoding the yql and inserting it into the format string
            var endpoint = string.Format(endpointFormat, HttpUtility.UrlEncode(yql));

            // add diagnostics url part, if indicated
            if (IncludeDiagnostics)
                endpoint += IncludeDiagnosticsUrlPart;

            // parse to uri
            Uri uri;
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out uri))
                throw new InvalidYqlUrlException(endpoint);

            return uri;
        }

        #endregion
    }
}