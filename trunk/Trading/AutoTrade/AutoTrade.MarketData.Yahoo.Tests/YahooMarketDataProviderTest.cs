using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Core;

namespace AutoTrade.MarketData.Yahoo.Tests
{
    [TestClass]
    public class YahooMarketDataProviderTest
    {
        /// <summary>
        /// Tests getting quotes
        /// </summary>
        [TestMethod]
        public void GetQuotes()
        {
            // create list of stocks
            var stocks = new[] { new Stock { Symbol = "MSFT" }, new Stock { Symbol = "YHOO" } };
            var expectedQuotes = new[] { new StockQuote { Symbol = "MSFT" }, new StockQuote { Symbol = "YHOO" } };

            // set up fake logger
            var logger = A.Fake<ILogger>();

            // set up first fake data provider
            var marketDataProvider1 = A.Fake<IYahooMarketDataProvider>();
            A.CallTo(() => marketDataProvider1.GetQuotes(A<IEnumerable<Stock>>.Ignored))
             .Throws<Exception>();

            // set up second fake data provider
            var marketDataProvider2 = A.Fake<IYahooMarketDataProvider>();
            A.CallTo(() => marketDataProvider2.GetQuotes(A<IEnumerable<Stock>>.Ignored))
             .Returns(expectedQuotes);

            // create Yahoo market data provider
            var yahooMarketDataProvider = new YahooMarketDataProvider(logger, new[] { marketDataProvider1, marketDataProvider2 });

            // get quotes from provider
            IEnumerable<StockQuote> actualQuotes = yahooMarketDataProvider.GetQuotes(stocks);

            // check that a call was made to get the stock quote select
            A.CallTo(() => marketDataProvider1.GetQuotes(stocks)).MustHaveHappened();
            A.CallTo(() => marketDataProvider2.GetQuotes(stocks)).MustHaveHappened();

            A.CallTo(() => logger.Log(A<LoggingEvent>.Ignored)).MustHaveHappened();

            // check that quotes returned are the same as the quotes returned by the result translator
            actualQuotes.Should().OnlyContain(q => expectedQuotes.Contains(q));
        }
    }
}
