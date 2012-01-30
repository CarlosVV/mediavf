using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaVF.Common.Entities
{
    [DataContract]
    public class RegexCapture
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int RegexID { get; set; }

        [DataMember]
        public int CaptureIndex { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
