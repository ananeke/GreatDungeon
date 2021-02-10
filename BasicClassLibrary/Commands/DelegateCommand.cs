using BasicClassLibrary.Commands;
using System;
using System.Windows.Input;

namespace BaseClassesLibrary.Commands
{
    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// </summary>
    public class DelegateCommand : SyncDelegateCommandBase, ICommand
    {
        #region Variables and Properties
        /// <summary>
        /// Can execute delegate.
        /// </summary>
        public Func<bool> CanExecuteDelegate { get; protected set; }
        /// <summary>
        /// Execute delegate.
        /// </summary>
        public Action ExecuteDelegate { get; protected set; }
        #endregion

        /// <summary>
        /// Creates a new instance of a <see cref="DelegateCommand"/>.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateCommand( Action execute, Func<bool> canExecute = null, Action<bool> onExecuteChanged = null) : base(onExecuteChanged)
        {
            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute ?? throw new ArgumentNullException( "Execute" );
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public override bool CanExecute( object parameter )
        {
            bool canExecute = false;
            try
            {
                canExecute = (CanExecuteDelegate != null) ? (!IsExecuting && CanExecuteDelegate()) : !IsExecuting;
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
        /// <param name="parameter">The parameter.</param>
        protected override void OnExecute( object parameter )
        {
            ExecuteDelegate();
        }
    }

    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// </summary>
    public class DelegateCommand<T> : SyncDelegateCommandBase, IValueTypeCommandBase
    {
        #region Variables and Properties
        /// <summary>
        /// Can execute delegate.
        /// </summary>
        public Func<T, bool> CanExecuteDelegate { get; protected set; }
        /// <summary>
        /// Execute delegate.
        /// </summary>
        public Action<T> ExecuteDelegate { get; protected set; }

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
        #endregion

        /// <summary>
        /// Creates a new instance of a <see cref="DelegateCommand"/>.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null, Action<bool> onExecuteChanged = null)
        {
            Type nullableType = typeof(Nullable<>);
            Type tTypeInfo = typeof(T);

            // For value type can start delegate method only when parameter is not null
            if (tTypeInfo.IsValueType && (!tTypeInfo.IsGenericType || !nullableType.IsAssignableFrom(tTypeInfo)))
                isValueType = true;
            else
                isValueType = false;

            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute ?? throw new ArgumentNullException( "Execute" );
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public override bool CanExecute( object parameter )
        {
            bool canExecute = false;
            try
            {
                bool delegateCanBeExecuted = CanExecuteDelegate != null;

                bool hasValueOrCanBeNull = !IsValueType || (IsValueType && parameter != null);

                canExecute = (delegateCanBeExecuted && hasValueOrCanBeNull)              ? !IsExecuting && CanExecuteDelegate((T)parameter)
                           : (delegateCanBeExecuted && IsValueType && parameter == null) ? false : !IsExecuting;
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
        /// <param name="parameter">The parameter.</param>
        protected override void OnExecute( object parameter )
        {
            if (isValueType && parameter == null)
                ExecuteDelegate(default(T));
            else
                ExecuteDelegate((T)parameter);
        }
    }
}
