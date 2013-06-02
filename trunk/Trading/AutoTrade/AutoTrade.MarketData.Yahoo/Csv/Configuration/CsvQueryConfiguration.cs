using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AutoTrade.MarketData.Yahoo.Csv.Configuration
{
    /// <summary>
    /// Represents a section in config for setting up Yahoo CSV queries
    /// </summary>
    internal class CsvQueryConfiguration : ConfigurationSection, ICsvColumnProvider
    {
        #region Properties

        /// <summary>
        /// Gets or sets the collection of tags
        /// </summary>
        [ConfigurationProperty("tags")]
        [ConfigurationCollection(typeof(CsvQueryColumnElementCollection))]
        public CsvQueryColumnElementCollection Tags
        {
            get { return (CsvQueryColumnElementCollection)base["tags"]; }
            set { base["tags"] = value; }
        }

        /// <summary>
        /// Gets the collection of columns currently enabled for the csv
        /// </summary>
        private List<CsvQueryColumnElement> EnabledColumns
        {
            get { return Tags.Cast<CsvQueryColumnElement>().Where(t => t.Enabled).ToList(); }
        }

        /// <summary>
        /// Gets the number of columns currently enabled
        /// </summary>
        public int EnabledColumnCount
        {
            get { return EnabledColumns.Count; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the collection of tags to include for the query
        /// </summary>
        /// <returns></returns>
        public string GetTagsString()
        {
            return string.Join(string.Empty, EnabledColumns.Select(t => t.Tag));
        }

        /// <summary>
        /// Gets the list of properties to map to columns in the csv (in the order they appear in the csv)
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetProperties()
        {
            return EnabledColumns.Where(c => !string.IsNullOrWhiteSpace(c.MappedProperty))
                                 .ToDictionary(c => EnabledColumns.IndexOf(c), c => c.MappedProperty);
        }

        #endregion
    }
}
