using System.Collections.Generic;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.EmailAlerts
{
    public class EmailAlertStockListProvider : IStockListProvider
    {
        /// <summary>
        /// Gets a list of stocks from an email alert system
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public IEnumerable<Stock> GetStocks(Subscription subscription)
        {
            throw new System.NotImplementedException();
        }
    }
}
