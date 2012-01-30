using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

using MediaVF.Common.Communication.Configuration;
using MediaVF.Common.Communication.Configuration.Facebook;
using MediaVF.Common.Communication.Invocation;
using MediaVF.Common.Communication.Utilities;
using MediaVF.Common.Configuration.Facebook;
using MediaVF.Entities.ArtistTrack;
using MediaVF.Services.Invocation;

namespace MediaVF.Web.BandedTogether.Server
{
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BandedTogetherService : InvocableService, IInvocableService, IConfigurationService
    {
        #region Constants

        /// <summary>
        /// Encryption key used to encrypt and decrypt configuration being passed from the server
        /// NOTE: This must be the same on both the client and the server
        /// </summary>
        const string CONFIG_ENCRYPTION_KEY = "8bb91201-95b2-46c9-8634-8f6b6f994784";

        #endregion

        #region IConfigurationService Implementation

        /// <summary>
        /// Gets configuration data and returns it in a server configuration object
        /// </summary>
        /// <returns></returns>
        public ServerConfiguration GetConfiguration()
        {
            // instantiate the object
            ServerConfiguration serverConfiguration = new ServerConfiguration();

            // set app settings
            serverConfiguration.AppSettings = new List<AppSetting>();
            foreach (string name in ConfigurationManager.AppSettings)
                serverConfiguration.AppSettings.Add(new AppSetting() { Key = name, Value = ConfigurationManager.AppSettings[name] });

            // create list of section settings
            serverConfiguration.SectionSettings = new List<ConfigurableSettingsData>();

            // add facebook section
            serverConfiguration.AddSettingsFromConfig<FacebookSettings, FacebookSection>(CONFIG_ENCRYPTION_KEY, "facebookConfig");

            return serverConfiguration;
        }

        #endregion
    }
}
