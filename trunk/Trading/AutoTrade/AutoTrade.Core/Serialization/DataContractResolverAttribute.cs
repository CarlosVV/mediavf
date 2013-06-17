using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace AutoTrade.Core.Serialization
{
    public class DataContractResolverAttribute : Attribute, IOperationBehavior
    {
        private readonly Type _dataContractResolverType;

        public DataContractResolverAttribute(Type dataContractResolverType)
        {
            _dataContractResolverType = dataContractResolverType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="parameters"></param>
        public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="proxy"></param>
        public void ApplyClientBehavior(OperationDescription description, System.ServiceModel.Dispatcher.ClientOperation proxy)
        {
            var behavior = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            behavior.DataContractResolver = Activator.CreateInstance(_dataContractResolverType) as DataContractResolver;
        }

        public void ApplyDispatchBehavior(OperationDescription description, System.ServiceModel.Dispatcher.DispatchOperation dispatch)
        {
            var behavior = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            behavior.DataContractResolver = Activator.CreateInstance(_dataContractResolverType) as DataContractResolver;
        }

        public void Validate(OperationDescription description)
        {
            // Do validation.
        }
    }
}
