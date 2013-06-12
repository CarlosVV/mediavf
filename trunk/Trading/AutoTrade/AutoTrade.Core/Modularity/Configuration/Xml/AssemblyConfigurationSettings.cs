using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AutoTrade.Core.Settings;

namespace AutoTrade.Core.Modularity.Configuration.Xml
{
    public class AssemblyConfigurationSettings : ISettingsProvider
    {
        #region Settings

        /// <summary>
        /// The name of the settings collection element
        /// </summary>
        public const string SettingsCollectionName = "settings";

        /// <summary>
        /// The name of the setting element
        /// </summary>
        private const string SettingElementName = "setting";

        /// <summary>
        /// The name of the name attribute on a setting element
        /// </summary>
        private const string SettingNameAttributeName = "name";

        /// <summary>
        /// The name of the value attribute on a setting element
        /// </summary>
        private const string SettingValueAttributeName = "value";

        #endregion

        #region Fields

        /// <summary>
        /// The dictionary containing the settings
        /// </summary>
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="AssemblyConfigurationSettings"/>
        /// </summary>
        /// <param name="node"></param>
        public AssemblyConfigurationSettings(XmlNode node)
        {
            // validate root element
            node.ShouldBeNamed(SettingsCollectionName);

            // add settings for each element
            node.ChildNodes.OfType<XmlElement>().ForEach(AddSetting);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a setting from an xml element
        /// </summary>
        /// <param name="settingElement"></param>
        private void AddSetting(XmlElement settingElement)
        {
            // verify element name
            settingElement.ShouldBeNamed(SettingElementName);

            // add setting from element
            _settings.Add(settingElement.GetAttributeValue(SettingNameAttributeName),
                          settingElement.GetAttributeValue(SettingValueAttributeName));
        }

        /// <summary>
        /// Gets the value for a setting from config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public T GetSetting<T>(string settingName)
        {
            // check if the setting is present
            if (!_settings.ContainsKey(settingName)) throw new AppSettingNotFoundException(settingName);

            // get value from config
            string textValue = _settings[settingName];

            // if no value was found in config, throw an exception
            if (textValue == null) throw new AppSettingNotFoundException(settingName);

            // convert to the expected type
            return textValue.ConvertTo<T>();
        }

        /// <summary>
        /// Gets the value for a setting from config, using a default value if the setting is not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetSetting<T>(string settingName, T defaultValue)
        {
            // check if the setting is present
            if (!_settings.ContainsKey(settingName)) return defaultValue;

            // get value from config
            string textValue = _settings[settingName];

            // if no value was found in config, throw an exception
            if (textValue == null) return defaultValue;

            // convert to the expected type
            return textValue.ConvertTo<T>();
        }

        #endregion
    }
}
