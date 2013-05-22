using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class YqlEndpointFormatNotFoundException : Exception
    {
        public override string Message
        {
            get { return Resources.YqlEndpointFormatNotFoundMessage; }
        }
    }
}