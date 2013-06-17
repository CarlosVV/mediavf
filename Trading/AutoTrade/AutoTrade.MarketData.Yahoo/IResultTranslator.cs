﻿using System.Collections.Generic;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.Yahoo
{
    public interface IResultTranslator
    {
        /// <summary>
        /// Translates the results of a YQL query to StockQuotes
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        IEnumerable<StockQuote> TranslateResultsToQuotes(string response);
    }
}
