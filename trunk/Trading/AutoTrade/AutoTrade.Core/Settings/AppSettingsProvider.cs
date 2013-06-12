using System.Configuration;

namespace AutoTrade.Core.Settings
{
    public class AppSettingsProvider : ISettingsProvider
    {
        #region Methods

        /// <summary>
        /// Gets the value for a setting from config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public T GetSetting<T>(string settingName)
        {
            // get value from config
            string textValue = ConfigurationManager.AppSettings[settingName];

            // if no value was found in config, throw an exception
            if (textValue == null)
                throw new AppSettingNotFoundException(settingName);

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
            // get value from config
            string textValue = ConfigurationManager.AppSettings[settingName];

            // if no value was found in config, throw an exception
            if (textValue == null)
                return defaultValue;

            // convert to the expected type
            return textValue.ConvertTo<T>();
        }

        #endregion
    }
}