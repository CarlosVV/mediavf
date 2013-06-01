using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Yql.Exceptions
{
    public class InvalidYqlResponseException : Exception
    {
        public InvalidYqlResponseException(Exception innerException)
            : base(null, innerException) { }

        public override string Message
        {
            get { return Resources.InvalidYqlResponseMessage; }
        }
    }
}