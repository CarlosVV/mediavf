using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MediaVF.Common.Communication
{
    [DataContract]
    public class InvokeParameter
    {
        [DataMember]
        public string ParameterType { get; set; }

        [DataMember]
        public string SerializedParameter { get; set; }
    }
}
