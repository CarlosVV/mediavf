﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.MarketData.Yahoo.Exceptions;
using AutoTrade.MarketData.Yahoo.Properties;
using log4net.Core;
using AutoTrade.Core;

namespace AutoTrade.MarketData.Yahoo
{
    public class YahooMarketDataProvider : IMarketDataProvider
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The providers to use for retrieving market data
        /// </summary>
        private readonly IEnumerable<IYahooMarketDataProvider> _providers;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YahooMarketDataProvider"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="providers"></param>
        public YahooMarketDataProvider(ILogger logger, IEnumerable<IYahooMarketDataProvider> providers)
        {
            _logger = logger;
            _providers = providers;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets quotes using the first provider that successfully returns quotes
        /// </summary>
        /// <param name="stocks"></param>
        /// <returns></returns>
        public IEnumerable<StockQuote> GetQuotes(IEnumerable<Stock> stocks)
        {
            foreach (var provider in _providers.OrderBy(p => p.Precedence))
            {
                try
                {
                    return provider.GetQuotes(stocks);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(Resources.FailedToRetrieveFromProviderMessageFormat, ex, provider.GetType().FullName);
                }
            }

            throw new FailedToRetrieveQuotesException();
        }

        #endregion
    }
}