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
        private readonly ICsvTagProvider _tagProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="CsvUrlProvider"/>
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="tagProvider"></param>
        public CsvUrlProvider(IYahooMarketDataSettings settings, ICsvTagProvider tagProvider)
        {
            _settings = settings;
            _tagProvider = tagProvider;
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
            List<string> tagList = _tagProvider.GetTags().ToList();
            if (tagList.Count < 1)
                throw new NoCsvTagsProvidedException();

            // get the url for retrieving the csv
            return string.Format(_settings.CsvUrlFormat,
                string.Join("+", symbolList),
                string.Join(string.Empty, _tagProvider.GetTags()));
        }

        #endregion
    }
}