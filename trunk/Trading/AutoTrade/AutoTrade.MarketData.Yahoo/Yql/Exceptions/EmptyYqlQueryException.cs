using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Yql.Exceptions
{
    public class EmptyYqlQueryException : Exception
    {
        public override string Message
        {
            get { return Resources.EmptyYqlQueryMessage; }
        }
    }
}