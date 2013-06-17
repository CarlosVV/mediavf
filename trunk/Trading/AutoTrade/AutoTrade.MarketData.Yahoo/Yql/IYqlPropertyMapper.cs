using System.Collections.Generic;
using System.Xml;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    public interface IYqlPropertyMapper
    {
        /// <summary>
        /// Sets properties on a quote from a Yql result
        /// </summary>
        void SetPropertiesOnQuote(StockQuote quote, IEnumerable<XmlElement> elements);
    }
}
