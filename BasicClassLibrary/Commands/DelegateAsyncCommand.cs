using BasicClassLibrary.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaseClassesLibrary.Commands
{
    public class DelegateAsyncCommand : AsyncDelegateCommandBase
    {
        /// <summary>
        /// Can execute delegate.
        /// </summary>
        public Func<bool> CanExecuteDelegate { get; protected set; }

        /// <summary>
        /// Execute delegate.
        /// </summary>
        public Func<CancellationToken, Task> ExecuteDelegate { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateAsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateAsyncCommand( Func<CancellationToken, Task> execute, Func<bool> canExecute = null, Action<bool> onExecuteChanged = null) : base(onExecuteChanged)
        {
            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute ?? throw new ArgumentNullException( "Execute" );
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed(default if CanExecuteDelegate not set); otherwise, false.
        /// </returns>
        public override bool CanExecute( object parameter )
        {
            bool canExecute = false;
            try
            {
                canExecute = !IsExecuting && !CancellationToken.IsCancellationRequested;
                if (CanExecuteDelegate != null)
                    canExecute &= CanExecuteDelegate();
            }
            catch
            {
                canExecute = false;
            }
            return canExecute;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        protected override async Task OnExecute( object parameter )
        {
            await ExecuteDelegate(CancellationToken.Token);
        }
    }

    public class DelegateAsyncCommand<T> : AsyncDelegateCommandBase, IValueTypeCommandBase
    {

        /// <summary>
        /// Can execute delegate.
        /// </summary>
        public Func<T, bool> CanExecuteDelegate { get; private set; }

        /// <summary>
        /// Execute delegate.
        /// </summary>
        public Func<T, CancellationToken, Task> ExecuteDelegate { get; private set; }

        protected bool isValueType;
        /// <summary>
        /// Occurs when the T type is value Type.
        /// </summary>
        public bool IsValueType
        {
            get
            {
                return isValueType;
            }
            protected set
            {
                isValueType = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateAsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateAsyncCommand(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute = null, Action<bool> onExecuteChanged = null) : base(onExecuteChanged)
        {
            Type nullableType = typeof(Nullable<>);
            Type tTypeInfo = typeof(T);

            // For value type can start delegate method only when parameter is not null
            if (tTypeInfo.IsValueType && (!tTypeInfo.IsGenericType || !nullableType.IsAssignableFrom(tTypeInfo)))
                isValueType = true;
            else
                isValueType = false;

            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute ?? throw new ArgumentNullException("Execute");
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed(default if CanExecuteDelegate not set); otherwise, false.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            bool canExecute = false;
            try
            {
                canExecute = !IsExecuting && !CancellationToken.IsCancellationRequested;

                if (canExecute && CanExecuteDelegate != null)
                {
                    bool hasValueOrCanBeNull = !IsValueType || (IsValueType && parameter != null);
                    if (hasValueOrCanBeNull)
                        canExecute &= CanExecuteDelegate((T)parameter);
                    else if (IsValueType && parameter == null)
                        canExecute = false;
                }
            }
            catch
            {
                canExecute = false;
            }
            return canExecute;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        protected override async Task OnExecute(object parameter)
        {
            if (isValueType && parameter == null)
                await ExecuteDelegate(default(T), CancellationToken.Token);
            else
                await ExecuteDelegate((T)parameter, CancellationToken.Token);
        }
    }
}
