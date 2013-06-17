using System;
using System.ComponentModel;

namespace AutoTrade.Core.UI
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// The event raised when the view model wants to close
        /// </summary>
        public event EventHandler Close;

        /// <summary>
        /// Raises the <see cref="Close"/> event
        /// </summary>
        protected void RaiseClose()
        {
            if (Close != null)
                Close(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event raised when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the event when a property changes with the given property's name
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Fields

        /// <summary>
        /// The message to display while the view model is busy
        /// </summary>
        private string _busyMessage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the busy message
        /// </summary>
        public string BusyMessage
        {
            get { return _busyMessage; }
            set
            {
                if (_busyMessage == value) return;
                _busyMessage = value;
                RaisePropertyChanged("BusyMessage");
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Gets or se
        /// </summary>
        public bool IsBusy
        {
            get { return !string.IsNullOrWhiteSpace(_busyMessage); }
        }

        #endregion
    }
}
