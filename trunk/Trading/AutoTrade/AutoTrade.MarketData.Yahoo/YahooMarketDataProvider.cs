using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core;
using AutoTrade.Core.StockData;
using AutoTrade.MarketData.Data;
using AutoTrade.MarketData.Yahoo.Exceptions;
using AutoTrade.MarketData.Yahoo.Properties;
using log4net;

namespace AutoTrade.MarketData.Yahoo
{
    [Name(Yahoo)]
    public class YahooMarketDataProvider : IMarketDataProvider, IStockDataProvider
    {
        #region Constants

        /// <summary>
        /// The name of the provider
        /// </summary>
        private const string Yahoo = "Yahoo";

        #endregion

        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger;

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
        public YahooMarketDataProvider(ILog logger, IEnumerable<IYahooMarketDataProvider> providers)
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
                    _logger.Warn(
                        string.Format(Resources.FailedToRetrieveFromProviderMessageFormat,provider.GetType().FullName), ex);

                }
            }

            throw new FailedToRetrieveDataException<StockQuote>();
        }

        /// <summary>
        /// Gets stock data for a collection of symbols
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public IEnumerable<StockData> GetStockData(IEnumerable<string> symbols)
        {
            foreach (var provider in _providers.OrderBy(p => p.Precedence))
            {
                try
                {
                    return provider.GetStockData(symbols);
                }
                catch (Exception ex)
                {
                    _logger.Warn(
                        string.Format(Resources.FailedToRetrieveFromProviderMessageFormat, provider.GetType().FullName), ex);

                }
            }

            throw new FailedToRetrieveDataException<StockData>();
        }

        #endregion
    }
}
