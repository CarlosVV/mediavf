using System;

namespace AutoTrade.MarketData.EmailAlerts
{
    public class UnsupportedEmailProtocolException : Exception
    {
        /// <summary>
        /// The name of the protocol
        /// </summary>
        private readonly string _protocolName;

        /// <summary>
        /// Instantiates an <see cref="UnsupportedEmailProtocolException"/>
        /// </summary>
        /// <param name="protocolName"></param>
        public UnsupportedEmailProtocolException(string protocolName)
        {
            _protocolName = protocolName;
        }

        /// <summary>
        /// Gets the message for the exception
        /// </summary>
        public override string Message
        {
            get { return string.Format("Protocol {0} is not supported for managing emails.", _protocolName); }
        }
    }
}