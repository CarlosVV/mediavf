using System.Linq;
using AutoTrade.Core.Modularity.Configuration;
using AutoTrade.MarketData.Yahoo.Yql.Configuration;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    class YqlPropertyMapperFactory : IYqlPropertyMapperFactory
    {
        /// <summary>
        /// The assembly configuration manager
        /// </summary>
        private readonly IAssemblyConfigurationManager _assemblyConfigurationManager;

        /// <summary>
        /// Instantiates a <see cref="YqlPropertyMapperFactory"/>
        /// </summary>
        /// <param name="assemblyConfigurationManager"></param>
        public YqlPropertyMapperFactory(IAssemblyConfigurationManager assemblyConfigurationManager)
        {
            _assemblyConfigurationManager = assemblyConfigurationManager;
        }

        /// <summary>
        /// Gets the property mapper
        /// </summary>
        /// <returns></returns>
        public IYqlPropertyMapper GetPropertyMapper()
        {
            // get the config for the assembly
            var assemblyConfig = _assemblyConfigurationManager.GetAssemblyConfiguration(GetType());

            // return the config section
            return assemblyConfig.CustomSections.FirstOrDefault(c =>
                c.GetType() == typeof(YqlQueryConfigurationSection)) as YqlQueryConfigurationSection;
        }
    }
}