using System;
using System.Windows.Input;

namespace AutoTrade.Core.UI
{
    public class DelegateCommand : ICommand
    {
        #region Events

        /// <summary>
        /// Event raised when the ability to execute the command changes
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Fields

        /// <summary>
        /// The delegate to invoke to execute the command
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// The delegate to determine if the command can be executed
        /// </summary>
        private readonly Predicate<object> _canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="DelegateCommand"/>
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action<object> execute)
            : this(execute, null) { }

        /// <summary>
        /// Instantiates a <see cref="DelegateCommand"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the command can be executed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (_execute != null && CanExecute(parameter))
                _execute(parameter);
        }

        #endregion
    }
}
