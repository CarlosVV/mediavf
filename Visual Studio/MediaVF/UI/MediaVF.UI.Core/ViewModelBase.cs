using System;
using System.ComponentModel;

using Microsoft.Practices.Unity;

namespace MediaVF.UI.Core
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Events

        public event Action Close;

        protected void RaiseClose()
        {
            if (Close != null)
                Close();
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        /// <summary>
        /// Event raised when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the event when a property changes with the given property's name
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

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

        public ViewModelBase(IUnityContainer container)
        {
            Container = container;
        }
    }
}
