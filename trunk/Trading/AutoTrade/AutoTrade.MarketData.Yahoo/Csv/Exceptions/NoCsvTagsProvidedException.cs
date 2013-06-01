using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Csv.Exceptions
{
    public class NoCsvTagsProvidedException : Exception
    {
        /// <summary>
        /// Gets message indicating that no CSV tags were provided
        /// </summary>
        public override string Message
        {
            get { return Resources.NoCsvTagsProvidedMessage; }
        }
    }
}