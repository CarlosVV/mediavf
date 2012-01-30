using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Configuration;

using Microsoft.Practices.Unity;

using MediaVF.Services.Core;

namespace MediaVF.Services.Communication
{
    public class InvokableServiceShell : DependencyObject, IShell
    {
        IUnityContainer Container { get; set; }

        ServiceHost Host { get; set; }

        public InvokableServiceShell(IUnityContainer container)
        {
            Container = container;
        }

        public void Initialize()
        {
            int portNumber;
            if (!int.TryParse(ConfigurationManager.AppSettings["PortNumber"], out portNumber))
                portNumber = 3636;

            string serviceName = ConfigurationManager.AppSettings["ServiceName"];
            if (string.IsNullOrEmpty(serviceName))
                serviceName = "DefaultInvokableService";

            Uri baseAddress = new Uri(string.Format("http://localhost:{0}/", portNumber));

            Host = new ServiceHost(typeof(InvokableService), baseAddress);

            Host.AddServiceEndpoint(typeof(IInvokableService), new BasicHttpBinding(), baseAddress.ToString() + serviceName);

            Host.Open();
        }
    }
}
