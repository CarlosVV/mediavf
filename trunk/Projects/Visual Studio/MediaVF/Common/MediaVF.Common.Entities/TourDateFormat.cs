using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaVF.Common.Entities
{
    [DataContract]
    public class TourDateFormat
    {
        [DataMember]
        public int FeedID { get; set; }

        [DataMember]
        public string DateFormat { get; set; }

        [DataMember]
        public string LocationFormat { get; set; }

        [DataMember]
        public bool LocationBeforeDate { get; set; }

        [DataMember]
        public bool TrailingSupportActs { get; set; }
    }
}
