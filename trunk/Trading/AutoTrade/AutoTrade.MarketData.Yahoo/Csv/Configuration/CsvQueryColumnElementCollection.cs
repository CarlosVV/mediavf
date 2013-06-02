using System.Configuration;

namespace AutoTrade.MarketData.Yahoo.Csv.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="CsvQueryColumnElement"/> objects
    /// </summary>
    internal class CsvQueryColumnElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="CsvQueryColumnElement"/>
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CsvQueryColumnElement();
        }

        /// <summary>
        /// Gets the name of the <see cref="CsvQueryColumnElement"/> as the key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CsvQueryColumnElement)element).Tag;
        }
    }
}