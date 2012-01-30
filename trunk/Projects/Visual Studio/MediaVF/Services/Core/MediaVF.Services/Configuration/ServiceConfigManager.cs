using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Practices.ServiceLocation;

using MediaVF.Services.Logging;
using System.Reflection;

namespace MediaVF.Services.Configuration
{
    public class ServiceConfigManager : IServiceConfigManager
    {
        #region Constants

        private const string CONFIG_FOLDER = "config";
        private const string SEED_CONFIG = "Seed.config";
        private const string SERVICE_COMP_CFG_PLACEHOLDER = "<!-- serviceConfigurationPlaceHolder -->";
        private const string SERVICE_CONFIG = "Service.config";

        #endregion Constants

        #region Properties

        /// <summary>
        /// Log
        /// </summary>
        private IComboLog Log { get; set; }

        public int ApplicationID { get; private set; }

        /// <summary>
        /// Underlying configuration object read from config file
        /// </summary>
        public ServiceConfigSection Configuration { get; private set; }

        public string ComponentDBConnectionString
        {
            get
            {
                return Configuration != null && Configuration.SharedDataContext != null ? 
                    Configuration.SharedDataContext.DataContext.ConnectionString.ConnectionString :
                    null;
            }
        }

        Dictionary<Type, ServiceComponentElement> _components;
        public Dictionary<Type, ServiceComponentElement> Components
        {
            get
            {
                if (_components == null && Configuration != null)
                    _components = Configuration.Components.ToDictionary(c => c.ComponentType, c => c);

                return _components;
            }
        }

        #endregion Properties

        #region Constructors

        public ServiceConfigManager(IComboLog log)
        {
            Log = log;
        }

        #endregion Constructors

        #region Load

        /// <summary>
        /// Loads configuration by reading in base .svccfg file and any .svccmpcfg files found in Config folder
        /// </summary>
        public void Load()
        {
            try
            {
                // set the application id from app settings
                int applicationID;
                if (int.TryParse(ConfigurationManager.AppSettings["ApplicationID"], out applicationID))
                    ApplicationID = applicationID;

                // get base service config
                string serviceConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + SEED_CONFIG;
                string serviceConfig = File.ReadAllText(serviceConfigFilePath);

                string serviceComponentsConfig = GetServiceComponentsSection();

                serviceConfig = serviceConfig.Replace(SERVICE_COMP_CFG_PLACEHOLDER, serviceComponentsConfig);

                // load config into xml doc
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(serviceConfig.ToString());

                // get location for generated config file
                string configFilePath = AppDomain.CurrentDomain.BaseDirectory + SERVICE_CONFIG;

                // delete the config file if its already present
                if (File.Exists(configFilePath))
                    File.Delete(configFilePath);
                StreamWriter textWriter = new StreamWriter(configFilePath);
                XmlWriter xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings() { Indent = true, OmitXmlDeclaration = true });
                xmlDoc.Save(xmlWriter);
                xmlWriter.Close();
                textWriter.Close();

                // set underlying configuration
                ConfigurationManager.RefreshSection("serviceConfig");
                Configuration = ConfigurationManager.GetSection("serviceConfig") as ServiceConfigSection;
            }
            catch (Exception ex)
            {
                Log.Fatal("Error loading service configuration.", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the serviceComponents section by aggregating all config found in .svccmpcfg files
        /// </summary>
        /// <returns></returns>
        public string GetServiceComponentsSection()
        {
            // check for config folder
            string configFolderPath = AppDomain.CurrentDomain.BaseDirectory + CONFIG_FOLDER;
            if (!Directory.Exists(configFolderPath))
                throw new DirectoryNotFoundException("Config folder not found: " + configFolderPath);

            // get all service component config files
            List<string> serviceComponentConfigFiles = Directory.GetFiles(configFolderPath, "*.comp.config").ToList();

            // build service component xml
            StringBuilder aggregatedConfig = new StringBuilder();

            serviceComponentConfigFiles.ForEach(svccmpcfgFile => aggregatedConfig.AppendLine(File.ReadAllText(svccmpcfgFile)));

            return aggregatedConfig.ToString();
        }

        #endregion

        #region Get

        /// <summary>
        /// Gets a component setting from config
        /// </summary>
        /// <param name="componentType">The type of the component for which to get the setting</param>
        /// <param name="key">The key for the setting</param>
        /// <returns></returns>
        public string GetComponentSetting(Type componentType, string key)
        {
            if (Components != null && Components.ContainsKey(componentType) && Components[componentType].Settings[key] != null)
                return Components[componentType].Settings[key].Value;
            else
                return string.Empty;
        }

        #endregion
    }
}
