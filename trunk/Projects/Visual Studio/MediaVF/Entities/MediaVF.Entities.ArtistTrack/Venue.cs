using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaVF.Entities.ArtistTrack
{
    [DataContract]
    public class Venue
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string StateOrProvince { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

    }
}
