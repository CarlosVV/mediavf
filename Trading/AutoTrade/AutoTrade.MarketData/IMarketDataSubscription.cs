namespace AutoTrade.MarketData
{
    public interface IMarketDataSubscription
    {
        /// <summary>
        /// Starts regular updates of data
        /// </summary>
        void Start();

        /// <summary>
        /// Stops regular updates of data
        /// </summary>
        void Stop();
    }
}