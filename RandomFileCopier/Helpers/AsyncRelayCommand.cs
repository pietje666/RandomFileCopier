using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RandomFileCopier.Helpers
{
    public sealed class AsyncRelayCommand
        : ICommand
    {
        private readonly Func<Task> _asyncAction;
        private readonly Func<bool> _canExecute;

        public AsyncRelayCommand(Func<Task> asyncAction)
            : this(asyncAction, null)
        {
        }

        public AsyncRelayCommand(Func<Task> asyncAction, Func<bool> canExecute)
        {
            _asyncAction = asyncAction;
            _canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        async void ICommand.Execute(object parameter)
        {
            if (_asyncAction != null && ((ICommand)this).CanExecute(null))
            {
                await _asyncAction();
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }

    public sealed class AsyncRelayCommand<TParam>
        : ICommand
    {
        private readonly Func<TParam, Task> _asyncAction;
        private readonly Func<TParam, bool> _canExecute;

        public AsyncRelayCommand(Func<TParam, Task> asyncAction)
            : this(asyncAction, null)
        {
        }

        public AsyncRelayCommand(Func<TParam, Task> asyncAction, Func<TParam, bool> canExecute)
        {
            _asyncAction = asyncAction;
            _canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            var isTypedParam = parameter is TParam;

            if (isTypedParam)
            {
                return _canExecute == null || _canExecute((TParam)parameter);
            }
            else
            {
                return _canExecute == null || _canExecute(default(TParam));
            }
        }

        async void ICommand.Execute(object parameter)
        {
            if (_asyncAction != null && ((ICommand)this).CanExecute(parameter))
            {
                var typedParam = (TParam)parameter;

                await _asyncAction(typedParam);
            }
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
