using System.Xml;
using AutoTrade.Core.Modularity.Configuration.Xml;

namespace AutoTrade.MarketData.Yahoo.Yql.Configuration
{
    class YqlPropertyMappingElement
    {
        #region Constants

        /// <summary>
        /// The name of the property mapping element
        /// </summary>
        private const string PropertyMappingElementName = "propertyMapping";

        /// <summary>
        /// The name of the xml element name element
        /// </summary>
        private const string XmlElementNameAttributeName = "xmlElementName";

        /// <summary>
        /// The name of the property name attribute
        /// </summary>
        private const string PropertyNameAttributeName = "propertyName";

        /// <summary>
        /// The name of the enabled attribute
        /// </summary>
        private const string EnabledAttributeName = "enabled";

        #endregion

        #region Fields

        /// <summary>
        /// The tag name
        /// </summary>
        private readonly string _xmlElementName;

        /// <summary>
        /// The property to which the column maps
        /// </summary>
        private readonly string _propertyName;

        private readonly bool _enabled;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YqlPropertyMappingElement"/>
        /// </summary>
        /// <param name="xmlElement"></param>
        public YqlPropertyMappingElement(XmlElement xmlElement)
        {
            // validate element
            xmlElement.ShouldBeNamed(PropertyMappingElementName);

            // get attribute values
            _xmlElementName = xmlElement.GetAttributeValue(XmlElementNameAttributeName);
            _propertyName = xmlElement.GetAttributeValue(PropertyNameAttributeName);
            _enabled = bool.Parse(xmlElement.GetAttributeValue(EnabledAttributeName));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the tag
        /// </summary>
        public string XmlElementName
        {
            get { return _xmlElementName; }
        }

        /// <summary>
        /// Gets the property that the tag is mapped to
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Gets the property indicating if the mapping is enabled
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
        }

        #endregion
    }
}
