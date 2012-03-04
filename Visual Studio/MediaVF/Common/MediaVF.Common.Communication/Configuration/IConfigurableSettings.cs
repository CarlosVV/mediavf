using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MediaVF.Common.Communication.Configuration
{
    /// <summary>
    /// Represents an interface for configurable settings objects
    /// </summary>
    public interface IConfigurableSettings<T> where T : ConfigurationSection
    {
        /// <summary>
        /// Implemented to populate the settings object from a configuration section
        /// </summary>
        /// <typeparam name="T">The type of the section</typeparam>
        /// <param name="section">The name of the section from which to populate the settings object</param>
        void PopulateFromConfigurationSection(T section);
    }
}
