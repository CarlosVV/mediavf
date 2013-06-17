using System.Xml;
using AutoTrade.Core.Modularity.Configuration.Xml;

namespace AutoTrade.MarketData.Yahoo.Csv.Configuration
{
    /// <summary>
    /// Represents a CSV query tag
    /// </summary>
    class CsvQueryColumnConfiguration
    {
        #region Constants

        /// <summary>
        /// The name of the tag element
        /// </summary>
        private const string ColumnElementName = "column";

        /// <summary>
        /// The name of the tag attribute
        /// </summary>
        private const string TagAttributeName = "tag";

        /// <summary>
        /// The name of the description attribute
        /// </summary>
        private const string DescriptionAttributeName = "description";

        /// <summary>
        /// The name of the mapped property attribute
        /// </summary>
        private const string MappedPropertyAttributeName = "mappedProperty";

        /// <summary>
        /// The name of the enabled attribute
        /// </summary>
        private const string EnabledAttributeName = "enabled";

        #endregion

        #region Fields

        /// <summary>
        /// The tag name
        /// </summary>
        private readonly string _tag;

        /// <summary>
        /// The description for the tag
        /// </summary>
        private readonly string _description;

        /// <summary>
        /// The property to which the column maps
        /// </summary>
        private readonly string _mappedProperty;

        /// <summary>
        /// Flag indicating if the column is enabled
        /// </summary>
        private readonly bool _enabled;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="CsvQueryColumnConfiguration"/>
        /// </summary>
        /// <param name="xmlElement"></param>
        public CsvQueryColumnConfiguration(XmlElement xmlElement)
        {
            // validate element
            xmlElement.ShouldBeNamed(ColumnElementName);

            // get attribute values
            _tag = xmlElement.GetAttributeValue(TagAttributeName);
            _description = xmlElement.GetAttributeValue(DescriptionAttributeName);
            _mappedProperty = xmlElement.GetAttributeValue(MappedPropertyAttributeName, false);
            _enabled = bool.Parse(xmlElement.GetAttributeValue(EnabledAttributeName));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the tag
        /// </summary>
        public string Tag
        {
            get { return _tag; }
        }

        /// <summary>
        /// Gets the tag description
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the property that the tag is mapped to
        /// </summary>
        public string MappedProperty
        {
            get { return _mappedProperty; }
        }

        /// <summary>
        /// Gets or sets flag indicating if the tag is currently enabled
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
        }

        #endregion
    }
}