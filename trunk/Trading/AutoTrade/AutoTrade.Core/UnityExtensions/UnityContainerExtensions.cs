using System;
using System.Linq;
using Microsoft.Practices.Unity;

namespace AutoTrade.Core.UnityExtensions
{
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Registers a type only if it has not already been registered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMapTo"></typeparam>
        /// <param name="unityContainer"></param>
        /// <param name="lifetimeManager"></param>
        public static void RegisterTypeIfMissing<T, TMapTo>(this IUnityContainer unityContainer, LifetimeManager lifetimeManager)
            where TMapTo : T
        {
            if (unityContainer.Registrations.All(r => r.RegisteredType != typeof (T)))
                unityContainer.RegisterType<T, TMapTo>(lifetimeManager);
        }

        /// <summary>
        /// Registers a type only if it has not already been registered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMapTo"></typeparam>
        /// <param name="unityContainer"></param>
        /// <param name="name"></param>
        /// <param name="lifetimeManager"></param>
        public static void RegisterTypeIfMissing<T, TMapTo>(this IUnityContainer unityContainer, string name, LifetimeManager lifetimeManager)
            where TMapTo : T
        {
            if (unityContainer.Registrations.All(r => r.Name != name && r.RegisteredType != typeof(T)))
                unityContainer.RegisterType<T, TMapTo>(name, lifetimeManager);
        }

        /// <summary>
        /// Registers a type only if it has not already been registered
        /// </summary>
        /// <param name="unityContainer"></param>
        /// <param name="mapToType"></param>
        /// <param name="lifetimeManager"></param>
        /// <param name="registeredType"></param>
        public static void RegisterTypeIfMissing(this IUnityContainer unityContainer,
            Type registeredType,
            Type mapToType,
            LifetimeManager lifetimeManager)
        {
            if (unityContainer.Registrations.All(r => r.RegisteredType != registeredType))
                unityContainer.RegisterType(registeredType, mapToType, lifetimeManager);
        }

        /// <summary>
        /// Registers a type only if it has not already been registered
        /// </summary>
        /// <param name="unityContainer"></param>
        /// <param name="mapToType"></param>
        /// <param name="name"></param>
        /// <param name="lifetimeManager"></param>
        /// <param name="registeredType"></param>
        public static void RegisterTypeIfMissing(this IUnityContainer unityContainer,
            Type registeredType,
            Type mapToType,
            string name,
            LifetimeManager lifetimeManager)
        {
            if (unityContainer.Registrations.All(r => r.Name != name || r.RegisteredType != registeredType))
                unityContainer.RegisterType(registeredType, mapToType, name, lifetimeManager);
        }
    }
}
