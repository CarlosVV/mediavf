namespace AutoTrade.Core.Settings
{
    public interface ISettingsProvider
    {
        /// <summary>
        /// Gets the value for a setting from config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <returns></returns>
        T GetSetting<T>(string settingName);

        /// <summary>
        /// Gets the value for a setting from config, using a default value if the setting is not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetSetting<T>(string settingName, T defaultValue);
    }
}
