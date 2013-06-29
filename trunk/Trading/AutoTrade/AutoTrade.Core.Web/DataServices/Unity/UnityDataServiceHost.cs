using System;
using System.Data.Services;
using Microsoft.Practices.Unity;
using Unity.Wcf;

namespace AutoTrade.Core.Web.DataServices.Unity
{
    public class UnityDataServiceHost : DataServiceHost
    {
        private readonly IUnityContainer _container;

        public UnityDataServiceHost(IUnityContainer container, Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            _container = container;

            foreach (var contractDescription in ImplementedContracts.Values)
            {
                var contractBehavior =
                    new UnityContractBehavior(new UnityInstanceProvider(container, contractDescription.ContractType));

                contractDescription.Behaviors.Add(contractBehavior);
            }
        }

        protected IUnityContainer Container
        {
            get { return _container; }
        }
    }
}