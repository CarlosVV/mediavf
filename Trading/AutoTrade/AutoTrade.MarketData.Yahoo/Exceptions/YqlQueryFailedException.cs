using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class YqlQueryFailedException : Exception
    {
        public YqlQueryFailedException(Exception innerException)
            : base(null, innerException) { }

        public override string Message
        {
            get { return Resources.YqlQueryFailedMessage; }
        }
    }
}