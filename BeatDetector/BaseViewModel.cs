using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BeatDetector
{
    /// <summary>
    /// Base class for DependencyObject view model.Proxy.
    /// </summary>
    public abstract class ViewModelBase :  INotifyPropertyChanged
    {
        
        
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        #region Methods


        /// <summary>
        /// Invoked whenever the value of a property on this object has been updated.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged(string propertyName)
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

        protected void DispatchError(System.Exception vExp, ViewModelBase sender)
        {
            //     System.Windows.MessageBox.Show("Une Erreur est survenu dans la génération de cette fonction");
        }

        


        public void OnPropertyChanged<TProperty>(Expression<Func<ViewModelBase, TProperty>> expression)
        {
            this.OnPropertyChanged(this.GetPropertyName(expression));
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
