using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class QueryResultsAreEmptyException : Exception
    {
        /// <summary>
        /// Gets message indicating results of executing a query came back empty
        /// </summary>
        public override string Message
        {
            get { return Resources.QueryResultsAreEmptyMessage; }
        }
    }
}