using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using MediaVF.Common.Communication.Utilities;
using System.Collections;

namespace MediaVF.Common.Communication.Invocation
{
    /// <summary>
    /// Represents a request to an invocation service
    /// </summary>
    [DataContract]
    public class InvokeRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type of the component on which to invoke the operation
        /// </summary>
        [DataMember]
        public string ComponentType { get; set; }

        /// <summary>
        /// Gets or sets the name of the operation to invoke on the component
        /// </summary>
        [DataMember]
        public string OperationName { get; set; }

        /// <summary>
        /// Gets or sets a list of parameters to be passed to operation to invoke
        /// </summary>
        [DataMember]
        public InvokeParameter[] OperationParameters { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an InvokeRequest with the given operation name and list of parameters
        /// </summary>
        /// <typeparam name="T">The type of the component for which the request is to be created</typeparam>
        /// <param name="operationName">The name of the operation to invoke on the component</param>
        /// <param name="parameters">The parameters used when invoking the operation</param>
        /// <returns>The InvokeRequest for the given operation and parameters</returns>
        public static InvokeRequest Create<T>(string operationName, params object[] parameters)
        {
            return Create<T>(operationName, null, parameters);
        }

        /// <summary>
        /// Creates an InvokeRequest with the given operation name and list of parameters
        /// </summary>
        /// <typeparam name="T">The type of the component for which the request is to be created</typeparam>
        /// <param name="operationName">The name of the operation to invoke on the component</param>
        /// <param name="encryptionKey">The key to use to encrypt the parameters when serializing</param>
        /// <param name="parameters">The parameters used when invoking the operation</param>
        /// <returns>The InvokeRequest for the given operation and parameters</returns>
        public static InvokeRequest Create<T>(string operationName, string encryptionKey, params object[] parameters)
        {
            // create a new request for the type and operation
            InvokeRequest request = new InvokeRequest() { ComponentType = typeof(T).FullName, OperationName = operationName };

            // create a list of parameter types and serialized data
            List<Tuple<Type, object>> parameterDictionary = new List<Tuple<Type, object>>();
            if (parameters != null && parameters.Length > 0)
            {
                // get the method info for the operation
                MethodInfo method = typeof(T).GetMethod(operationName);
                if (method != null)
                {
                    // get the list of parameters for 
                    List<ParameterInfo> parameterInfos = method.GetParameters().ToList();
                    foreach (ParameterInfo parameterInfo in parameterInfos)
                        parameterDictionary.Add(new Tuple<Type, object>(parameterInfo.ParameterType, parameters[parameterInfos.IndexOf(parameterInfo)]));
                }
            }

            // serialize parameters and store on request
            request.OperationParameters = InvokeParameter.CreateInvokeParameters(parameterDictionary, encryptionKey).ToArray();

            return request;
        }

        #endregion
    }
}
