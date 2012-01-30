using System;
using System.ComponentModel;

using Microsoft.Practices.Unity;

namespace MediaVF.UI.Core
{
    public class ContainerViewModel : ClosableViewModel
    {
        #region Properties

        protected IUnityContainer Container { get; private set; }

        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;

                    RaisePropertyChanged("IsBusy");
                }
            }
        }

        string _busyMessage;
        public string BusyMessage
        {
            get { return _busyMessage; }
            set
            {
                if (_busyMessage != value)
                {
                    _busyMessage = value;

                    RaisePropertyChanged("BusyMessage");
                }
            }
        }

        #endregion

        public ContainerViewModel(IUnityContainer container)
        {
            Container = container;
        }
    }
}
