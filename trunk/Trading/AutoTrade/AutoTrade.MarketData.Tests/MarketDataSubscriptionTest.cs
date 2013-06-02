using System;
using System.Collections.Generic;
using System.Data.Entity;
using AutoTrade.MarketData.Entities;
using AutoTrade.Tests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;
using log4net;

namespace AutoTrade.MarketData.Tests
{
    [TestClass]
    public class MarketDataSubscriptionTest
    {
        [TestMethod]
        public void GetLatestData()
        {
            // create fakes
            var logger = A.Fake<ILog>();
            var marketDataProvider = A.Fake<IMarketDataProvider>();
            var marketDataRepository = A.Fake<IMarketDataRepository>();

            var stockQuoteSet = A.Fake<IDbSet<StockQuote>>();
            marketDataRepository.StockQuotes = stockQuoteSet;

            // create subscriptionData
            var stocks = new List<Stock>();
            var subscription = new Subscription { Stocks = stocks, UpdateInterval = TimeSpan.FromMilliseconds(int.MaxValue) };

            // create fake return value
            var stockQuotes = new List<StockQuote>
                {
                    new StockQuote { AskPrice = 1.0m, BidPrice = 1.2m, Change = 0.01m, OpenPrice = 0.9m, QuoteDateTime = DateTime.Now, Symbol = "TST1" },
                    new StockQuote { AskPrice = 2.0m, BidPrice = 2.2m, Change = 0.02m, OpenPrice = 1.0m, QuoteDateTime = DateTime.Now, Symbol = "TST2" },
                    new StockQuote { AskPrice = 3.0m, BidPrice = 3.2m, Change = 0.03m, OpenPrice = 1.1m, QuoteDateTime = DateTime.Now, Symbol = "TST3" }
                };

            // hold onto list of quotes added to repository
            var repositoryQuotes = new List<StockQuote>();

            // handle calls to fakes
            A.CallTo(() => marketDataProvider.GetQuotes(subscription.Stocks)).Returns(stockQuotes);
            A.CallTo(() => stockQuoteSet.Add(A<StockQuote>.Ignored))
             .Invokes((StockQuote quote) => repositoryQuotes.Add(quote));

            // create subscriptionData
            var marketDataSubscription = new MarketDataSubscription(logger, marketDataRepository, marketDataProvider, subscription);

            // call get latest data
            marketDataSubscription.InvokeMethod("GetLatestData");

            // ensure call to get data was made
            A.CallTo(() => marketDataProvider.GetQuotes(subscription.Stocks)).MustHaveHappened();
            
            // ensure call to store data was made
            A.CallTo(() => stockQuoteSet.Add(A<StockQuote>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(3));

            // check that quotes were added to the repository
            repositoryQuotes.Should().HaveCount(3);
            repositoryQuotes.Should().OnlyContain(q => stockQuotes.Contains(q));
        }
    }
}
