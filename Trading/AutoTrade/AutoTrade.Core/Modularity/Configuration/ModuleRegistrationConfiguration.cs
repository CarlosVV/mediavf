using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AutoTrade.Core.Properties;
using Microsoft.Practices.Unity;

namespace AutoTrade.Core.Modularity.Configuration
{
    class ModuleRegistrationConfiguration
    {
        #region Constants

        /// <summary>
        /// The name of the root element
        /// </summary>
        private const string RootName = "registrations";

        /// <summary>
        /// The name of the child elements
        /// </summary>
        private const string RegistrationElementName = "registration";

        /// <summary>
        /// The name of the type attribute
        /// </summary>
        private const string TypeAttributeName = "type";

        /// <summary>
        /// The name of the mapTo attribute
        /// </summary>
        private const string MapToAttributeName = "mapTo";

        /// <summary>
        /// The name of the lifetime attribute
        /// </summary>
        private const string LifetimeAttributeName = "lifetime";

        #region Aliases

        /// <summary>
        /// The alias for a <see cref="ContainerControlledLifetimeManager" />
        /// </summary>
        private const string SingletonAlias = "singleton";

        /// <summary>
        /// The alias for a <see cref="TransientLifetimeManager"/>
        /// </summary>
        private const string InstanceAlias = "instance";

        #endregion

        #endregion

        #region Nested

        /// <summary>
        /// Represents a registration for a dependency container
        /// </summary>
        private class Registration
        {
            #region Fields

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
            public Registration(Type type, Type mapTo, LifetimeManager lifetime)
            {
                _type = type;
                _mapTo = mapTo;
                _lifetime = lifetime;
            }

            #endregion

            #region Properties

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
        /// Instantiates a <see cref="ModuleRegistrationConfiguration"/>
        /// </summary>
        private ModuleRegistrationConfiguration(IEnumerable<Registration> registrations)
        {
            _registrations = registrations;
        }

        #endregion

        #region Static

        /// <summary>
        /// Create registrations from xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static ModuleRegistrationConfiguration Create(string xml)
        {
            // create xml document from xml
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // validate root element
            ValidateNode(xmlDoc.FirstChild, RootName);

            // get registrations from child nodes
            return new ModuleRegistrationConfiguration(
                xmlDoc.FirstChild.ChildNodes.OfType<XmlElement>().Select(CreateRegistration));
        }

        /// <summary>
        /// Gets a registration from an xml element
        /// </summary>
        /// <param name="registrationElement"></param>
        /// <returns></returns>
        private static Registration CreateRegistration(XmlElement registrationElement)
        {
            ValidateNode(registrationElement, RegistrationElementName);

            return new Registration(GetTypeFromAttribute(registrationElement, TypeAttributeName),
                GetTypeFromAttribute(registrationElement, MapToAttributeName),
                GetLifetimeManager(registrationElement, LifetimeAttributeName));
        }

        /// <summary>
        /// Gets a lifetime manager from an attribute of an element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private static LifetimeManager GetLifetimeManager(XmlElement element, string attributeName)
        {
            return GetLifetimeManager(GetAttributeValue(element, attributeName));
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

            throw new ModuleRegistrationException(Resources.UnrecognizedLifetimeManagerAliasExceptionMessage,
                lifetimeManagerAlias);
        }

        /// <summary>
        /// Validates a node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="expectedName"></param>
        private static void ValidateNode(XmlNode node, string expectedName)
        {
            // verify root node
            if (StringComparer.OrdinalIgnoreCase.Compare(node.Name, expectedName) != 0)
                throw new ModuleRegistrationException(Resources.UnexpectedNodeExceptionMessage,
                    expectedName,
                    node.Name);
        }

        /// <summary>
        /// Gets a type from an attribute on an element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private static Type GetTypeFromAttribute(XmlElement element, string attributeName)
        {
            return GetType(GetAttributeValue(element, attributeName));
        }

        /// <summary>
        /// Gets the value of an attribute
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetAttributeValue(XmlElement element, string name)
        {
            // get the attribute
            var attribute = element.GetAttributeNode(name);
            if (attribute == null)
                throw new ModuleRegistrationException(Resources.MissingAttributeExceptionMessage, name, element.Name);

            // check that the attribute is populated
            if (string.IsNullOrWhiteSpace(attribute.Value))
                throw new ModuleRegistrationException(Resources.AttributeValueNullExceptionMessage, name, element.Name);

            return attribute.Value;
        }

        /// <summary>
        /// Gets a type from a type name string
        /// </summary>
        /// <param name="assemblyQualifiedTypeName"></param>
        /// <returns></returns>
        private static Type GetType(string assemblyQualifiedTypeName)
        {
            // split into 2 parts - full type name and assembly name
            var typeNameParts = assemblyQualifiedTypeName.Split(',');
            if (typeNameParts.Length != 2)
                throw new InvalidOperationException(string.Format(Resources.InvalidTypeNameFormatExceptionMessage,
                    assemblyQualifiedTypeName));

            // get the type and assembly names out of the split text
            var fullNamePart = typeNameParts[0];
            var assemblyNamePart = typeNameParts[1];

            // get the assembly
            var assembly =
                AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyNamePart);
            if (assembly == null)
                throw new TypeLoadException(string.Format(Resources.AssemblyNotFoundExceptionMessage, assemblyNamePart));

            // get the type
            var type = assembly.GetType(fullNamePart, false);
            if (type == null)
                throw new TypeLoadException(string.Format(Resources.TypeNotFoundExceptionMessage,
                    fullNamePart,
                    assemblyNamePart));

            return type;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds registrations to a Unity container
        /// </summary>
        /// <param name="container"></param>
        public void AddRegistrationsToContainer(IUnityContainer container)
        {
            _registrations.ForEach(r => container.RegisterType(r.Type, r.MapTo, r.Lifetime));
        }

        #endregion
    }
}
