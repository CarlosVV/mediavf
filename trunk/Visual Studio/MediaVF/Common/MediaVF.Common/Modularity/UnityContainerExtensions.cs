using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Modularity;

using MediaVF.Common.Utilities;

namespace MediaVF.Common.Modularity
{
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Configures module load listeners by registering all associated types, resolving them to instances, and attaching their OnModuleLoaded
        /// methods to the ModuleManager's LoadModuleCompleted event
        /// </summary>
        public static void ConfigureModuleLoadListeners(this IUnityContainer container)
        {
            // if any module load listeners are registered
            IEnumerable<IModuleLoadListener> moduleLoadListeners = RegisterAndResolveAllDerivedTypes<IModuleLoadListener>(container);
            if (moduleLoadListeners.Any())
            {
                // hook up any module load listeneres to the module manager load completed event
                IModuleManager moduleManager = container.Resolve<IModuleManager>();
                moduleLoadListeners.ToList().ForEach(listener =>
                    moduleManager.LoadModuleCompleted += delegate(object sender, LoadModuleCompletedEventArgs e) { listener.OnModuleLoaded(e); });
            }
        }

        /// <summary>
        /// Registers all types derived from a base type, then resolves all types to a list of instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> RegisterAndResolveAllDerivedTypes<T>(this IUnityContainer container)
        {
            RegisterAllDerivedTypes<T>(container);

            return container.ResolveAll<T>();
        }

        /// <summary>
        /// Registers all types derived from a base type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterAllDerivedTypes<T>(this IUnityContainer container)
        {
            // if an object was already mapped to another type but is derived from the registering type, don't register a new instance
            // for it, but instead register the existing instance with the registering type
            List<ContainerRegistration> alreadyMappedTypes =
                container.Registrations.Where(registration => typeof(T).IsAssignableFrom(registration.MappedToType) &&
                                              registration.LifetimeManagerType == typeof(ContainerControlledLifetimeManager)).ToList();

            IEnumerable<Assembly> assemblyList = AssemblyUtility.GetAssemblies();

            // get all listener types
            List<List<Type>> derivedTypes =
                assemblyList.Select(assembly => assembly.GetTypes()
                                                        .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface).ToList()).ToList();

            // register all listener types
            derivedTypes.ForEach(derivedTypeInnerList =>
                derivedTypeInnerList.ForEach(derivedType =>
                {
                    ContainerRegistration existingRegistration = alreadyMappedTypes.FirstOrDefault(registration => registration.MappedToType == derivedType);
                    if (existingRegistration != null)
                    {
                        T resolved = (T)container.Resolve(existingRegistration.RegisteredType, existingRegistration.Name);
                        container.RegisterInstance<T>(string.IsNullOrEmpty(existingRegistration.Name) ? existingRegistration.RegisteredType.Name : existingRegistration.Name, resolved);
                    }
                    else
                        container.RegisterType(typeof(T), derivedType, derivedType.FullName, new ContainerControlledLifetimeManager());
                }));
        }
    }
}
