namespace AutoTrade.MarketData.Yahoo.Yql
{
    public interface IYqlExecutor
    {
        /// <summary>
        /// Executes a YQL query and returns the raw xml result
        /// </summary>
        /// <param name="yql"></param>
        /// <returns></returns>
        string ExecuteYqlQuery(string yql);
    }
}
