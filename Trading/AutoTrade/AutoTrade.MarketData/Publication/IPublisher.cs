namespace AutoTrade.MarketData.Publication
{
    public interface IPublisher<in T>
    {
        /// <summary>
        /// Publishes data
        /// </summary>
        /// <param name="data"></param>
        void Publish(T data);
    }
}
