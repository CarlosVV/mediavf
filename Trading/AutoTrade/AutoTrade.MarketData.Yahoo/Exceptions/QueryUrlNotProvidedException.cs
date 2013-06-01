using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class QueryUrlNotProvidedException : Exception
    {
        /// <summary>
        /// Gets message indicating the query url was not provided
        /// </summary>
        public override string Message
        {
            get { return Resources.QueryUrlNotProvidedMessage; }
        }
    }
}
