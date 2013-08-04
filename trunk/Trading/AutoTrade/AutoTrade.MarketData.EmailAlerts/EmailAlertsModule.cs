using AutoTrade.Core.Modularity;
using AutoTrade.Core.Modularity.Configuration;
using AutoTrade.MarketData.Data;
using Microsoft.Practices.Unity;

namespace AutoTrade.MarketData.EmailAlerts
{
    class EmailAlertsModule : ConfigurableModule
    {
        #region Methods

        /// <summary>
        /// Instantiates a <see cref="EmailAlertsModule"/>
        /// </summary>
        /// <param name="container"></param>
        /// <param name="assemblyConfigurationManager"></param>
        public EmailAlertsModule(IUnityContainer container,
                                 IAssemblyConfigurationManager assemblyConfigurationManager)
            : base(container, assemblyConfigurationManager)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the module
        /// </summary>
        public override void Initialize()
        {
            // call base first
            base.Initialize();

            // resolve repository and feed factory
            var repository = Container.Resolve<IMarketDataRepository>();
            var emailFeedFactory = Container.Resolve<IEmailFeedFactory>();
            
            // add configurations
            emailFeedFactory.AddConfigurations(repository.EmailFeedConfigurationsQuery);
        }

        #endregion
    }
}
