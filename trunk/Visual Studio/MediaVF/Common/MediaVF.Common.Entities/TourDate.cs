using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaVF.Common.Entities
{
    [DataContract]
    public class TourDate
    {
        [DataMember]
        public int BandID { get; set; }

        [DataMember]
        public int? VenueID { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string StateOrProvince { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public DateTime? DoorsOpen { get; set; }
    }
}
