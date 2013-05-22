using System;
using AutoTrade.Core.Properties;

namespace AutoTrade.Core
{
    public class AppSettingNotFoundException : Exception
    {
        /// <summary>
        /// The setting name
        /// </summary>
        private readonly string _settingName;

        /// <summary>
        /// Instantiates a <see cref="AppSettingNotFoundException"/>
        /// </summary>
        /// <param name="settingName"></param>
        public AppSettingNotFoundException(string settingName)
        {
            _settingName = settingName;
        }

        /// <summary>
        /// Gets the message for the exception
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.AppSettingMissingMessageFormat, _settingName); }
        }
    }
}