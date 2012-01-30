using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using MediaVF.Common.Communication;

namespace MediaVF.Services.Communication
{
    [ServiceContract]
    public interface IInvokableService
    {
        [OperationContract]
        InvokeResponse Invoke(InvokeRequest request);
    }
}
