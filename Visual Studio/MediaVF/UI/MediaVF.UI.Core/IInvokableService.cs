using System;
using System.Net;
using System.ServiceModel;

using MediaVF.Common.Communication;

namespace MediaVF.UI.Core
{
    [ServiceContract]
    public interface IInvokableService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginInvoke(InvokeRequest request, AsyncCallback callback, object userState);

        InvokeResponse EndInvoke(IAsyncResult result);
    }
}
