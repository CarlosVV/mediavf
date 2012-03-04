using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

using MediaVF.Services.Logging;
using Microsoft.Practices.ServiceLocation;
using System.Threading;
using MediaVF.Services.Modularity;

namespace MediaVF.Services.Components
{
    public class ServiceComponentManager : IServiceComponentManager
    {
        #region Properties

        /// <summary>
        /// Log
        /// </summary>
        IComboLog Log { get; set; }

        /// <summary>
        /// Container
        /// </summary>
        IUnityContainer Container { get; set; }

        /// <summary>
        /// List of components
        /// </summary>
        List<IServiceComponent> _serviceComponents;
        public List<IServiceComponent> ServiceComponents
        {
            get
            {
                if (_serviceComponents == null)
                    _serviceComponents = new List<IServiceComponent>();
                return _serviceComponents;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Instantiate manager with log and container
        /// </summary>
        /// <param name="log"></param>
        /// <param name="container"></param>
        public ServiceComponentManager(IComboLog log, IUnityContainer container)
        {
            Log = log;
            Container = container;
        }

        #endregion Constructors

        #region IModuleLoadListener Implementation

        /// <summary>
        /// Check for IServiceComponent modules being loaded, and when one is found, give it a child container and add it to the list
        /// </summary>
        /// <param name="e"></param>
        public void OnModuleLoaded(LoadModuleCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    // get type for reflection only
                    Type moduleType = Type.GetType(e.ModuleInfo.ModuleType);

                    if (typeof(IServiceComponent).IsAssignableFrom(moduleType))
                    {
                        IServiceComponent serviceComponent = (IServiceComponent)Container.Resolve(moduleType);
                        if (e.ModuleInfo is ExtendedModuleInfo)
                            serviceComponent.ID = ((ExtendedModuleInfo)e.ModuleInfo).ModuleID;
                        Container.RegisterInstance<IServiceComponent>(e.ModuleInfo.ModuleName, serviceComponent, new ContainerControlledLifetimeManager());

                        // add to service component collection
                        ServiceComponents.Add(serviceComponent);

                        // create a child container for the component
                        serviceComponent.ComponentContainer = Container.CreateChildContainer();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error handling module load in service component manager.", ex);
                }
            }
        }

        #endregion

        #region IServiceComponentManager Implementation

        /// <summary>
        /// Runs all registered service components
        /// </summary>
        public void RunComponents()
        {
            // run each service component in the list, on its own thread
            Container.ResolveAll<IServiceComponent>().ToList().ForEach(serviceComponent => new Thread(() => serviceComponent.Run()).Start());
        }

        #endregion
    }
}
