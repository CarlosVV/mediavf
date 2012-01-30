using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaVF.Entities.ArtistTrack
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public bool SyncWithFacebook { get; set; }

        [DataMember]
        public string FacebookEmail { get; set; }

        [DataMember]
        public string FacebookPassword { get; set; }

        [DataMember]
        public string FacebookAccessToken { get; set; }
    }
}
