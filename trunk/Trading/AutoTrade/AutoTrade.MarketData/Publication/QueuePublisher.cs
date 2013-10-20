using System;
using AutoTrade.Core.Msmq;

namespace AutoTrade.MarketData.Publication
{
    class QueuePublisher<T> : IPublisher<T>
    {
        /// <summary>
        /// The message queue to which to publish messages
        /// </summary>
        private readonly IMessageQueue<PublicationData<T>> _messageQueue;

        /// <summary>
        /// Instantiates a QueuePublisher
        /// </summary>
        /// <param name="messageQueueFactory"></param>
        public QueuePublisher(IMessageQueueFactory messageQueueFactory)
        {
            // create queue
            _messageQueue = messageQueueFactory.CreatePrivateQueue<PublicationData<T>>(typeof(T).FullName);
        }

        /// <summary>
        /// Publishes data
        /// </summary>
        /// <param name="data"></param>
        public void Publish(T data)
        {
            _messageQueue.Add(new PublicationData<T>
                {
                    Id = Guid.NewGuid(),
                    PublishDateTime = DateTime.Now,
                    Data = data
                });
        }
    }
}