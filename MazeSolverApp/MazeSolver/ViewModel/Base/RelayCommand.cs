using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MazeSolver.ViewModel
{
    /// <summary>
    /// Class for creating Commands
    /// </summary>
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _action;
        private Func<bool> _canExec = () => true;

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public RelayCommand(Action action, Func<bool> canExec)
        {
            _action = action;
            _canExec = canExec;
        }

        /// <summary>
        /// Runs Delegate to check if function can be run 
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return _canExec();
        }

        /// <summary>
        /// Run Command Action
        /// </summary>
        public void Execute(object parameter)
        {
            _action();
        }

        /// <summary>
        /// Publish CanExecuteChanged (to re-evaluate CanExecute)
        /// </summary>
        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}
