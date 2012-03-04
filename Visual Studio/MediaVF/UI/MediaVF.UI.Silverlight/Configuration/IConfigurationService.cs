using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;

using MediaVF.Common.Communication.Configuration;

namespace MediaVF.UI.Core.Configuration
{
    /// <summary>
    /// Represents the interface used by a service that provides configuration from the server
    /// </summary>
    [ServiceContract]
    public interface IConfigurationService
    {
        /// <summary>
        /// Begins the service call to get configuration from the server
        /// </summary>
        /// <returns></returns>
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetConfiguration(AsyncCallback callback, object userState);

        /// <summary>
        /// Ends the service call to get configuration from the server
        /// </summary>
        /// <returns></returns>
        ServerConfiguration EndGetConfiguration(IAsyncResult result);
    }
}
