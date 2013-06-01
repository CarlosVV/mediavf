using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class EndpointFormatNotFoundException : Exception
    {
        private readonly string _endpointName;

        public EndpointFormatNotFoundException(string endpointName)
        {
            _endpointName = endpointName;
        }

        public override string Message
        {
            get { return string.Format(Resources.EndpointFormatNotFoundMessageFormat); }
        }
    }
}