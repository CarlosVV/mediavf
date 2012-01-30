using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MediaVF.Entities.ArtistTrack
{
    [DataContract]
    public class Band
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public long FacebookID { get; set; }

        [DataMember]
        public string Hometown { get; set; }

        [DataMember]
        public int LabelID { get; set; }

        [DataMember]
        public Label Label { get; set; }

        [DataMember]
        public List<int> GenreIDs { get; set; }

        [DataMember]
        public List<BandGenre> Genres { get; set; }
    }
}
