using AutoTrade.Core.Email;

namespace AutoTrade.MarketData.EmailAlerts.PennyPicks
{
    public interface IEmailStockParser
    {
        /// <summary>
        /// Parses a stock symbol from an email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        string ParseStockSymbol(IEmail email);
    }
}