using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;

using MediaVF.Common.Communication.Utilities;
using MediaVF.Common.Configuration.Facebook;
using MediaVF.Entities.ArtistTrack;
using MediaVF.Services.Components;
using MediaVF.Services.Configuration;
using MediaVF.Services.Logging;
using MediaVF.Common.Utilities;

namespace MediaVF.Web.ArtistTrack.Server.Admin
{
    public class AdminServiceComponent : ServiceComponent, IAdminService
    {
        #region Constants

        const string USER_NOT_FOUND = "Email address not found. Please click register if you are a new user.";
        const string INVALID_PASSWORD = "Invalid password.";
        const string USER_EXISTS = "An account already exists for the given email address.";

        #endregion

        #region Properties

        List<User> _loggedOnUsers;
        public List<User> LoggedOnUsers
        {
            get
            {
                if (_loggedOnUsers == null)
                    _loggedOnUsers = new List<User>();
                return _loggedOnUsers;
            }
        }

        string _serverPasswordKey;
        string ServerPasswordKey
        {
            get
            {
                if (string.IsNullOrEmpty(_serverPasswordKey))
                    _serverPasswordKey = ConfigManager.Components[GetType()].Settings["PasswordServerKey"].Value;
                return _serverPasswordKey;
            }
        }

        #endregion

        #region Constructor

        public AdminServiceComponent(IServiceConfigManager configManager, IComboLog log)
            : base(configManager, log) { }

        #endregion

        #region ServiceComponent Implementation

        /// <summary>
        /// Overridden to do nothing
        /// </summary>
        protected override void RegisterTypes() { }

        /// <summary>
        /// Overridden to do nothing
        /// </summary>
        public override void Run() { }

        #endregion

        #region Methods

        /// <summary>
        /// Logs in a user with the given user name and password, using the given key to decrypt the password
        /// </summary>
        /// <param name="userName">The name of the user logging on</param>
        /// <param name="password">The password of the user logging on</param>
        /// <param name="passwordKey">The key to decrypt to the password</param>
        /// <returns>The logged-on user</returns>
        public User Login(string userName, string password, string passwordKey)
        {
            // check that a user with the given user name exists
            List<User> allUsers = DataManager.GetDataContext<User>().GetAll<User>();
            User user = allUsers.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
                throw new Exception(USER_NOT_FOUND);

            // decrypt the server password
            string storedPassword = EncryptionUtility.Decrypt(ServerPasswordKey, user.Password);

            // decrypt the given password
            string givenPassword = EncryptionUtility.Decrypt(passwordKey, password);

            // check that the passwords match
            if (storedPassword != givenPassword)
                throw new Exception(INVALID_PASSWORD);

            // add to logged on users
            if (!LoggedOnUsers.Any(u => u.Email == user.Email))
                LoggedOnUsers.Add(user);

            return user;
        }

        /// <summary>
        /// Checks if a user exists with the given user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckForExistingUser(string userName)
        {
            List<User> allUsers = DataManager.GetDataContext<User>().GetAll<User>();
            return allUsers.Any(u => u.UserName == userName);
        }

        /// <summary>
        /// Registers a new user using the given password key to decrypt their password
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="passwordKey"></param>
        public void Register(User newUser, string passwordKey, string facebookAuthCode)
        {
            // check if user already exists
            List<User> allUsers = DataManager.GetDataContext<User>().GetAll<User>();
            if (allUsers.Any(u => u.UserName == newUser.UserName))
                throw new Exception(USER_EXISTS);

            if (newUser.SyncWithFacebook)
                newUser.FacebookAccessToken = FacebookManager.GetFacebookAccessToken(facebookAuthCode);

            // decrypt password
            string decrypted = EncryptionUtility.Decrypt(passwordKey, newUser.Password);
            
            // re-encrypt password with server key

            string serverEncrypt = EncryptionUtility.Encrypt(ServerPasswordKey, decrypted);

            // set the user's password to the server-encrypted version
            newUser.Password = serverEncrypt;

            // add the users to the data context and save
            DataManager.GetDataContext<User>().AddObjects<User>(new User[] { newUser }.ToList());
            DataManager.GetDataContext<User>().Save();

            // add the user the logged on users
            if (!LoggedOnUsers.Any(u => u.Email == newUser.Email))
                LoggedOnUsers.Add(newUser);
        }

        #endregion
    }
}