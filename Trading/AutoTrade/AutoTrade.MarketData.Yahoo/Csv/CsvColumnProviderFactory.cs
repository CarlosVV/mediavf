using System.Linq;
using AutoTrade.Core.Modularity.Configuration;
using AutoTrade.MarketData.Yahoo.Csv.Configuration;

namespace AutoTrade.MarketData.Yahoo.Csv
{
    class CsvColumnProviderFactory : ICsvColumnProviderFactory
    {
        #region Fields

        /// <summary>
        /// The assembly configuration manager
        /// </summary>
        private readonly IAssemblyConfigurationManager _assemblyConfigurationManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="CsvColumnProviderFactory"/>
        /// </summary>
        /// <param name="assemblyConfigurationManager"></param>
        public CsvColumnProviderFactory(IAssemblyConfigurationManager assemblyConfigurationManager)
        {
            _assemblyConfigurationManager = assemblyConfigurationManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a CSV column provider
        /// </summary>
        /// <returns></returns>
        public ICsvColumnProvider GetColumnProvider()
        {
            // get the config for the assembly
            var assemblyConfig = _assemblyConfigurationManager.GetAssemblyConfiguration(GetType());

            // return the config section
            return assemblyConfig.CustomSections.FirstOrDefault(c =>
                c.GetType() == typeof(CsvQueryConfigurationSection)) as CsvQueryConfigurationSection;
        }

        #endregion
    }
}