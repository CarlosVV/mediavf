using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoTrade.Core.StockData;
using AutoTrade.MarketData.Data;
using Microsoft.Practices.Unity;

namespace AutoTrade.MarketData.EmailAlerts.PennyPicks
{
    class PennyPicksEmailStockProvider : IStockListProvider
    {
        #region Constants

        /// <summary>
        /// The name of the parser to use
        /// </summary>
        private const string ParserName = "PennyPicksStockParser";

        #endregion

        #region Fields

        /// <summary>
        /// The email feed
        /// </summary>
        private readonly IEmailFeed _feed;

        /// <summary>
        /// The parser for getting stock symbols from emails
        /// </summary>
        private readonly IEmailStockParser _stockParser;

        /// <summary>
        /// The 
        /// </summary>
        private readonly IStockDataProvider _stockDataProvider;

        /// <summary>
        /// The stock retriever
        /// </summary>
        private readonly IStockRetriever _stockRetriever;

        /// <summary>
        /// The collection of stocks that have been found in the feed
        /// </summary>
        private readonly List<Stock> _stocks = new List<Stock>();

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="PennyPicksEmailStockProvider"/>
        /// </summary>
        /// <param name="emailAlertsAppSettings"></param>
        /// <param name="emailFeedFactory"></param>
        /// <param name="stockParser"></param>
        /// <param name="stockDataProviderFactory"></param>
        /// <param name="stockRetriever"></param>
        public PennyPicksEmailStockProvider(IEmailAlertsAppSettings emailAlertsAppSettings,
            IEmailFeedFactory emailFeedFactory,
            [Dependency(ParserName)] IEmailStockParser stockParser,
            IStockDataProviderFactory stockDataProviderFactory,
            IStockRetriever stockRetriever)
        {
            // check nulls
            if (emailAlertsAppSettings == null) throw new ArgumentNullException("emailAlertsAppSettings");
            if (emailFeedFactory == null) throw new ArgumentNullException("emailFeedFactory");
            if (stockParser == null) throw new ArgumentNullException("stockParser");
            if (stockRetriever == null) throw new ArgumentNullException("stockRetriever");
            
            // create feed
            _feed = emailFeedFactory.CreateFeed(emailAlertsAppSettings.PennyPicksFeedName);
            _feed.NewEmailsFound += FeedOnNewEmailsFound;

            // get the stock data provider
            _stockDataProvider = stockDataProviderFactory.GetStockDataProvider(emailAlertsAppSettings.PennyPicksStockDataProviderName);

            // set stock parser and retriever
            _stockParser = stockParser;
            _stockRetriever = stockRetriever;

            // start feed
            _feed.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles new emails found in the feed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="emailEventArgs"></param>
        private void FeedOnNewEmailsFound(object sender, EmailEventArgs emailEventArgs)
        {
            // check that emails were found
            if (emailEventArgs.Emails == null) return;

            // get symbols from emails
            var symbols = emailEventArgs.Emails.Select(_stockParser.ParseStockSymbol)
                                               .Where(s => !string.IsNullOrWhiteSpace(s))
                                               .ToList();


            // get stocks from symbols
            var stocks = _stockRetriever.GetStocks(_stockDataProvider, symbols);

            // add stocks to collection
            _stocks.AddRange(stocks);
        }

        /// <summary>
        /// Gets the collection of stocks that have been parsed from the email feed
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public IEnumerable<Stock> GetStocks(Subscription subscription)
        {
            return new ReadOnlyCollection<Stock>(_stocks);
        }

        #endregion
    }
}
