using System;
using System.Windows.Input;

namespace NoteAppWPF
{
    // TODO: если подключил MvvmLight, то зачем этот класс?
    // TODO: xml
    public class RelayCommand : ICommand
    {
        // TODO: xml
        private Action<object> _execute;

        // TODO: xml
        private Func<object, bool> _canExecute;

        // TODO: xml
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // TODO: xml
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // TODO: xml
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // TODO: xml
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
