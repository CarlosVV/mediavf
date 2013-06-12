using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    class YqlQueryProvider : IYqlQueryProvider
    {
        #region Fields

        /// <summary>
        /// The Yahoo market data settings
        /// </summary>
        private readonly IYahooMarketDataSettings _marketDataSettings;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YqlQueryProvider"/>
        /// </summary>
        public YqlQueryProvider(IYahooMarketDataSettings marketDataSettings)
        {
            _marketDataSettings = marketDataSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the YQL used to selected data for multiple stock quotes
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public string GetMultiStockQuoteSelect(IEnumerable<string> symbols)
        {
            // ensure symbols are not null
            if (symbols == null)
                throw new ArgumentNullException("symbols");

            // create select
            return string.Format(_marketDataSettings.YqlMultiQuoteStockSelect,
                string.Join(",", symbols.Select(t => string.Format("\"{0}\"", t))));
        }

        #endregion
    }
}