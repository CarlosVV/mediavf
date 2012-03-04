using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaVF.Common.Communication.Configuration.Facebook
{
    /// <summary>
    /// Represents serializable Facebook settings pulled from configuration
    /// </summary>
    [DataContract]
    public partial class FacebookSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ID of the application in Facebook
        /// </summary>
        [DataMember]
        public string ApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the application secret code in Facebook
        /// </summary>
        [DataMember]
        public string ApplicationSecret { get; set; }

        /// <summary>
        /// Gets or sets the url for getting a user's authorization for the application
        /// </summary>
        [DataMember]
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// Gets or sets the url for getting an access token for a user
        /// </summary>
        [DataMember]
        public string AccessTokenUrl { get; set; }

        /// <summary>
        /// Gets or sets the url to redirect to after authorization
        /// </summary>
        [DataMember]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the scope of access needed by the application
        /// </summary>
        [DataMember]
        public string Scope { get; set; }

        #endregion
    }
}
