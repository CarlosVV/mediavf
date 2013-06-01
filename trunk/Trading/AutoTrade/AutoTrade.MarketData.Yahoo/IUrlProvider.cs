using System.Collections.Generic;

namespace AutoTrade.MarketData.Yahoo
{
    public interface IUrlProvider
    {
        /// <summary>
        /// Executes a YQL query and returns the raw xml result
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        string GetUrl(IEnumerable<string> symbols);
    }
}
