using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace newton
{
    public class CommandHandler : ICommand
    {
        public delegate void ICommandOnExecute(object parameter);
        public delegate bool ICommandOnCanExecute(object parameter);

        private ICommandOnExecute _execute;
        private ICommandOnCanExecute _canExecute;

        public CommandHandler(ICommandOnExecute onExecuteMethod, ICommandOnCanExecute onCanExecuteMethod)
        {
            _execute = onExecuteMethod;
            _canExecute = onCanExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }

        #endregion
    }
}
