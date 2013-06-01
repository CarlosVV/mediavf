using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core.Web;
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

            // set up fake urlProvider
            var urlProvider = A.Fake<IUrlProvider>();
            A.CallTo(() => urlProvider.GetUrl(A<IEnumerable<string>>.Ignored)).Returns("TestUrl");

            // set up fake request executor
            var webRequestExecutor = A.Fake<IWebRequestExecutor>();
            A.CallTo(() => webRequestExecutor.ExecuteRequest(A<string>.Ignored)).Returns("TestResult");

            // set up fake translator
            var yqlResultTranslator = A.Fake<IResultTranslator>();
            A.CallTo(() => yqlResultTranslator.TranslateResultsToQuotes("TestResult")).Returns(expectedQuotes);

            // create Yahoo market data provider
            var yahooMarketDataProvider = new YahooMarketDataProvider(urlProvider, webRequestExecutor, yqlResultTranslator);

            // get quotes from provider
            IEnumerable<StockQuote> actualQuotes = yahooMarketDataProvider.GetQuotes(stocks);

            // check that a call was made to get the stock quote select
            A.CallTo(() => urlProvider.GetUrl(A<IEnumerable<string>>.Ignored)).MustHaveHappened();
            A.CallTo(() => webRequestExecutor.ExecuteRequest("TestUrl")).MustHaveHappened();
            A.CallTo(() => yqlResultTranslator.TranslateResultsToQuotes("TestResult")).MustHaveHappened();

            // check that quotes returned are the same as the quotes returned by the result translator
            actualQuotes.Should().OnlyContain(q => expectedQuotes.Contains(q));
        }
    }
}
