using System.Configuration;
using AutoTrade.MarketData.DataService.Properties;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.Wcf;

namespace AutoTrade.MarketData.DataService
{
    public class DataServiceHostFactory : UnityServiceHostFactory
    {
        /// <summary>
        /// The name of the Unity config section
        /// </summary>
        private const string UnitySectionName = "unity";

        /// <summary>
        /// Configures a container
        /// </summary>
        /// <param name="container"></param>
        protected override void ConfigureContainer(IUnityContainer container)
        {
            // get unity config section
            var configSection = (UnityConfigurationSection)ConfigurationManager.GetSection(UnitySectionName);
            if (configSection == null)
                throw new ConfigurationErrorsException(Resources.UnitySectionNotFoundMessage);

            // configure container
            configSection.Configure(container);
        }
    }
}