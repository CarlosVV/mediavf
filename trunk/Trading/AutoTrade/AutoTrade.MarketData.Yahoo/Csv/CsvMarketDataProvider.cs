using AutoTrade.Core.Web;

namespace AutoTrade.MarketData.Yahoo.Csv
{
    public class CsvMarketDataProvider : YahooMarketDataProviderBase
    {
        /// <summary>
        /// Instantiates a <see cref="CsvMarketDataProvider"/>
        /// </summary>
        /// <param name="urlProvider"></param>
        /// <param name="webRequestExecutor"></param>
        /// <param name="quotesResultTranslator"></param>
        public CsvMarketDataProvider(ICsvUrlProvider urlProvider,
            IWebRequestExecutor webRequestExecutor,
            ICsvResultTranslator quotesResultTranslator)
            : base(urlProvider, webRequestExecutor, quotesResultTranslator)
        {   
        }

        /// <summary>
        /// Gets the precedence
        /// </summary>
        public override int Precedence
        {
            get { return 2; }
        }
    }
}
