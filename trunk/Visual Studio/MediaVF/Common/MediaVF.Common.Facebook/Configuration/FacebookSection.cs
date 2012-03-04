using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MediaVF.Common.Configuration.Facebook
{
    /// <summary>
    /// Represents a configuration section containing settings for communicating with Facebook
    /// </summary>
    public class FacebookSection : ConfigurationSection
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ID of the application in Facebook
        /// </summary>
        [ConfigurationProperty("applicationID")]
        public string ApplicationID
        {
            get { return (string)base["applicationID"]; }
            set { base["applicationID"] = value; }
        }

        /// <summary>
        /// Gets or sets the application secret code in Facebook
        /// </summary>
        [ConfigurationProperty("applicationSecret")]
        public string ApplicationSecret
        {
            get { return (string)base["applicationSecret"]; }
            set { base["applicationSecret"] = value; }
        }

        /// <summary>
        /// Gets or sets the url for getting a user's authorization for the application
        /// </summary>
        [ConfigurationProperty("authorizationUrl")]
        public string AuthorizationUrl
        {
            get { return (string)base["authorizationUrl"]; }
            set { base["authorizationUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the url for getting an access token for a user
        /// </summary>
        [ConfigurationProperty("accessTokenUrl")]
        public string AccessTokenUrl
        {
            get { return (string)base["accessTokenUrl"]; }
            set { base["accessTokenUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the url to redirect to after authorization
        /// </summary>
        [ConfigurationProperty("redirectUrl")]
        public string RedirectUrl
        {
            get { return (string)base["redirectUrl"]; }
            set { base["redirectUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the scope of access needed by the application
        /// </summary>
        [ConfigurationProperty("scope")]
        public string Scope
        {
            get { return (string)base["scope"]; }
            set { base["scope"] = value; }
        }

        #endregion
    }
}
