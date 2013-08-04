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
        /// <param name="resultTranslator"></param>
        public CsvMarketDataProvider(ICsvUrlProvider urlProvider,
            IWebRequestExecutor webRequestExecutor,
            ICsvResultTranslator resultTranslator)
            : base(urlProvider, webRequestExecutor, resultTranslator, resultTranslator)
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
