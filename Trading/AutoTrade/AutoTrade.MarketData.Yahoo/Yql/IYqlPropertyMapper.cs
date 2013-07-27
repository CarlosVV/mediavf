using System.Collections.Generic;
using System.Xml;
using AutoTrade.Core.StockData;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    public interface IYqlPropertyMapper
    {
        /// <summary>
        /// Sets properties on a quote from a Yql result
        /// </summary>
        /// <typeparam name="T">The type of item to populate</typeparam>
        /// <param name="item">The item to populate</param>
        /// <param name="elements">The xml elements from which to populate properties</param>
        void SetPropertiesFromXml<T>(T item, IEnumerable<XmlElement> elements);
    }
}
