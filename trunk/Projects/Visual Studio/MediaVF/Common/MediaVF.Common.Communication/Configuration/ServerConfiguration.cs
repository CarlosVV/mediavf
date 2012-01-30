using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using MediaVF.Common.Communication.Utilities;

namespace MediaVF.Common.Communication.Configuration
{
    /// <summary>
    /// Represents configuration on the server-side
    /// </summary>
    [DataContract]
    public partial class ServerConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets or sets a list of general app settings
        /// </summary>
        [DataMember]
        public List<AppSetting> AppSettings { get; set; }

        /// <summary>
        /// Gets or sets a list of configuration section data
        /// </summary>
        [DataMember]
        public List<ConfigurableSettingsData> SectionSettings { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Writes a section object to xml and stores it with its type in a ConfigurationSectionData object
        /// </summary>
        /// <typeparam name="T">The type of the configuration section</typeparam>
        /// <param name="encryptionKey">The key to use to encrypt when serializing</param>
        /// <param name="obj">The object to serialize and store</param>
        public void WriteSection(Type sectionType, string encryptionKey, string sectionName, object obj)
        {
            // instantiate section list, if necessary
            if (SectionSettings == null)
                SectionSettings = new List<ConfigurableSettingsData>();

            // add the serialized section with its type
            SectionSettings.Add(new ConfigurableSettingsData()
            {
                Name = sectionName,
                TypeFullName = sectionType.AssemblyQualifiedName,
                SerializedData = DataContractUtility.Serialize(sectionType, obj, encryptionKey)
            });
        }

        /// <summary>
        /// Reads a section of the given type from the stored sections
        /// </summary>
        /// <typeparam name="T">The type of section to deserialize</typeparam>
        /// <param name="encryptionKey">The key to use to decrypt when deserializing</param>
        /// <returns>An configuration section object of type T</returns>
        public object ReadSection(Type sectionType, string encryptionKey)
        {
            // check that there are any sections
            if (SectionSettings != null)
            {
                // get the data for the given section type and deserialize the section
                ConfigurableSettingsData configurationSectionData = SectionSettings.FirstOrDefault(s => s.TypeFullName == sectionType.FullName);
                if (configurationSectionData != null)
                    return DataContractUtility.Deserialize(sectionType, configurationSectionData.SerializedData, encryptionKey);
            }
            
            return null;
        }

        #endregion
    }
}
