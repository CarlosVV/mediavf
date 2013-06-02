using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace AutoTrade.MarketData.CollectionService
{
    public static class UnityConfigurationSectionExtensions
    {
        /// <summary>
        /// Appends registrations to a container from a config section
        /// </summary>
        /// <param name="unityConfigurationSection"></param>
        /// <param name="container"></param>
        public static void AppendRegistrations(this UnityConfigurationSection unityConfigurationSection, IUnityContainer container)
        {
            // get the container from config
            var configContainer = unityConfigurationSection.Containers[0];
            
            // loop through registrations and add them to the container
            foreach (var registration in configContainer.Registrations)
                container.RegisterType(GetType(registration.MapToName),
                    GetType(registration.Name),
                    registration.Lifetime.CreateLifetimeManager());
        }

        /// <summary>
        /// Gets a type from its assembly-qualified 
        /// </summary>
        /// <param name="assemblyQualifiedTypeName"></param>
        /// <returns></returns>
        private static Type GetType(string assemblyQualifiedTypeName)
        {
            // split on comma
            var typeNameParts = assemblyQualifiedTypeName.Split(',');

            // get type and assembly name
            var fullTypeName = typeNameParts[0].Trim();
            var assemblyName = typeNameParts[1].Trim();

            // get assembly by name
            var assembly =
                AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName(true).Name == assemblyName);

            // get type from assembly
            if (assembly != null)
                return assembly.GetType(fullTypeName);

            return null;
        }
    }
}
