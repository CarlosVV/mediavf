namespace AutoTrade.Core.Msmq
{
    public interface IMessageQueueFactory
    {
        /// <summary>
        /// Creates a message queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="machineName"></param>
        /// <returns></returns>
        IMessageQueue<T> CreatePrivateQueue<T>(string name, string machineName = null) where T : class;
    }
}