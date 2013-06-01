using System.Configuration;

namespace AutoTrade.MarketData.Yahoo.Csv.Configuration
{
    /// <summary>
    /// Represents a CSV query tag
    /// </summary>
    internal class CsvQueryTagElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the tag
        /// </summary>
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
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