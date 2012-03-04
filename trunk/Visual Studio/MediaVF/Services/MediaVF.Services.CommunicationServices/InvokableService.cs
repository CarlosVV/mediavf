using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Unity;

using MediaVF.Common.Communication;
using MediaVF.Common.Entities;
using MediaVF.Services.Core.Components;
using Microsoft.Practices.ServiceLocation;

namespace MediaVF.Services.Communication
{
    public class InvokableService : IInvokableService
    {
        IUnityContainer Container { get; set; }

        public InvokableService()
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
                    MethodInfo operation = component.GetType().GetMethod(request.OperationName, request.OperationParameters.Select(op => Type.GetType(op.ParameterType)).ToArray());
                    if (operation != null)
                    {
                        try
                        {
                            SerializationUtility serializer = new SerializationUtility();

                            List<object> parameters = new List<object>();
                            foreach (InvokeParameter parameter in request.OperationParameters)
                                parameters.Add(serializer.Deserialize(Type.GetType(parameter.ParameterType), parameter.SerializedParameter));
                                
                            object result;

                            try
                            {
                                result = operation.Invoke(component, parameters.ToArray());
                                if (result != null)
                                    response.OperationResult = serializer.Serialize(result.GetType(), result);
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
