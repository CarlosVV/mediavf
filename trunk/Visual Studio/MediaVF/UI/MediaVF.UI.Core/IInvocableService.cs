using System;
using System.Net;
using System.ServiceModel;

using MediaVF.Common.Communication.Invocation;

namespace MediaVF.UI.Core
{
    [ServiceContract]
    public partial interface IInvocableService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginInvoke(InvokeRequest request, AsyncCallback callback, object userState);

        InvokeResponse EndInvoke(IAsyncResult result);
    }
}
