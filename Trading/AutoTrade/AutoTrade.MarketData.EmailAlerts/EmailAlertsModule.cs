using AutoTrade.Core.Modularity;
using AutoTrade.Core.Modularity.Configuration;
using AutoTrade.MarketData.Data;
using Microsoft.Practices.Unity;

namespace AutoTrade.MarketData.EmailAlerts
{
    class EmailAlertsModule : ConfigurableModule
    {
        /// <summary>
        /// Instantiates a <see cref="EmailAlertsModule"/>
        /// </summary>
        /// <param name="unityContainer"></param>
        /// <param name="assemblyConfigurationManager"></param>
        public EmailAlertsModule(IUnityContainer unityContainer, IAssemblyConfigurationManager assemblyConfigurationManager)
            : base(unityContainer, assemblyConfigurationManager)
        {
            // resolve repository and feed factory
            var repository = unityContainer.Resolve<IMarketDataRepository>();
            var emailFeedFactory = unityContainer.Resolve<IEmailFeedFactory>();
            
            // add configurations
            emailFeedFactory.AddConfigurations(repository.EmailFeedConfigurationsQuery);
        }
    }
}
