using System;
using System.Collections.Generic;

namespace AutoTrade.Core.Msmq
{
    public interface IMessageQueue<T> where T : class
    {
        /// <summary>
        /// Event raised when new messages are found in the queue
        /// </summary>
        event EventHandler<IEnumerable<T>> NewMessagesFound;

        /// <summary>
        /// Adds a payload to the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        void Add(T payload);

        /// <summary>
        /// Starts reading messages off the queue
        /// </summary>
        void StartReadingMessages(TimeSpan interval);

        /// <summary>
        /// Stops reading messages off the queue
        /// </summary>
        void StopReadingMessages();
    }
}
