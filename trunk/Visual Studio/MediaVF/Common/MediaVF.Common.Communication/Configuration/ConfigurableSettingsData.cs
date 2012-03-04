using System.Runtime.Serialization;

namespace MediaVF.Common.Communication.Configuration
{
    /// <summary>
    /// Represents a serialized configuration section
    /// </summary>
    [DataContract]
    public class ConfigurableSettingsData
    {
        /// <summary>
        /// Gets or sets the name of the settings
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full name of the type of the settings object
        /// </summary>
        [DataMember]
        public string TypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the assembly-qualified name of the type of the settings object
        /// </summary>
        [DataMember]
        public string TypeAssemblyQualifiedName { get; set; }

        /// <summary>
        /// Gets or sets the serialized data representing the settings object
        /// </summary>
        [DataMember]
        public string SerializedData { get; set; }
    }
}
