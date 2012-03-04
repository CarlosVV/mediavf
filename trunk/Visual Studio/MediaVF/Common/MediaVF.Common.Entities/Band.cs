using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MediaVF.Common.Entities
{
    [DataContract]
    public class Band
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Hometown { get; set; }

        [DataMember]
        public string RecordLabel { get; set; }
    }
}
