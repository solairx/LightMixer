using LightMixer;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using UIFrameWork.Utils;

namespace UIFrameWork
{
    /// <summary>
    /// Base class for DependencyObject view model.Proxy.
    /// </summary>
    public abstract class BaseViewModel :  INotifyPropertyChanged
    {
        

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods



        /// <summary>
        /// Invoked whenever the value of a property on this object has been updated.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName)
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
            LightMixerBootStrap.Dispatcher.Invoke(() =>
            {
                this.OnPropertyChanged(expression);
            });
        }

        public void AsyncOnPropertyChange(string name)
        {
            LightMixerBootStrap.Dispatcher.Invoke(() =>
            {
                this.OnPropertyChanged(name);
            });
        }

        public void RunOnUIThread(Action ac)
        {
            LightMixerBootStrap.Dispatcher.Invoke(() =>
            {
                ac.Invoke();
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

        #endregion Methods
    }
}