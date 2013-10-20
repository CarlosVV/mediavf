using System.IO;
using System.Messaging;
using System.Runtime.Serialization;

namespace AutoTrade.Core.Msmq
{
    public class MessageFormatter<T> : IMessageFormatter where T : class
    {
        /// <summary>
        /// Gets a new message formatter of the same type
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new MessageFormatter<T>();
        }

        /// <summary>
        /// Checks if a message can be read by the formatter
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CanRead(Message message)
        {
            return message.BodyStream != null;
        }

        /// <summary>
        /// Reads the body of a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public object Read(Message message)
        {
            // create serializer for type
            var serializer = new DataContractSerializer(typeof(T));

            // get the body of the message as text from the message's body stream
            return serializer.ReadObject(message.BodyStream);
        }

        /// <summary>
        /// Writes the body of a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="obj"></param>
        public void Write(Message message, object obj)
        {
            // create serializer for type
            var serializer = new DataContractSerializer(typeof (T));

            // deserialize the object
            message.BodyStream = new MemoryStream();
            serializer.WriteObject(message.BodyStream, obj);
        }
    }
}