using System.Configuration;

namespace AutoTrade.MarketData.Yahoo.Csv.Configuration
{
    /// <summary>
    /// Represents a CSV query tag
    /// </summary>
    internal class CsvQueryColumnElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the tag
        /// </summary>
        [ConfigurationProperty("tag")]
        public string Tag
        {
            get { return (string)base["tag"]; }
            set { base["tag"] = value; }
        }

        /// <summary>
        /// Gets or sets the tag description
        /// </summary>
        [ConfigurationProperty("description")]
        public string Description
        {
            get { return (string)base["description"]; }
            set { base["description"] = value; }
        }

        /// <summary>
        /// Gets the property that the tag is mapped to
        /// </summary>
        [ConfigurationProperty("mappedProperty")]
        public string MappedProperty
        {
            get { return (string)base["mappedProperty"]; }
            set { base["mappedProperty"] = value; }
        }

        /// <summary>
        /// Gets or sets flag indicating if the tag is currently enabled
        /// </summary>
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
            set { base["enabled"] = value; }
        }
    }
}