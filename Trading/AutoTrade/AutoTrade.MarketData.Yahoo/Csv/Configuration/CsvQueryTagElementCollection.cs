using System.Configuration;

namespace AutoTrade.MarketData.Yahoo.Csv.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="CsvQueryTagElement"/> objects
    /// </summary>
    internal class CsvQueryTagElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="CsvQueryTagElement"/>
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CsvQueryTagElement();
        }

        /// <summary>
        /// Gets the name of the <see cref="CsvQueryTagElement"/> as the key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CsvQueryTagElement)element).Name;
        }
    }
}