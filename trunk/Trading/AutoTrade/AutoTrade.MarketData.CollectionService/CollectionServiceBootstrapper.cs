using System;
using AutoTrade.Core.Bootstrapping;
using Microsoft.Practices.Unity;

namespace AutoTrade.MarketData.CollectionService
{
    public class CollectionServiceBootstrapper : ConfigurationBootstrapper
    {
        /// <summary>
        /// Runs all data subscriptions
        /// </summary>
        /// <returns></returns>
        public override IDisposable Run()
        {
            // run base
            base.Run();

            // get subscription manager
            var subscriptionManager = Container.Resolve<ISubscriptionManager>();

            // start all subscriptions
            subscriptionManager.StartAllSubscriptions();

            // return the container
            return Container;
        }
    }
}
