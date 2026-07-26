using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Uniceps.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool>? _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute;

        }

        public bool CanExecute(object? parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute());
        }

        public async void Execute(object? parameter)
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await _executeAsync();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }
#pragma warning disable CS0067 // The event is never used
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067
        public void RaiseCanExecuteChanged()
        {
            
            CommandManager.InvalidateRequerySuggested();

        }
    }
    public class AsyncRelayCommand<T> : ICommand
    {
        private readonly Func<T?, Task> _executeAsync;
        private readonly Func<T?, bool>? _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<T?, Task> executeAsync, Func<T?, bool>? canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            // نتحقق أولاً إذا كان الكوماند قيد التنفيذ لمنع الضغط المتكرر
            if (_isExecuting) return false;

            if (_canExecute == null) return true;

            // تحويل الباراميتر بأمان إلى النوع المطلق T
            T? typedParameter = CastParameter(parameter);
            return _canExecute(typedParameter);
        }

        public async void Execute(object? parameter)
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                T? typedParameter = CastParameter(parameter);
                await _executeAsync(typedParameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        // دالة مساعدة لتحويل الـ object القادم من الواجهة إلى النوع المحدد T بأمان
        private static T? CastParameter(object? parameter)
        {
            if (parameter is T typedParam)
            {
                return typedParam;
            }

            // إذا كان الباراميتر عبارة عن تغيير نوع رقمي (مثلاً تمرير string من الواجهة لـ int)
            if (parameter != null && typeof(T) != typeof(object))
            {
                try
                {
                    return (T)Convert.ChangeType(parameter, typeof(T));
                }
                catch
                {
                    return default;
                }
            }

            return default;
        }
    }
}
