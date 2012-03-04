using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Practices.Unity;

using MediaVF.Common.Communication;

namespace MediaVF.UI.Core
{
    public class InvokableServiceClient<T>
    {
        IUnityContainer Container { get; set; }

        public InvokableServiceClient(IUnityContainer container)
        {
            Container = container;
        }

        public void Invoke(string operationName, Action<InvokeResponse> callback, params object[] args)
        {
            IInvokableService service = Container.Resolve<IInvokableService>();
            SynchronizationContext currentContext = SynchronizationContext.Current;

            service.BeginInvoke(InvokeRequest.Create<T>(operationName, args),
                (asyncResult) =>
                {
                    InvokeResponse response = service.EndInvoke(asyncResult);
                    if (callback != null && response.Error == null)
                        currentContext.Send((obj) => callback(response), null);
                    else if (response.Error != null)
                        throw response.Error;
                },
                null);
        }
    }
}
