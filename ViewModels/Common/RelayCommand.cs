using System;
using System.Windows.Input;

namespace QL_quanCafe.ViewModels.Common
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute; // Thêm dấu ?

        // Thêm dấu ?
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Thêm dấu ?
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute((T)parameter!);
        }

        // Thêm dấu ?
        public void Execute(object? parameter)
        {
            _execute((T)parameter!);
        }
    }
}
