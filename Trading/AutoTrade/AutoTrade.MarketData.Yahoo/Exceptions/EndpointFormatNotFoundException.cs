using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class EndpointFormatNotFoundException : Exception
    {
        /// <summary>
        /// The name of the endpoint not found
        /// </summary>
        private readonly string _endpointName;

        /// <summary>
        /// Instantiates a <see cref="EndpointFormatNotFoundException"/>
        /// </summary>
        /// <param name="endpointName"></param>
        public EndpointFormatNotFoundException(string endpointName)
        {
            _endpointName = endpointName;
        }

        /// <summary>
        /// Gets the message indicating the endpoint that was not found
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.EndpointFormatNotFoundMessageFormat, _endpointName); }
        }
    }
}