
namespace ExternalServicesPOC.Web
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Text;

    using MediaVF.Common.Communication;
    using MediaVF.Common.Entities;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Configuration;
    using MediaVF.Common.Communication.Utilities;


    // TODO: Create methods containing your application logic.
    [EnableClientAccess]
    public class FacebookService : DomainService
    {
        string _encryptionKey;
        string EncryptionKey
        {
            get
            {
                if (string.IsNullOrEmpty(_encryptionKey))
                    _encryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
                return _encryptionKey;
            }
        }

        [Invoke]
        public string GetAccessToken(string accessTokenUrl, string applicationID, string redirectUrl, string applicationSecret, string authorizationCode)
        {
            authorizationCode = EncryptionUtility.Decrypt(EncryptionKey, authorizationCode);

            StringBuilder accessUrl = new StringBuilder();
            accessUrl.Append(accessTokenUrl).Append("?")
                     .Append("client_id=").Append(applicationID)
                     .Append("&redirect_uri=").Append(redirectUrl)
                     .Append("&client_secret=").Append(applicationSecret)
                     .Append("&code=").Append(authorizationCode);

            string accessResponseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(accessUrl.ToString());
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                accessResponseText = reader.ReadToEnd();
            }

            string accessToken = string.Empty;
            if (!string.IsNullOrEmpty(accessResponseText) && accessResponseText.Contains('='))
                accessToken = accessResponseText.Split('=').Last();

            return accessToken;
        }

        [Query]
        public IQueryable<Band> GetBands(string accessToken)
        {
            accessToken = EncryptionUtility.Decrypt(EncryptionKey, accessToken);

            string musicUrl = string.Format("https://graph.facebook.com/me/music?access_token={0}", accessToken);

            string musicResponseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(musicUrl.ToString());
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                musicResponseText = reader.ReadToEnd();
            }

            JObject jObj = JObject.Parse(musicResponseText);

            int id = 0;
            JToken dataToken = jObj["data"];
            IEnumerable<Band> bands = dataToken.Children().Select(artistToken =>
                new Band()
                {
                    ID = id++,
                    FacebookID = long.Parse((string)artistToken["id"]),
                    Name = (string)artistToken["name"]
                }
            );
            
            return bands.AsQueryable();
        }
    }
}


