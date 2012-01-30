using System;
using System.Net;
using System.Runtime.Serialization;

namespace MediaVF.Entities.ArtistTrack
{
    [DataContract]
    public class BandGenre
    {
        [DataMember]
        public int BandID { get; set; }

        [DataMember]
        public int GenreID { get; set; }

        [DataMember]
        public Genre Genre { get; set; }
    }
}
