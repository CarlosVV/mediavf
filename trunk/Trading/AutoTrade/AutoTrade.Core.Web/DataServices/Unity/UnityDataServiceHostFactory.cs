using System;
using System.Data.Services;
using System.ServiceModel;
using AutoTrade.Core.UnityExtensions;
using Microsoft.Practices.Unity;

namespace AutoTrade.Core.Web.DataServices.Unity
{
    public class UnityDataServiceHostFactory : DataServiceHostFactory
    {
        /// <summary>
        /// Creates a <see cref="UnityDataServiceHost"/>
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="baseAddresses"></param>
        /// <returns></returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var container = CreateContainer();

            return new UnityDataServiceHost(container, serviceType, baseAddresses);
        }

        /// <summary>
        /// Configures a container
        /// </summary>
        protected virtual IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            container.ConfigureFromConfigurationSection();

            return container;
        }
    }
}