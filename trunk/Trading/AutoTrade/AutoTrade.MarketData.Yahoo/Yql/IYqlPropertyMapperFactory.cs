namespace AutoTrade.MarketData.Yahoo.Yql
{
    public interface IYqlPropertyMapperFactory
    {
        /// <summary>
        /// Gets a property mapper
        /// </summary>
        /// <returns></returns>
        IYqlPropertyMapper GetPropertyMapper();
    }
}
