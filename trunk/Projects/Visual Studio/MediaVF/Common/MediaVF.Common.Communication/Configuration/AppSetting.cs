using System.Runtime.Serialization;

namespace MediaVF.Common.Communication.Configuration
{
    /// <summary>
    /// Represents an app setting in configuration
    /// </summary>
    [DataContract]
    public class AppSetting
    {
        /// <summary>
        /// Gets or sets the key for the setting
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the setting
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}
