using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using UIFrameWork.Utils;

namespace UIFrameWork
{
    /// <summary>
    /// Base class for DependencyObject view model.Proxy.
    /// </summary>
    public abstract class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        protected IEventAggregator EventAggregator
        {
            get;
            set;
        }


        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.DependencyObject"/> has been updated. The specific dependency property that changed is reported in the event data.
        /// </summary>
        /// <param name="e">Event data that will contain the dependency property identifier of interest, the property metadata for the type, and old and new values.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            // only notify for properties on this type
            if (e.Property.OwnerType.IsAssignableFrom(this.GetType()))
            {
                this.OnPropertyChanged(e.Property.Name);
            }
        }

        /// <summary>
        /// Invoked whenever the value of a property on this object has been updated.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged(string propertyName)
        {
            var handlers = this.PropertyChanged;
            if (handlers != null)
            {
                handlers(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual void OnPropertyChanged()
        {
            var handlers = this.PropertyChanged;
            if (handlers != null)
            {
                handlers(this, new PropertyChangedEventArgs(string.Empty));
            }
        }

        protected void DispatchError(System.Exception vExp, BaseViewModel sender)
        {
            //     System.Windows.MessageBox.Show("Une Erreur est survenu dans la génération de cette fonction");
        }

        /// <summary>
        /// Invoked whenever the value of a property on this object has been updated.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        protected virtual void OnPropertyChanged(Expression<Func<object>> propertyExpression)
        {
            this.OnPropertyChanged(Reflect.Property(propertyExpression).Name);
        }


        public void OnPropertyChanged<TProperty>(Expression<Func<BaseViewModel, TProperty>> expression)
        {
            this.OnPropertyChanged(this.GetPropertyName(expression));
        }

        public void AsyncOnPropertyChange<TProperty>(Expression<Func<BaseViewModel, TProperty>> expression)
        {
            Dispatcher.Invoke(() =>
            {
                this.OnPropertyChanged(expression);
            });
        }

        private string GetPropertyName<BaseViewModel, TProperty>(Expression<Func<BaseViewModel, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;

                    if (memberExpression == null)
                        throw new NotImplementedException();
                }
                else
                    throw new NotImplementedException();
            }

            var propertyName = memberExpression.Member.Name;
            return propertyName;
        }



        #endregion
    }
}
