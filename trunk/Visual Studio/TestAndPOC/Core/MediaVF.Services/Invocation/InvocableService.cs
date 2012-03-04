using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using MediaVF.Common.Communication.Invocation;
using MediaVF.Common.Communication.Utilities;
using MediaVF.Services.Components;

namespace MediaVF.Services.Invocation
{
    public class InvocableService : IInvocableService
    {
        IUnityContainer Container { get; set; }

        public InvocableService()
        {
            Container = ServiceLocator.Current.GetInstance<IUnityContainer>();
        }

        public InvokeResponse Invoke(InvokeRequest request)
        {
            InvokeResponse response = new InvokeResponse();

            if (!string.IsNullOrEmpty(request.ComponentType))
            {
                IServiceComponentManager componentManager = Container.Resolve<IServiceComponentManager>();
                IServiceComponent component = componentManager.ServiceComponents.FirstOrDefault(c => c != null && c.GetType().GetInterface(request.ComponentType) != null);

                if (component != null)
                {
                    MethodInfo operation =
                        component.GetType().GetMethod(request.OperationName,
                            request.OperationParameters.Select(op => DataContractUtility.GetType(op.AssemblyQualifiedTypeName, op.TypeName, op.IsEnumerable)).ToArray());
                    if (operation != null)
                    {
                        try
                        {
                            List<object> parameters = new List<object>();
                            foreach (InvokeParameter parameter in request.OperationParameters)
                                parameters.Add(DataContractUtility.Deserialize(DataContractUtility.GetType(parameter.AssemblyQualifiedTypeName, parameter.TypeName, parameter.IsEnumerable),
                                    parameter.SerializedParameter));
                                
                            object result;

                            try
                            {
                                result = operation.Invoke(component, parameters.ToArray());
                                if (result != null)
                                    response.OperationResult = DataContractUtility.Serialize(result.GetType(), result);
                            }
                            catch (Exception ex)
                            {
                                response.Error = new Exception("An exception occurred trying to invoke operation.", ex);
                            }

                        }
                        catch (Exception ex)
                        {
                            response.Error = new InvalidRequestException("An exception occurred trying to deserialize operation parameters.", ex);
                        }
                    }
                    else
                        response.Error = new InvalidRequestException("Operation not found.");
                }
                else
                    response.Error = new InvalidRequestException("Component not found.");
            }
            else
                response.Error = new InvalidRequestException("Component type not provided.");

            return response;
        }
    }
}
