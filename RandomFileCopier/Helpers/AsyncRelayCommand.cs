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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }

    public sealed class AsyncRelayCommand<TParameter>
        : ICommand
    {
        private readonly Func<TParameter, Task> _asyncAction;
        private readonly Func<TParameter, bool> _canExecute;

        public AsyncRelayCommand(Func<TParameter, Task> asyncAction)
            : this(asyncAction, null)
        {
        }

        public AsyncRelayCommand(Func<TParameter, Task> asyncAction, Func<TParameter, bool> canExecute)
        {
            _asyncAction = asyncAction;
            _canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            var isTypedParam = parameter is TParameter;

            if (isTypedParam)
            {
                return _canExecute == null || _canExecute((TParameter)parameter);
            }
            else
            {
                return _canExecute == null || _canExecute(default(TParameter));
            }
        }

        async void ICommand.Execute(object parameter)
        {
            if (_asyncAction != null && ((ICommand)this).CanExecute(parameter))
            {
                var typedParam = (TParameter)parameter;

                await _asyncAction(typedParam);
            }
        }

        public event EventHandler CanExecuteChanged;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
