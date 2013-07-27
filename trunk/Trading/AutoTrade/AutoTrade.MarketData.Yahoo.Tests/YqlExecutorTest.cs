using System.Collections.Generic;
using AutoTrade.MarketData.Yahoo.Yql;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTrade.MarketData.Yahoo.Tests
{
    [TestClass]
    public class YqlExecutorTest
    {
        private const string TestYql = "select * from yahoo.finance.quotes where symbol in (\"YHOO\",\"AAPL\",\"GOOG\",\"MSFT\")";

        [TestMethod]
        public void ExecuteYqlQuery()
        {
            // set up fake app settings provider
            var settings = A.Fake<IYahooMarketDataSettings>();
            A.CallTo(() => settings.YqlUrlFormat)
             .Returns("http://query.yahooapis.com/v1/public/yql?q={0}&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");

            // set up fake query provider
            var queryProvider = A.Fake<IYqlQueryProvider>();
            A.CallTo(() => queryProvider.GetMultiStockQuoteSelect(A<IEnumerable<string>>.Ignored)).Returns(TestYql);

            // create urlProvider
            var yqlExecutor = new YqlUrlProvider(settings, queryProvider);

            // execute query
            string result = yqlExecutor.GetQuotesUrl(A<IEnumerable<string>>.Ignored);

            // check that a result was returned
            result.Should().NotBeBlank();
        }
    }
}
