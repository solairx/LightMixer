﻿using System.Windows.Input;
using System.Diagnostics;

namespace LightMixer.Model
{
    public class DelegateCommand : ICommand
    {
        #region Static disabled command

        /// <summary>
        /// A <see cref="DelegateCommand"/> instance that does nothing and can never be executed.
        /// </summary>
        public static readonly DelegateCommand Disabled = new DelegateCommand(() => { }) { IsEnabled = false };

        #endregion Static disabled command

        #region Private data

        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
        private List<WeakReference> weakHandlers;
        private bool? isEnabled;
        private bool raiseCanExecuteChangedPending;

        #endregion Private data

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action execute)
            : this(execute != null ? p => execute() : (Action<object>)null, (Func<object, bool>)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action execute, Func<bool> canExecute)
            : this(execute != null ? p => execute() : (Action<object>)null, canExecute != null ? p => canExecute() : (Func<object, bool>)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action execute, Func<object, bool> canExecute)
            : this(execute != null ? p => execute() : (Action<object>)null, canExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action<object> execute)
            : this(execute, (Func<object, bool>)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action<object> execute, Func<bool> canExecute)
            : this(execute, canExecute != null ? p => canExecute() : (Func<object, bool>)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion Constructors

        #region CanExecuteChanged event

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (weakHandlers == null)
                {
                    weakHandlers = new List<WeakReference>(new[] { new WeakReference(value) });
                }
                else
                {
                    weakHandlers.Add(new WeakReference(value));
                }
            }
            remove
            {
                if (weakHandlers == null) return;

                for (int i = weakHandlers.Count - 1; i >= 0; i--)
                {
                    WeakReference weakReference = weakHandlers[i];
                    EventHandler handler = weakReference.Target as EventHandler;
                    if (handler == null || handler == value)
                    {
                        weakHandlers.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CanExecuteChanged"/> event.
        /// </summary>
        [DebuggerStepThrough]
        public void RaiseCanExecuteChanged() => OnCanExecuteChanged();

        /// <summary>
        /// Raises the <see cref="E:CanExecuteChanged"/> event after all other processing has
        /// finished. Multiple calls to this function before the asynchronous action has been
        /// started are ignored.
        /// </summary>
        

        /// <summary>
        /// Raises the <see cref="E:CanExecuteChanged"/> event.
        /// </summary>
        [DebuggerStepThrough]
        protected virtual void OnCanExecuteChanged()
        {
            raiseCanExecuteChangedPending = false;
            PurgeWeakHandlers();
            if (weakHandlers == null) return;

            WeakReference[] handlers = weakHandlers.ToArray();
            foreach (WeakReference reference in handlers)
            {
                EventHandler handler = reference.Target as EventHandler;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        [DebuggerStepThrough]
        private void PurgeWeakHandlers()
        {
            if (weakHandlers == null) return;

            for (int i = weakHandlers.Count - 1; i >= 0; i--)
            {
                if (!weakHandlers[i].IsAlive)
                {
                    weakHandlers.RemoveAt(i);
                }
            }

            if (weakHandlers.Count == 0)
                weakHandlers = null;
        }

        #endregion CanExecuteChanged event

        #region ICommand methods

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) => isEnabled ?? canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Convenience method that invokes CanExecute without parameters.
        /// </summary>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool CanExecute() => CanExecute(null);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <exception cref="InvalidOperationException">The <see cref="CanExecute(object)"/> method returns false.</exception>
        [DebuggerStepThrough]
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("The command cannot be executed because CanExecute returned false.");
            }
            execute(parameter);
        }

        /// <summary>
        /// Convenience method that invokes the command without parameters.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="CanExecute(object)"/> method returns false.</exception>
        [DebuggerStepThrough]
        public void Execute() => Execute(null);

        /// <summary>
        /// Invokes the command if the <see cref="CanExecute(object)"/> method returns true.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command was executed; otherwise, false.</returns>
        public bool TryExecute(object parameter)
        {
            if (CanExecute(parameter))
            {
                Execute(parameter);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Convenience method that invokes the command without parameters if the
        /// <see cref="CanExecute(object)"/> method returns true.
        /// </summary>
        /// <returns>true if this command was executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool TryExecute() => TryExecute(null);

        #endregion ICommand methods

        #region Enabled state

        /// <summary>
        /// Gets or sets a value indicating whether the current DelegateCommand is enabled. If this
        /// value is not null, it takes precedence over the canExecute function was passed in the
        /// constructor. If no function was passed and this value is null the command is enabled.
        /// </summary>
        public bool? IsEnabled
        {
            [DebuggerStepThrough]
            get
            {
                return isEnabled;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    RaiseCanExecuteChanged();
                }
            }
        }

        #endregion Enabled state
    }
}


