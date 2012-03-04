using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using MediaVF.Common.Communication.Utilities;

namespace MediaVF.Common.Communication.Invocation
{
    /// <summary>
    /// Represents a parameter passed to a call to an invocation service
    /// </summary>
    [DataContract]
    public class InvokeParameter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the assembly-qualified type name of the parameter object
        /// </summary>
        [DataMember]
        public string AssemblyQualifiedTypeName { get; set; }

        /// <summary>
        /// Gets or sets the full type name of the parameter object
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if this parameter is a collection of objects of the given type
        /// </summary>
        [DataMember]
        public bool IsEnumerable { get; set; }

        /// <summary>
        /// Gets or sets the serialized parameter object
        /// </summary>
        [DataMember]
        public string SerializedParameter { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a list of serialized parameters for invocation of a service operation
        /// </summary>
        /// <param name="serializer">The utility used to serialize the parameter objects</param>
        /// <param name="parameters">The list of parameter types and objects</param>
        /// <returns>A list of invoke parameters</returns>
        public static List<InvokeParameter> CreateInvokeParameters(List<Tuple<Type, object>> parameters)
        {
            return CreateInvokeParameters(parameters, null);
        }

        /// <summary>
        /// Creates a list of serialized parameters for invocation of a service operation
        /// </summary>
        /// <param name="serializer">The utility used to serialize the parameter objects</param>
        /// <param name="parameters">The list of parameter types and objects</param>
        /// <returns>A list of invoke parameters</returns>
        public static List<InvokeParameter> CreateInvokeParameters(List<Tuple<Type, object>> parameters, string encryptionKey)
        {
            if (parameters != null && parameters.Count > 0)
            {
                // create list of parameters
                List<InvokeParameter> serializedParameters = new List<InvokeParameter>();
                foreach (Tuple<Type, object> parameter in parameters)
                {
                    // get the object type
                    Type objectType = parameter.Item1;

                    // check if the parameter is an enumerable
                    bool isEnumerable = false;
                    if (typeof(IEnumerable).IsAssignableFrom(objectType) && objectType.IsGenericType)
                    {
                        // get the underlying object type
                        objectType = objectType.GetGenericArguments().First();
                        isEnumerable = true;
                    }

                    // create the parameter with the type information and the serialize object
                    serializedParameters.Add(
                        new InvokeParameter()
                        {
                            TypeName = objectType.FullName,
                            AssemblyQualifiedTypeName = objectType.AssemblyQualifiedName,
                            IsEnumerable = isEnumerable,
                            SerializedParameter = DataContractUtility.Serialize(parameter.Item1, parameter.Item2, encryptionKey)
                        });
                }

                return serializedParameters;
            }

            return null;
        }

        #endregion
    }
}
