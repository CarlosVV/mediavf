using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using MediaVF.Common.Configuration.Facebook;
using MediaVF.Entities.ArtistTrack;
using MediaVF.Common.Communication.Utilities;

namespace MediaVF.Web.ArtistTrack.Server.Admin
{
    public static class FacebookManager
    {
        #region Access Token

        /// <summary>
        /// Gets an access token from Facebook
        /// </summary>
        public static string GetFacebookAccessToken(string authorizationCode)
        {
            // get Facebook values from config
            FacebookSection facebookSection = ConfigurationManager.GetSection("facebookConfig") as FacebookSection;

            // make call to get access token
            return GetFacebookAccessToken(facebookSection.AccessTokenUrl,
                facebookSection.ApplicationID,
                facebookSection.RedirectUrl,
                facebookSection.ApplicationSecret,
                authorizationCode);
        }

        /// <summary>
        /// Gets an access token by making a web request to the URL built from the given parameters
        /// </summary>
        /// <param name="accessTokenUrl">The base access token URL</param>
        /// <param name="applicationID">The application ID on Facebook</param>
        /// <param name="redirectUrl">The redirect URL to be directed to after completion</param>
        /// <param name="applicationSecret">The application secret</param>
        /// <param name="authorizationCode">The user-specific authorization code</param>
        /// <returns></returns>
        private static string GetFacebookAccessToken(string accessTokenUrl, string applicationID, string redirectUrl, string applicationSecret, string authorizationCode)
        {
            //// decrypt the authorization code
            //authorizationCode = EncryptionUtility.Decrypt(ServerPasswordKey, authorizationCode);

            // create the access token url
            StringBuilder accessUrl = new StringBuilder();
            accessUrl.Append(accessTokenUrl).Append("?")
                     .Append("client_id=").Append(applicationID)
                     .Append("&redirect_uri=").Append(redirectUrl)
                     .Append("&client_secret=").Append(applicationSecret)
                     .Append("&code=").Append(authorizationCode);

            // initialize the response to empty
            string accessResponseText = string.Empty;

            // create web request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(accessUrl.ToString());

            // make request to Facebook and read into plain text
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                accessResponseText = reader.ReadToEnd();
            }

            // response should be the value after the last equals sign
            string accessToken = string.Empty;
            if (!string.IsNullOrEmpty(accessResponseText) && accessResponseText.Contains('='))
                accessToken = accessResponseText.Split('=').Last();

            return accessToken;
        }

        #endregion

        #region Arttists
        
        /// <summary>
        /// Gets artists from a user's Facebook
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static IEnumerable<Artist> GetArtists(string accessToken)
        {
            // build url for requesting info from Facebook
            string musicUrl = string.Format("https://graph.facebook.com/me/music?access_token={0}", accessToken);

            string musicResponseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(musicUrl.ToString());
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                musicResponseText = reader.ReadToEnd();
            }

            /*JObject jObj = JObject.Parse(musicResponseText);

            int id = 0;
            JToken dataToken = jObj["data"];
            IEnumerable<Artist> artists = dataToken.Children().Select(artistToken =>
                new Artist()
                {
                    ID = id++,
                    FacebookID = long.Parse((string)artistToken["id"]),
                    Name = (string)artistToken["name"]
                }
            );

            return artists.AsQueryable();*/
            return null;
        }

        #endregion

    }
}