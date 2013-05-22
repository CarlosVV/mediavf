using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class InvalidYqlUrlException : Exception
    {
        /// <summary>
        /// The text that failed to be created into a url
        /// </summary>
        private readonly string _failedUrl;

        /// <summary>
        /// Instantiates an <see cref="InvalidYqlUrlException"/>
        /// </summary>
        /// <param name="failedUrl"></param>
        public InvalidYqlUrlException(string failedUrl)
        {
            _failedUrl = failedUrl;
        }

        /// <summary>
        /// Gets the message indicating the failed url
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.InvalidYqlEndpointFormat, _failedUrl); }
        }
    }
}