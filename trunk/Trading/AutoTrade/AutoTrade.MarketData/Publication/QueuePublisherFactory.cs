using AutoTrade.Core.Msmq;

namespace AutoTrade.MarketData.Publication
{
    class QueuePublisherFactory : IPublisherFactory
    {
        /// <summary>
        /// The message queue factory
        /// </summary>
        private readonly IMessageQueueFactory _messageQueueFactory;

        /// <summary>
        /// Instantiates a <see cref="QueuePublisherFactory"/>
        /// </summary>
        /// <param name="messageQueueFactory"></param>
        public QueuePublisherFactory(IMessageQueueFactory messageQueueFactory)
        {
            _messageQueueFactory = messageQueueFactory;
        }

        /// <summary>
        /// Creates a publisher for a data type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPublisher<T> CreatePublisher<T>()
        {
            return new QueuePublisher<T>(_messageQueueFactory);
        }
    }
}