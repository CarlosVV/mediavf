using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Practices.Unity;

using Shared = MediaVF.Common.Communication.Invocation;

namespace MediaVF.UI.Core
{
    public class InvokableServiceClient<T>
    {
        IUnityContainer Container { get; set; }

        public InvokableServiceClient(IUnityContainer container)
        {
            Container = container;
        }

        public void Invoke(string operationName, Action<Shared.InvokeResponse> callback, params object[] args)
        {
            IInvocableService service = Container.Resolve<IInvocableService>();
            SynchronizationContext currentContext = SynchronizationContext.Current;

            service.BeginInvoke(Shared.InvokeRequest.Create<T>(operationName, args),
                (asyncResult) =>
                {
                    Shared.InvokeResponse response = service.EndInvoke(asyncResult);
                    if (callback != null && response.Error == null)
                        currentContext.Send((obj) => callback(response), null);
                    else if (response.Error != null)
                        throw response.Error;
                },
                null);
        }
    }
}
