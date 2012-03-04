using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

using MediaVF.Common.Utilities;

namespace MediaVF.Common.Communication.Utilities
{
    /// <summary>
    /// Represents a utility used to work with data contracts
    /// </summary>
    public static partial class DataContractUtility
    {
        #region Properties

        /// <summary>
        /// Gets a static dictionary of DataContract full type names and their corresponding types
        /// </summary>
        static Dictionary<string, Type> _dataContractTypes;
        static Dictionary<string, Type> DataContractTypes
        {
            get
            {
                if (_dataContractTypes == null)
                    _dataContractTypes = new Dictionary<string, Type>();
                return _dataContractTypes;
            }
        }

        #endregion

        #region Methods

        #region Type Management

        /// <summary>
        /// Loads up a static collection of all available data contract types in loaded assemblies
        /// </summary>
        public static void LoadDataContractTypes()
        {
            // get a collection of loaded MediaVF assemblies
            IEnumerable<Assembly> mediaVFAssemblies = AssemblyUtility.GetAssemblies("MediaVF");

            // get all data contract types in loaded assemblies
            IEnumerable<Type> dataContractTypes =
                mediaVFAssemblies.SelectMany(a => a.GetTypes().Where(t => t.GetCustomAttributes(typeof(DataContractAttribute), true).Any()));

            // create dictionary of type names to types
            foreach (Type dataContractType in dataContractTypes)
            {
                // add the type name and its corresponding type
                DataContractTypes.Add(dataContractType.FullName, dataContractType);

                // add the enumerable version of the type as well
                Type enumerableType = typeof(IEnumerable<>).MakeGenericType(dataContractType);
                DataContractTypes.Add(enumerableType.FullName, enumerableType);
            }
        }

        /// <summary>
        /// Gets a data contract type using either its assembly-qualified name or its full name
        /// </summary>
        /// <param name="assemblyQualifiedName">The assembly-qualified name of the type to get</param>
        /// <param name="fullName">The full name of the type to get</param>
        /// <param name="isEnumerable">Flag indicating if the type to return should be a generic enumerable type for the given type</param>
        /// <returns></returns>
        public static Type GetType(string assemblyQualifiedName, string fullName, bool isEnumerable)
        {
            // try to get the type using the assembly-qualified name
            Type type = Type.GetType(assemblyQualifiedName);

            // if the type was not found, use the full name
            if (type == null)
            {
                // load all data contract types, if necessary
                if (DataContractTypes.Count == 0)
                    LoadDataContractTypes();

                // check if the type is found in the loaded data contract types
                if (DataContractTypes.ContainsKey(fullName))
                    type = DataContractTypes[fullName];

                // if the enumerable version was requested, create the enumerable type
                if (isEnumerable)
                    type = typeof(IEnumerable<>).MakeGenericType(type);
            }

            // return the type
            return type;
        }

        #endregion

        #region Serialization

        #region Serialize

        /// <summary>
        /// Serializes a data contract by using GetType to return the object type, or returning an empty string if the object is null
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>If the object is not null, a serialized version of the object; otherwise, an empty string</returns>
        public static string Serialize(object obj)
        {
            return Serialize(obj, null);
        }

        /// <summary>
        /// Serializes a data contract by using GetType to return the object type, or returning an empty string if the object is null
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>If the object is not null, a serialized version of the object; otherwise, an empty string</returns>
        public static string Serialize(object obj, string encryptionKey)
        {
            // check if the object is null
            if (obj != null)
                return Serialize(obj.GetType(), obj, encryptionKey);
            else
                return string.Empty;
        }

        /// <summary>
        /// Serializes a data contract of the given type
        /// </summary>
        /// <typeparam name="T">The type of the data contract</typeparam>
        /// <param name="obj">The data contract to serialize</param>
        /// <returns>The serialized version of the data contract</returns>
        public static string Serialize<T>(T obj)
        {
            return Serialize(typeof(T), obj, null);
        }
        
        /// <summary>
        /// Serializes a data contract of the given type
        /// </summary>
        /// <param name="t">The type of the data contract</param>
        /// <param name="obj">The data contract to serialize</param>
        /// <returns>The serialized version of the data contract</returns>
        public static string Serialize(Type t, object obj)
        {
            return Serialize(t, obj, null);
        }
        
        /// <summary>
        /// Serializes a data contract of the given type
        /// </summary>
        /// <typeparam name="T">The type of the data contract</typeparam>
        /// <param name="obj">The data contract to serialize</param>
        /// <returns>The serialized version of the data contract</returns>
        public static string Serialize<T>(T obj, string encryptionKey)
        {
            return Serialize(typeof(T), obj, encryptionKey);
        }

        /// <summary>
        /// Serializes a data contract
        /// </summary>
        /// <param name="t">The type of the object to serialize</param>
        /// <param name="obj">The object to serialize</param>
        /// <returns>The serialized version of the object</returns>
        public static string Serialize(Type t, object obj, string encryptionKey)
        {
            string xml;

            // check that the object is a data contract
            if ((t.GetCustomAttributes(typeof(DataContractAttribute), true).Any()) ||
                (t.IsGenericType && typeof(IEnumerable).IsAssignableFrom(t) && t.GetGenericArguments().First().GetCustomAttributes(typeof(DataContractAttribute), true).Any()))
            {
                // create object to store xml
                StringBuilder objXml = new StringBuilder();

                // create xml writer
                using (XmlWriter xmlWriter = XmlWriter.Create(objXml))
                {
                    // create data contract serializer for the contract type
                    DataContractSerializer serializer = new DataContractSerializer(t);

                    // write object to xml
                    serializer.WriteObject(xmlWriter, obj);

                    // flush the writer to ensure completion of xml writing
                    xmlWriter.Flush();

                    // set xml
                    xml = objXml.ToString();
                }
            }
            else
                xml = obj.ToString();

            // if an encryption key was provided, encrypt the serialized result before returning
            if (!string.IsNullOrEmpty(encryptionKey))
                xml = EncryptionUtility.Encrypt(encryptionKey, xml);

            return xml;
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserializes xmlData to a data contract object
        /// </summary>
        /// <typeparam name="T">The type of the data contract to which to deserialize</typeparam>
        /// <param name="xmlData">The xml data to deserialize</param>
        /// <returns>The deserialized data contract object</returns>
        public static T Deserialize<T>(string xmlData)
        {
            return (T)Deserialize(typeof(T), xmlData, null);
        }
        
        /// <summary>
        /// Deserializes xmlData to a data contract object
        /// </summary>
        /// <typeparam name="T">The type of the data contract to which to deserialize</typeparam>
        /// <param name="xmlData">The xml data to deserialize</param>
        /// <param name="encryptionKey">The key to use to decrypt the data before deserializing</param>
        /// <returns>The deserialized data contract object</returns>
        public static T Deserialize<T>(string xmlData, string encryptionKey)
        {
            return (T)Deserialize(typeof(T), xmlData, encryptionKey);
        }
        
        /// <summary>
        /// Deserializes xmlData to a data contract object
        /// </summary>
        /// <param name="t">The type of the data contract to which to deserialize</param>
        /// <param name="xmlData">The xml data to deserialize</param>
        /// <returns>The deserialized data contract object</returns>
        public static object Deserialize(Type t, string xmlData)
        {
            return Deserialize(t, xmlData, null);
        }
        
        /// <summary>
        /// Deserializes xmlData to a data contract object
        /// </summary>
        /// <param name="t">The type of the data contract to which to deserialize</param>
        /// <param name="xmlData">The xml data to deserialize</param>
        /// <param name="encryptionKey">The key to use to decrypt the data before deserializing</param>
        /// <returns>The deserialized data contract object</returns>
        public static object Deserialize(Type t, string xmlData, string encryptionKey)
        {
            // if an encryption key was provided, decrypt the data first
            if (!string.IsNullOrEmpty(encryptionKey))
                xmlData = EncryptionUtility.Decrypt(encryptionKey, xmlData);

            object obj;

            // check that the type is a data contract type
            if ((t.GetCustomAttributes(typeof(DataContractAttribute), true).Any()) ||
                (t.IsGenericType && typeof(IEnumerable).IsAssignableFrom(t) && t.GetGenericArguments().First().GetCustomAttributes(typeof(DataContractAttribute), true).Any()))
            {
                // read the xml data into a reader
                using (StringReader dataReader = new StringReader(xmlData))
                using (XmlReader xmlReader = XmlReader.Create(dataReader))
                {
                    // create serializer
                    DataContractSerializer serializer = new DataContractSerializer(t);

                    // deserialize to an object
                    obj = serializer.ReadObject(xmlReader);
                }
            }
            else
                obj = Convert.ChangeType(xmlData, t, CultureInfo.CurrentCulture);

            return obj;
        }

        #endregion

        #endregion

        #endregion
    }
}
