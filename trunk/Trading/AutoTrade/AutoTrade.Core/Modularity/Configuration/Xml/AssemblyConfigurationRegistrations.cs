using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AutoTrade.Core.Properties;
using AutoTrade.Core.UnityExtensions;
using Microsoft.Practices.Unity;

namespace AutoTrade.Core.Modularity.Configuration.Xml
{
    public class AssemblyConfigurationRegistrations
    {
        #region Constants

        /// <summary>
        /// The name of the registrations collection element
        /// </summary>
        public const string RegistrationsCollectionName = "registrations";

        /// <summary>
        /// The name of the child elements
        /// </summary>
        private const string RegistrationElementName = "registration";

        /// <summary>
        /// The name of the name attribute
        /// </summary>
        private const string RegistrationNameAttributeName = "name";

        /// <summary>
        /// The name of the type attribute
        /// </summary>
        private const string RegistrationTypeAttributeName = "type";

        /// <summary>
        /// The name of the mapTo attribute
        /// </summary>
        private const string RegistrationMapToAttributeName = "mapTo";

        /// <summary>
        /// The name of the lifetime attribute
        /// </summary>
        private const string RegistrationLifetimeAttributeName = "lifetime";

        /// <summary>
        /// The alias for a <see cref="ContainerControlledLifetimeManager" />
        /// </summary>
        private const string SingletonAlias = "singleton";

        /// <summary>
        /// The alias for a <see cref="TransientLifetimeManager"/>
        /// </summary>
        private const string InstanceAlias = "instance";

        /// <summary>
        /// The name of the injection constructor element
        /// </summary>
        private const string InjectionConstructorElementName = "injectionConstructor";

        /// <summary>
        /// The name of the injection constructor parameter element
        /// </summary>
        private const string ParameterElementName = "parameter";

        /// <summary>
        /// The name of the injection constructor resolve attribute
        /// </summary>
        private const string ResolveAttributeName = "resolve";

        /// <summary>
        /// The name of the injection constructor type attribute
        /// </summary>
        private const string TypeAttributeName = "type";

        /// <summary>
        /// The name of the injection constructor value attribute
        /// </summary>
        private const string ValueAttributeName = "value";

        #endregion

        #region Nested

        /// <summary>
        /// Represents a registration for a dependency container
        /// </summary>
        private class Registration
        {
            #region Fields

            /// <summary>
            /// The name of the registration
            /// </summary>
            private readonly string _name;

            /// <summary>
            /// The injection constructor
            /// </summary>
            private readonly InjectionConstructor _injectionConstructor;

            /// <summary>
            /// The type to register
            /// </summary>
            private readonly Type _type;

            /// <summary>
            /// The type to map to in the registration
            /// </summary>
            private readonly Type _mapTo;

            /// <summary>
            /// The lifetime of the registration
            /// </summary>
            private readonly LifetimeManager _lifetime;

            #endregion

            #region Constructors

            /// <summary>
            /// Instantiates a <see cref="Registration"/>
            /// </summary>
            /// <param name="type"></param>
            /// <param name="mapTo"></param>
            /// <param name="lifetime"></param>
            /// <param name="name"></param>
            /// <param name="injectionConstructor"></param>
            public Registration(Type type,
                                Type mapTo, 
                                LifetimeManager lifetime,
                                string name,
                                InjectionConstructor injectionConstructor = null)
            {
                _type = type;
                _mapTo = mapTo;
                _lifetime = lifetime;
                _name = name;
                _injectionConstructor = injectionConstructor;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the name of the registration
            /// </summary>
            public string Name
            {
                get { return _name; }
            }

            /// <summary>
            /// Gets the type to map
            /// </summary>
            public Type Type
            {
                get { return _type; }
            }

            /// <summary>
            /// Gets the type to map to
            /// </summary>
            public Type MapTo
            {
                get { return _mapTo; }
            }

            /// <summary>
            /// Gets the lifetime of the registration
            /// </summary>
            public LifetimeManager Lifetime
            {
                get { return _lifetime; }
            }

            public InjectionConstructor InjectionConstructor
            {
                get { return _injectionConstructor; }
            }

            #endregion
        }

        #endregion

        #region Fields
        
        /// <summary>
        /// The mapping of aliases to <see cref="LifetimeManager"/> types
        /// </summary>
        private static readonly Dictionary<string, Type> LifetimeManagerTypeAliases =
            new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
                {
                    { SingletonAlias, typeof(ContainerControlledLifetimeManager) },
                    { InstanceAlias, typeof(TransientLifetimeManager) }
                };

        /// <summary>
        /// The collection of registrations
        /// </summary>
        private readonly IEnumerable<Registration> _registrations;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="AssemblyConfigurationRegistrations"/>
        /// </summary>
        public AssemblyConfigurationRegistrations(XmlNode node)
        {
            // validate root element
            node.ShouldBeNamed(RegistrationsCollectionName);

            // get registrations from child nodes
            _registrations = node.ChildNodes.OfType<XmlElement>().Select(CreateRegistration).ToList();
        }

        #endregion

        #region Static

        /// <summary>
        /// Gets a registration from an xml element
        /// </summary>
        /// <param name="registrationElement"></param>
        /// <returns></returns>
        private static Registration CreateRegistration(XmlElement registrationElement)
        {
            registrationElement.ShouldBeNamed(RegistrationElementName);

            // initialize injection constructor to null
            InjectionConstructor injectionConstructor = null;

            // check for an injection constructor element
            var injectionConstructorElement =
                registrationElement.ChildNodes.OfType<XmlElement>()
                                   .FirstOrDefault(x => x.Name == InjectionConstructorElementName);
            if (injectionConstructorElement != null)
                injectionConstructor = GetInjectionConstructor(injectionConstructorElement);

            return new Registration(registrationElement.GetTypeFromAttribute(RegistrationTypeAttributeName),
                registrationElement.GetTypeFromAttribute(RegistrationMapToAttributeName),
                GetLifetimeManager(registrationElement, RegistrationLifetimeAttributeName),
                registrationElement.GetAttributeValue(RegistrationNameAttributeName, false),
                injectionConstructor);
        }

        /// <summary>
        /// Gets a lifetime manager from an attribute of an element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private static LifetimeManager GetLifetimeManager(XmlElement element, string attributeName)
        {
            return GetLifetimeManager(element.GetAttributeValue(attributeName));
        }

        /// <summary>
        /// Gets a lifetime manager from an alias
        /// </summary>
        /// <param name="lifetimeManagerAlias"></param>
        /// <returns></returns>
        private static LifetimeManager GetLifetimeManager(string lifetimeManagerAlias)
        {
            if (LifetimeManagerTypeAliases.ContainsKey(lifetimeManagerAlias))
                return Activator.CreateInstance(LifetimeManagerTypeAliases[lifetimeManagerAlias]) as LifetimeManager;

            throw new AssemblyConfigurationException(Resources.UnrecognizedLifetimeManagerAliasExceptionMessage,
                lifetimeManagerAlias);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds registrations to a Unity container
        /// </summary>
        /// <param name="container"></param>
        public void AddRegistrationsToContainer(IUnityContainer container)
        {
            _registrations.ForEach(r =>
                {
                    // create empty list of injection members
                    var injectionMembers = new List<InjectionMember>();

                    // add injection constructor, if provided
                    if (r.InjectionConstructor != null)
                        injectionMembers.Add(r.InjectionConstructor);

                    // register type
                    if (string.IsNullOrWhiteSpace(r.Name))
                        container.RegisterTypeIfMissing(r.Type, r.MapTo, r.Lifetime, injectionMembers.ToArray());
                    else
                        container.RegisterTypeIfMissing(r.Type, r.MapTo, r.Name, r.Lifetime, injectionMembers.ToArray());
                });
        }

        /// <summary>
        /// Gets an injection constructor
        /// </summary>
        /// <param name="constructorElement"></param>
        /// <returns></returns>
        private static InjectionConstructor GetInjectionConstructor(XmlNode constructorElement)
        {
            // ensure element is an injection constructor element
            constructorElement.ShouldBeNamed(InjectionConstructorElementName);

            return new InjectionConstructor(constructorElement.ChildNodes.OfType<XmlElement>().Select(GetConstructorParameter).ToArray());
        }

        /// <summary>
        /// Gets a parameter for a constructor from a parameter XmlElement
        /// </summary>
        /// <param name="parameterElement"></param>
        /// <returns></returns>
        private static object GetConstructorParameter(XmlElement parameterElement)
        {
            // ensure element is parameter element
            parameterElement.ShouldBeNamed(ParameterElementName);

            // get flag indicating whether or not to resolve the value of the parameter
            var resolve = parameterElement.GetAttributeValue(ResolveAttributeName).ConvertTo<bool>();

            // get the type of the parameter
            var type = parameterElement.GetTypeFromAttribute(TypeAttributeName);

            // get value to be passed in - if resolving, this is not required
            var value = parameterElement.GetAttributeValue(ValueAttributeName, !resolve);

            // get value for parameter
            return resolve
                ? new ResolvedParameter(type, string.IsNullOrWhiteSpace(value) ? value : null)
                : value.ConvertTo(type);
        }

        #endregion
    }
}
