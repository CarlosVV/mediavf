using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.MarketData.Yahoo.Csv.Exceptions;
using AutoTrade.MarketData.Yahoo.Exceptions;

namespace AutoTrade.MarketData.Yahoo.Csv
{
    public class CsvUrlProvider : ICsvUrlProvider
    {
        #region Fields

        /// <summary>
        /// The app settings provider
        /// </summary>
        private readonly IYahooMarketDataSettings _settings;

        /// <summary>
        /// The provider of tags for the CSV query
        /// </summary>
        private readonly ICsvColumnProvider _columnProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="CsvUrlProvider"/>
        /// </summary>
        /// <param name="settings"></param>
        /// <param>
        ///     <name>columnProvider</name>
        /// </param>
        /// <param name="columnProviderFactory"></param>
        public CsvUrlProvider(IYahooMarketDataSettings settings, ICsvColumnProviderFactory columnProviderFactory)
        {
            _settings = settings;
            _columnProvider = columnProviderFactory.GetColumnProvider();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the url for a csv of quotes from Yahoo
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public string GetUrl(IEnumerable<string> symbols)
        {
            // ensure symbols are not null
            if (symbols == null)
                throw new ArgumentNullException("symbols");

            // enumerate collection
            List<string> symbolList = symbols.ToList();
            if (symbolList.Count < 1)
                throw new NoSymbolsProvidedException();

            // get tags for query
            string tagsString = _columnProvider.GetTagsString();
            if (string.IsNullOrWhiteSpace(tagsString))
                throw new NoCsvTagsProvidedException();

            // get the url for retrieving the csv
            return string.Format(_settings.CsvUrlFormat, string.Join("+", symbolList), tagsString);
        }

        #endregion
    }
}