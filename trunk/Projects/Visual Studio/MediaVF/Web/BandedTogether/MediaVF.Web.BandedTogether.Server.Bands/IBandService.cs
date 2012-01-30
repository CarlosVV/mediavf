using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Entities.ArtistTrack;

namespace MediaVF.Web.BandedTogether.Server.Bands
{
    public interface IBandsService
    {
        IEnumerable<Band> GetBandsForUser(int userID);

        void AddBandsForUser(int userID, IEnumerable<Band> bands);
    }
}
