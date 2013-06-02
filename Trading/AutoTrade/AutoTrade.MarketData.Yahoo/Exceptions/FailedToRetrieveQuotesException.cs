using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class FailedToRetrieveQuotesException : Exception
    {
        /// <summary>
        /// Gets message indicating that all providers failed to retrieve quote data
        /// </summary>
        public override string Message
        {
            get { return Resources.FailedToRetrieveFromAllProvidersMessage; }
        }
    }
}
