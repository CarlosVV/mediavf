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
        /// <param name="resultTranslator"></param>
        public YqlMarketDataProvider(IYqlUrlProvider urlProvider,
            IWebRequestExecutor webRequestExecutor,
            IYqlResultTranslator resultTranslator)
            : base(urlProvider, webRequestExecutor, resultTranslator)
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
