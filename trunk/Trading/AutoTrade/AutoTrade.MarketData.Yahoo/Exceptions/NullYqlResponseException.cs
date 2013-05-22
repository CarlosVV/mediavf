using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class NullYqlResponseException : Exception
    {
        public override string Message
        {
            get { return Resources.NullYqlResponseMessage; }
        }
    }
}