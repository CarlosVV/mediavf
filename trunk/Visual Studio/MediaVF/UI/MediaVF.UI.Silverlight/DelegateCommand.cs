using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MediaVF.UI.Core
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        protected void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public Action<object> ExecuteAction { get; private set; }

        public Predicate<object> CanExecutePredicate { get; private set; }

        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, (Predicate<object>)null) { }

        public DelegateCommand(Action<object> executeAction, INotifyPropertyChanged attachedObj)
            : this(executeAction, null, attachedObj) { }

        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
            : this(executeAction, canExecutePredicate, null) { }

        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecutePredicate, INotifyPropertyChanged attachedObj)
        {
            ExecuteAction = executeAction;
            CanExecutePredicate = canExecutePredicate;

            if (attachedObj != null)
                attachedObj.PropertyChanged += (sender, e) => RaiseCanExecuteChanged();
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecutePredicate != null)
                return CanExecutePredicate(parameter);
            else
                return true;
        }

        public void Execute(object parameter)
        {
            if (ExecuteAction != null)
                ExecuteAction(parameter);
        }
    }
}
