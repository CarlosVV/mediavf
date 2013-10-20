using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MSFT = System.Messaging;

namespace AutoTrade.Core.Msmq
{
    public class MessageQueue<T> : IMessageQueue<T> where T : class
    {
        #region Constants

        /// <summary>
        /// The part of the path that indicates the queue is private
        /// </summary>
        private const string PrivateQueuePathPart = "Private$";

        /// <summary>
        /// The delimiter between parts of the queue path
        /// </summary>
        private const string PathDelimiter = "\\";

        /// <summary>
        /// The local machine identifier
        /// </summary>
        private const string LocalMachineIdentifier = ".";

        #endregion

        #region Fields

        /// <summary>
        /// The underlying MSMQ to send and receive messages
        /// </summary>
        private readonly System.Messaging.MessageQueue _underlyingQueue;

        /// <summary>
        /// Timer for regularly pulling messages off the queue
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// The path to the queue
        /// </summary>
        private readonly string _path;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a MessageQueue
        /// </summary>
        /// <param name="name"></param>
        /// <param name="machineName"></param>
        /// <param name="isPrivate"></param>
        public MessageQueue(string name, string machineName = null, bool isPrivate = false)
        {
            _timer = new Timer();
            _timer.Elapsed += TimerElapsed;

            _path = BuildQueuePath(name, machineName, isPrivate);

            _underlyingQueue = !MSFT.MessageQueue.Exists(Path) ? MSFT.MessageQueue.Create(_path) : new MSFT.MessageQueue(_path);
            _underlyingQueue.Formatter = new MessageFormatter<T>();
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when new messages are found in the queue
        /// </summary>
        public event EventHandler<IEnumerable<T>> NewMessagesFound;

        /// <summary>
        /// The path to the queue
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Raises the <see cref="NewMessagesFound"/> event
        /// </summary>
        /// <param name="messages"></param>
        protected virtual void OnNewMessagesFound(IEnumerable<T> messages)
        {
            if (NewMessagesFound != null)
                NewMessagesFound(this, messages);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the path to the queue
        /// </summary>
        /// <param name="name"></param>
        /// <param name="machineName"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        private static string BuildQueuePath(string name, string machineName, bool isPrivate)
        {
            // create builder
            var path = new StringBuilder();

            // add machine name
            path.Append(!string.IsNullOrWhiteSpace(machineName) ? machineName : LocalMachineIdentifier)
                .Append(PathDelimiter);

            // add private qualifier, if necessary
            if (isPrivate) path.Append(PrivateQueuePathPart).Append(PathDelimiter);

            // append the name of the queue
            path.Append(name);

            return path.ToString();
        }

        /// <summary>
        /// Adds a payload to the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        public void Add(T payload)
        {
            var message = new MSFT.Message(payload, new MessageFormatter<T>()) {Recoverable = true};

            _underlyingQueue.Send(message);
        }

        /// <summary>
        /// Starts reading messages off the queue
        /// </summary>
        public void StartReadingMessages(TimeSpan interval)
        {
            _timer.Interval = interval.TotalMilliseconds;
            _timer.Start();
        }

        /// <summary>
        /// Stops reading messages off the queue
        /// </summary>
        public void StopReadingMessages()
        {
            _timer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            var messages = _underlyingQueue.GetAllMessages();

            if (messages.Any())
                OnNewMessagesFound(messages.Select(GetMessagePayload));
        }

        /// <summary>
        /// Gets the payload of a payload
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static T GetMessagePayload(System.Messaging.Message message)
        {
            return message.Body as T;
        }

        #endregion
    }
}