using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using MediaVF.Entities.ArtistTrack;

namespace MediaVF.Web.ArtistTrack.Server.Admin
{
    public interface IAdminService
    {
        /// <summary>
        /// Logs a user in to ArtistTracck
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="passwordKey"></param>
        /// <returns></returns>
        User Login(string email, string password, string passwordKey);

        /// <summary>
        /// Checks if a user already exists for the given email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool CheckForExistingUser(string email);

        /// <summary>
        /// Registers a new user in ArtistTrack
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordKey"></param>
        /// <param name="facebookAuthCode"></param>
        void Register(User user, string passwordKey, string facebookAuthCode);
    }
}
