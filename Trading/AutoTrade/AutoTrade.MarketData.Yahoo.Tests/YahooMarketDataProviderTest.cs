using System.Collections.Generic;
using System.Linq;
using AutoTrade.MarketData.Yahoo.Yql;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            // setup fake provider
            var yqlProvider = A.Fake<IYqlProvider>();
            A.CallTo(() => yqlProvider.GetMultiStockQuoteSelect(A<IEnumerable<string>>.Ignored)).Returns("TestSelect");

            // set up fake executor
            var yqlExecutor = A.Fake<IYqlExecutor>();
            A.CallTo(() => yqlExecutor.ExecuteYqlQuery("TestSelect")).Returns("TestResult");

            // set up fake translator
            var yqlResultTranslator = A.Fake<IYqlResultTranslator>();
            A.CallTo(() => yqlResultTranslator.TranslateResultsToQuotes("TestResult")).Returns(expectedQuotes);

            // create Yahoo market data provider
            var yahooMarketDataProvider = new YahooMarketDataProvider(yqlProvider, yqlExecutor, yqlResultTranslator);

            // get quotes from provider
            IEnumerable<StockQuote> actualQuotes = yahooMarketDataProvider.GetQuotes(stocks);

            // check that a call was made to get the stock quote select
            A.CallTo(() => yqlProvider.GetMultiStockQuoteSelect(A<IEnumerable<string>>.Ignored)).MustHaveHappened();
            A.CallTo(() => yqlExecutor.ExecuteYqlQuery("TestSelect")).MustHaveHappened();
            A.CallTo(() => yqlResultTranslator.TranslateResultsToQuotes("TestResult")).MustHaveHappened();

            // check that quotes returned are the same as the quotes returned by the result translator
            actualQuotes.Should().OnlyContain(q => expectedQuotes.Contains(q));
        }
    }
}
