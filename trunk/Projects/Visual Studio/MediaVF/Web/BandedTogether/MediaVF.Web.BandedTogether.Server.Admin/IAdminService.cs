using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using MediaVF.Entities.ArtistTrack;

namespace MediaVF.Web.BandedTogether.Server.Admin
{
    public interface IAdminService
    {
        User Login(string email, string password, string passwordKey);

        bool CheckForExistingUser(string email);

        void Register(User user, string passwordKey, string facebookAuthCode);
    }
}
