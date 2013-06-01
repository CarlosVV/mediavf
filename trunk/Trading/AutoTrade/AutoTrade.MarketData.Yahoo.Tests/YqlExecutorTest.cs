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

            // create executor
            var yqlExecutor = new YqlExecutor(settings);

            // execute query
            string result = yqlExecutor.ExecuteYqlQuery(TestYql);

            // check that a result was returned
            result.Should().NotBeBlank();
        }
    }
}
