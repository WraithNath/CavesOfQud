using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WraithNath.CavesOfQud.ViewModels
{
    /// <summary>
    /// Implementation of the ICommand Interface
    /// </summary>
    public class Command : ICommand
    {
        #region Members

        private Action<object> _executeAction = null;
        private Func<object, bool> _canExecuteFunction = null;

        #endregion Members

        #region Events

        /// <summary>
        /// Adds and Removes handlers to the CanExecute Changed event
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion Events

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executeAction">The Action to execute</param>
        public Command(Action<object> executeAction) : this(executeAction, null) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executeAction">The Action to execute</param>
        /// <param name="canExecuteFunction">The Predicate to determine whether the action can execute</param>
        public Command(Action<object> executeAction, Func<object,bool> canExecuteFunction)
        {
            if (executeAction == null) throw new ArgumentNullException(nameof(executeAction));

            _executeAction = executeAction;
            _canExecuteFunction = canExecuteFunction;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets whether the command can be executed
        /// </summary>
        /// <returns>true if can execute</returns>
        public bool CanExecute()
        {
            return this.CanExecute(null);
        }

        /// <summary>
        /// Gets whether the command can be executed
        /// </summary>
        /// <param name="parameter">The parameter to check</param>
        /// <returns>rue if can execute</returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecuteFunction == null) return true;

            return _canExecuteFunction.Invoke(parameter);
        }

        /// <summary>
        /// Executre the command
        /// </summary>
        /// <param name="parameter">The command parameter</param>
        public void Execute(object parameter)
        {
            _executeAction.Invoke(parameter);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute()
        {
            this.Execute(null);
        }
        
        #endregion Methods
    }
}
