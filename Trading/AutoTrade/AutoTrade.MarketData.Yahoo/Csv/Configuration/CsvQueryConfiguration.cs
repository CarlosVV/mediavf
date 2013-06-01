using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AutoTrade.MarketData.Yahoo.Csv.Configuration
{
    /// <summary>
    /// Represents a section in config for setting up Yahoo CSV queries
    /// </summary>
    internal class CsvQueryConfiguration : ConfigurationSection, ICsvTagProvider
    {
        /// <summary>
        /// Gets or sets the collection of tags
        /// </summary>
        [ConfigurationProperty("tags")]
        [ConfigurationCollection(typeof(CsvQueryTagElementCollection))]
        public CsvQueryTagElementCollection Tags
        {
            get { return (CsvQueryTagElementCollection)base["tags"]; }
            set { base["tags"] = value; }
        }

        /// <summary>
        /// Gets the collection of tags to include for the query
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetTags()
        {
            return Tags.Cast<CsvQueryTagElement>().Where(t => t.Enabled).Select(t => t.Name);
        }
    }
}
