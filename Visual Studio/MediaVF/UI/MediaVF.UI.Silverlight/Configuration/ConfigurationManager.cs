using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

using Shared = MediaVF.Common.Communication.Configuration;
using MediaVF.Common.Communication.Utilities;

namespace MediaVF.UI.Core.Configuration
{
    public class ConfigurationManager : ClientBase<IConfigurationService>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the encryption key for the configuration manager
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Gets a dictionary of keys and values representing general application settings
        /// </summary>
        Dictionary<string, string> _appSettings;
        public Dictionary<string, string> AppSettings
        {
            get
            {
                if (_appSettings == null)
                    _appSettings = new Dictionary<string, string>();
                return _appSettings;
            }
        }

        /// <summary>
        /// Gets a dictionary of k
        /// </summary>
        Dictionary<string, object> _configurationSections;
        Dictionary<string, object> ConfigurationSections
        {
            get
            {
                if (_configurationSections == null)
                    _configurationSections = new Dictionary<string, object>();
                return _configurationSections;
            }
        }

        #endregion

        #region Constructors

        public ConfigurationManager()
            : base() { }

        public ConfigurationManager(string endpointConfigurationName)
            : base(endpointConfigurationName) { }

        public ConfigurationManager(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress) { }

        public ConfigurationManager(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress) { }

        public ConfigurationManager(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress) { }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration section with the given name and of the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetSection<T>(string name)
        {
            // initialize to default value
            T section = default(T);
            
            // check for section with matching name and type
            if (ConfigurationSections.ContainsKey(name) && ConfigurationSections[name] is T)
                section = (T)ConfigurationSections[name];

            // return
            return section;
        }

        /// <summary>
        /// Gets a configuration section with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetSection(string name)
        {
            // initialize to null
            object section = null;

            // check for section with matching name
            if (ConfigurationSections.ContainsKey(name))
                section = ConfigurationSections[name];

            // return
            return section;
        }

        /// <summary>
        /// Gets configuration from the server and invokes a callback when completed
        /// </summary>
        /// <param name="callback"></param>
        public void LoadConfiguration(Action callback)
        {
            // invoke service call
            base.Channel.BeginGetConfiguration(
                (asyncResult) =>
                {
                    // get server configuration by ending async call
                    Shared.ServerConfiguration serverConfiguration = base.Channel.EndGetConfiguration(asyncResult);
                    if (serverConfiguration != null)
                    {
                        // load app settings
                        if (serverConfiguration.AppSettings != null)
                            serverConfiguration.AppSettings.ForEach(appSetting => AppSettings.Add(appSetting.Key, appSetting.Value));

                        // load configuration sections
                        if (serverConfiguration.SectionSettings != null)
                            serverConfiguration.SectionSettings.ForEach(sectionData =>
                                {
                                    // check that the type is recognized
                                    Type sectionType = DataContractUtility.GetType(sectionData.TypeAssemblyQualifiedName, sectionData.TypeFullName, false);
                                    if (sectionType != null)
                                    {
                                        // deserialize to object and store in configuration section
                                        object section = serverConfiguration.ReadSection(sectionType, EncryptionKey);
                                        if (section != null)
                                            ConfigurationSections.Add(sectionData.Name, section);
                                    }
                                });
                    }

                    callback();
                },
                null);
        }

        #endregion
    }
}
