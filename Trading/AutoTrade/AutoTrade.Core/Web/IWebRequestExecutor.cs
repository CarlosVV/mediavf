namespace AutoTrade.Core.Web
{
    public interface IWebRequestExecutor
    {
        /// <summary>
        /// Executes a web request and returns the response
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string ExecuteRequest(string url);
    }
}
