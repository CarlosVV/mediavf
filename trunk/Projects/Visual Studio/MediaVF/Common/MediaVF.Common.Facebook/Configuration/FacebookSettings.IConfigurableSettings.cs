using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Common.Configuration.Facebook;
using System.Configuration;

namespace MediaVF.Common.Communication.Configuration.Facebook
{
    /// <summary>
    /// Represents the desktop-only implementation of IConfigurableSettings
    /// </summary>
    public partial class FacebookSettings : IConfigurableSettings<FacebookSection>
    {
        #region IConfigurableSettings Implementation

        /// <summary>
        /// Populates the object from a FacebookSection object
        /// </summary>
        /// <param name="section"></param>
        public void PopulateFromConfigurationSection(FacebookSection section)
        {
            ApplicationID = section.ApplicationID;
            ApplicationSecret = section.ApplicationSecret;
            AuthorizationUrl = section.AuthorizationUrl;
            AccessTokenUrl = section.AccessTokenUrl;
            RedirectUrl = section.RedirectUrl;
            Scope = section.Scope;
        }

        #endregion
    }
}
