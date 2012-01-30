using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Configuration;

using Microsoft.Practices.Unity;

using MediaVF.Common.Communication.Invocation;
using MediaVF.Services;

namespace MediaVF.Services.Invocation
{
    public class InvocableServiceShell : DependencyObject, IShell
    {
        IUnityContainer Container { get; set; }

        ServiceHost Host { get; set; }

        public InvocableServiceShell(IUnityContainer container)
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
                serviceName = "DefaultInvocableService";

            Uri baseAddress = new Uri(string.Format("http://localhost:{0}/", portNumber));

            Host = new ServiceHost(typeof(InvocableService), baseAddress);

            Host.AddServiceEndpoint(typeof(IInvocableService), new BasicHttpBinding(), baseAddress.ToString() + serviceName);

            Host.Open();
        }
    }
}
