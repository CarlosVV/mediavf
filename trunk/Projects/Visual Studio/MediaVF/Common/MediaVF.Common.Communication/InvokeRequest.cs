using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace MediaVF.Common.Communication
{
    [DataContract]
    public class InvokeRequest
    {
        [DataMember]
        public string ComponentType { get; set; }

        [DataMember]
        public string OperationName { get; set; }

        [DataMember]
        public InvokeParameter[] OperationParameters { get; set; }

        public static InvokeRequest Create<T>(string operationName, params object[] parameters)
        {
            InvokeRequest request = new InvokeRequest() { ComponentType = typeof(T).FullName, OperationName = operationName };

            List<Tuple<Type, object>> parameterDictionary = new List<Tuple<Type, object>>();
            if (parameters != null && parameters.Length > 0)
            {
                MethodInfo method = typeof(T).GetMethod(operationName);
                if (method != null)
                {
                    List<ParameterInfo> parameterInfos = method.GetParameters().ToList();
                    foreach (ParameterInfo parameterInfo in parameterInfos)
                        parameterDictionary.Add(new Tuple<Type, object>(parameterInfo.ParameterType, parameters[parameterInfos.IndexOf(parameterInfo)]));
                }
            }

            SerializationUtility serializer = new SerializationUtility();
            request.OperationParameters = serializer.CreateInvokeParameters(parameterDictionary).ToArray();

            return request;
        }
    }
}
