namespace AutoTrade.Core.Msmq
{
    public class MessageQueueFactory : IMessageQueueFactory
    {
        /// <summary>
        /// Creates a message queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public IMessageQueue<T> CreatePrivateQueue<T>(string name, string machineName = null) where T : class
        {
            return new MessageQueue<T>(name, machineName, true);
        }
    }
}