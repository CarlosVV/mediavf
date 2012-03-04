using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Entities.ArtistTrack;

namespace MediaVF.Web.ArtistTrack.Server.Artists
{
    public interface IArtistsService
    {
        IEnumerable<Artist> GetArtistsForUser(int userID);

        void AddArtistsForUser(int userID, IEnumerable<Artist> artists);
    }
}
