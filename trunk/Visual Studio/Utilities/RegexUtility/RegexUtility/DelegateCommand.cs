using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace RegexUtility
{
    class DelegateCommand : ICommand
    {
        #region Events

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Delegates

        Action<object> ExecuteAction { get; set; }

        Predicate<object> CanExecutePredicate { get; set; }

        #endregion

        #region Constructors

        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, null) { }

        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
        {
            ExecuteAction = executeAction;
            CanExecutePredicate = canExecutePredicate;
        }

        #endregion

        #region Methods

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

        #endregion
    }
}
