using System.Configuration;

using MediaVF.Common.Communication.Utilities;

namespace MediaVF.Common.Communication.Configuration
{
    public partial class ServerConfiguration
    {
        /// <summary>
        /// Adds a settings object from a section in configuration
        /// </summary>
        /// <typeparam name="T">The type of settings object to create</typeparam>
        /// <param name="encryptionKey">The encryption key used for encrypting and serializing the settings object</param>
        /// <param name="sectionName">The name of the section in config</param>
        public void AddSettingsFromConfig<TSettings, TSection>(string encryptionKey, string sectionName)
            where TSettings : IConfigurableSettings<TSection>, new()
            where TSection : ConfigurationSection
        {
            // get the section from config
            TSection section = (TSection)ConfigurationManager.GetSection(sectionName);

            // if the section exists...
            if (section != null)
            {
                // create a new settings object
                TSettings settings = new TSettings();

                // populate the object from the section
                settings.PopulateFromConfigurationSection(section);

                // create new settings data object, with the new settings object serialized
                SectionSettings.Add(new ConfigurableSettingsData()
                {
                    Name = sectionName,
                    TypeAssemblyQualifiedName = typeof(TSettings).AssemblyQualifiedName,
                    TypeFullName = typeof(TSettings).FullName,
                    SerializedData = DataContractUtility.Serialize(typeof(TSettings), settings, encryptionKey)
                });
            }
        }
    }
}
