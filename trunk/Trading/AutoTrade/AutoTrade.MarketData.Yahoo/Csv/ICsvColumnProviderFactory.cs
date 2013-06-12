namespace AutoTrade.MarketData.Yahoo.Csv
{
    public interface ICsvColumnProviderFactory
    {
        /// <summary>
        /// Gets a CSV column provider
        /// </summary>
        /// <returns></returns>
        ICsvColumnProvider GetColumnProvider();
    }
}
