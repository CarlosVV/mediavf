using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core.StockData;

namespace AutoTrade.MarketData.Data
{
    public class StockRetriever : IStockRetriever
    {
        #region Fields

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IMarketDataRepository _marketDataRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="StockRetriever"/>
        /// </summary>
        /// <param name="marketDataRepository"></param>
        public StockRetriever(IMarketDataRepository marketDataRepository)
        {
            _marketDataRepository = marketDataRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets stocks by their symbols
        /// </summary>
        /// <param name="stockDataProvider"></param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public IEnumerable<Stock> GetStocks(IStockDataProvider stockDataProvider, IEnumerable<string> symbols)
        {
            if (stockDataProvider == null) throw new ArgumentNullException("stockDataProvider");

            // check that symbols were provided
            if (symbols == null) return new List<Stock>();

            // enumerate symbols, if necessary
            var symbolList = symbols as string[] ?? symbols.ToArray();

            // get stocks by symbols
            var stocks = _marketDataRepository.StocksQuery.Where(s => symbolList.Contains(s.Symbol)).ToList();

            // get list of symbols for stocks that were not found
            var missingStocks = symbolList.Except(stocks.Select(s => s.Symbol)).ToList();

            // get data for stocks that are missing
            if (missingStocks.Count > 0)
            {
                var newStocks = stockDataProvider.GetStockData(missingStocks).Select(CreateStock);

                // get data for missing stocks and insert it to the repository
                foreach (var newStock in newStocks)
                    _marketDataRepository.Stocks.Add(newStock);

                // save changes
                _marketDataRepository.SaveChanges();

                // add new stocks
                stocks.AddRange(newStocks);
            }

            return stocks;
        }

        /// <summary>
        /// Creates a <see cref="Stock"/> entity for insertion from a <see cref="StockData"/> object
        /// </summary>
        /// <param name="stockData"></param>
        /// <returns></returns>
        private static Stock CreateStock(StockData stockData)
        {
            var stock = stockData.ToStockEntity();

            stock.Created = DateTime.Now;
            stock.Modified = DateTime.Now;

            return stock;
        }

        #endregion
    }
}