namespace AutoTrade.MarketData.Publication
{
    public interface IPublisherFactory
    {
        /// <summary>
        /// Creates a publisher for a data type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IPublisher<T> CreatePublisher<T>();
    }
}
