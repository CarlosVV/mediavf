using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class NoSymbolsProvidedException : Exception
    {
        /// <summary>
        /// Gets the message indicating that no symbols were provided for a query
        /// </summary>
        public override string Message
        {
            get { return Resources.NoSymbolsProvidedMessage; }
        }
    }
}
