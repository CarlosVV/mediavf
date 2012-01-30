using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaVF.Entities.ArtistTrack
{
    [DataContract]
    public class Regex
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int ModuleID { get; set; }

        [DataMember]
        public string RegexPattern { get; set; }

        [DataMember]
        public string MatchType { get; set; }

        [DataMember]
        public List<RegexCapture> Captures { get; set; }
    }
}
