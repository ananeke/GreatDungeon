using BaseClassesLibrary.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BasicClassLibrary.Commands
{
    public interface IValueTypeCommandBase
    {
        #region Variables and Properties
        /// <summary>
        /// Occurs when the T type is value Type.
        /// </summary>
        bool IsValueType { get; }
        #endregion
    }

    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// </summary>
    public abstract class DelegateCommandBase : ICommand
    {
        #region Variables and Properties
        protected bool isExecuting;
        /// <summary>
        /// Occurs when the command is executing.
        /// </summary>
        public bool IsExecuting
        {
            get
            {
                return isExecuting;
            }
            protected set
            {
                isExecuting = value;
                OnExecuteChanged?.Invoke(isExecuting);
            }
        }

        /// <summary>
        /// Fired when the command is executing or finish execution
        /// </summary>
        public event Action<bool> OnExecuteChanged;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        public DelegateCommandBase(Action<bool> onExecuteChanged = null)
        {
            isExecuting = false;
            if (onExecuteChanged != null)
                OnExecuteChanged += onExecuteChanged;
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public virtual bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public abstract void Execute(object parameter);
    }

    public abstract class SyncDelegateCommandBase : DelegateCommandBase
    {
        public SyncDelegateCommandBase(Action<bool> onExecuteChanged = null) : base(onExecuteChanged)
        {

        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Execute(object parameter)
        {
            try
            {
                IsExecuting = true;
                OnExecute(parameter);
            }
            finally
            {
                IsExecuting = false;
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected abstract void OnExecute(object parameter);

    }

    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// </summary>
    public abstract class AsyncDelegateCommandBase : DelegateCommandBase
    {
        #region Variables and Properties
        /// <summary>
        /// Token to cancel async command.
        /// </summary>
        public CancellationTokenSource CancellationToken { get; protected set; }

        /// <summary>
        /// Cancel command.
        /// </summary>
        public DelegateCommand CancelCommand { get; protected set; }
        #endregion

        public AsyncDelegateCommandBase(Action<bool> onExecuteChanged = null) : base(onExecuteChanged)
        {
            ResetCancellationToken();
            CancelCommand = new DelegateCommand(() => CancellationToken?.Cancel());
        }

        public void ResetCancellationToken()
        {
            CancellationToken?.Dispose();
            CancellationToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override async void Execute(object parameter)
        {
            try
            {
                IsExecuting = true;
                await OnExecute(parameter);
            }
            finally
            {
                IsExecuting = false;
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected abstract Task OnExecute(object parameter);
    }
}
