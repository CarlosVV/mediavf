using AutoTrade.Core.Web;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    class YqlMarketDataProvider : YahooMarketDataProviderBase
    {
        /// <summary>
        /// Instantiates a <see cref="YqlMarketDataProvider"/>
        /// </summary>
        /// <param name="urlProvider"></param>
        /// <param name="webRequestExecutor"></param>
        /// <param name="quotesResultTranslator"></param>
        public YqlMarketDataProvider(IYqlUrlProvider urlProvider,
            IWebRequestExecutor webRequestExecutor,
            IYqlResultTranslator quotesResultTranslator)
            : base(urlProvider, webRequestExecutor, quotesResultTranslator)
        {
        }

        /// <summary>
        /// Gets the precedence
        /// </summary>
        public override int Precedence
        {
            get { return 1; }
        }
    }
}
