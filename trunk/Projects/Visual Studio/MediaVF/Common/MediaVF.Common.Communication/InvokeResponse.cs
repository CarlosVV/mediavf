using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MediaVF.Common.Communication
{
    [DataContract]
    public class InvokeResponse
    {
        [DataMember]
        public string OperationResult { get; set; }

        [DataMember]
        public Exception Error { get; set; }

        public T GetValue<T>()
        {
            SerializationUtility serializer = new SerializationUtility();
            return (T)serializer.Deserialize(typeof(T), OperationResult);
        }
    }
}
