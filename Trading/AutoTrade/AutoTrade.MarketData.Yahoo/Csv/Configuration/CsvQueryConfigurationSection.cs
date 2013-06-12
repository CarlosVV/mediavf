using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AutoTrade.Core.Modularity.Configuration;
using AutoTrade.Core.Modularity.Configuration.Xml;

namespace AutoTrade.MarketData.Yahoo.Csv.Configuration
{
    /// <summary>
    /// Represents a section in config for setting up Yahoo CSV queries
    /// </summary>
    internal class CsvQueryConfigurationSection : IAssemblyConfigurationSection, ICsvColumnProvider
    {
        #region Constants

        /// <summary>
        /// The name of the section
        /// </summary>
        public const string SectionName = "yahooCsvQueries";

        /// <summary>
        /// The name of the tags element
        /// </summary>
        private const string ColumnsElementName = "columns";

        #endregion

        #region Fields

        /// <summary>
        /// The column configurations
        /// </summary>
        private IEnumerable<CsvQueryColumnConfiguration> _columnConfigurations; 

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the collection of tags
        /// </summary>
        public IEnumerable<CsvQueryColumnConfiguration> ColumnConfigurations
        {
            get { return _columnConfigurations; }
        }

        /// <summary>
        /// Gets the collection of columns currently enabled for the csv
        /// </summary>
        private List<CsvQueryColumnConfiguration> EnabledColumns
        {
            get { return _columnConfigurations.Where(t => t.Enabled).ToList(); }
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
        /// Loads section from xml
        /// </summary>
        /// <param name="xmlElement"></param>
        public void Load(XmlElement xmlElement)
        {
            // validate name
            xmlElement.ShouldBeNamed(SectionName);

            // validate name of columns collection
            xmlElement.FirstChild.ShouldBeNamed(ColumnsElementName);

            // create column configurations
            _columnConfigurations =
                xmlElement.FirstChild.ChildNodes.OfType<XmlElement>().Select(e => new CsvQueryColumnConfiguration(e));
        }

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
