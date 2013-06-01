using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    class YqlQueryProvider : IYqlQueryProvider
    {
        #region Constants

        /// <summary>
        /// The name of the setting for getting the YQL query for quotes for multiple stocks
        /// </summary>
        private const string MultiQuoteStockSelectSettingName = "YqlMultiQuoteStockSelect";

        /// <summary>
        /// The format for creating YQL query for getting quotes for multiple stocks
        /// </summary>
        private const string DefaultMultiStockQuoteSelectFormat = @"select * from yahoo.finance.quotes where symbol in ({0})";

        #endregion

        #region Fields

        /// <summary>
        /// The app settings provider
        /// </summary>
        private readonly IAppSettingsProvider _appSettingsProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YqlQueryProvider"/>
        /// </summary>
        /// <param name="appSettingsProvider"></param>
        public YqlQueryProvider(IAppSettingsProvider appSettingsProvider)
        {
            _appSettingsProvider = appSettingsProvider;
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

            // get the format for the query to select quotes
            string stockQuoteSelectFormat = _appSettingsProvider.GetSetting(MultiQuoteStockSelectSettingName, DefaultMultiStockQuoteSelectFormat);

            // create select
            return string.Format(stockQuoteSelectFormat, string.Join(",", symbols.Select(t => string.Format("\"{0}\"", t))));
        }

        #endregion
    }
}