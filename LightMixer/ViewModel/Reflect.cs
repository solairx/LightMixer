using System;
using System.Linq.Expressions;
using System.Reflection;

namespace UIFrameWork.Utils
{
    /// <summary>
    /// Class used to perform LINQ expression-based reflection.
    /// </summary>
    public static class Reflect
    {
        /// <summary>
        /// Obtains the MemberInfo object corresponding to the expression.
        /// </summary>
        /// <param name="expr">An expression pointing to a member.</param>
        /// <returns>The MemberInfo corresponding to the expression.</returns>
        public static MemberInfo Member(Expression<Func<object>> expr)
        {
            return MemberImpl(expr);
        }
        public static MemberInfo Member<T>(Expression<Func<T>> expr)
        {
            return MemberImpl(expr);
        }

        /// <summary>
        /// Obtains the MemberInfo object corresponding to the expression.
        /// </summary>
        /// <typeparam name="T">The type of the class to recover the member for.</typeparam>
        /// <param name="expr">An expression pointing to a member.</param>
        /// <returns>The MemberInfo corresponding to the expression.</returns>
        public static MemberInfo Member<T>(Expression<Func<T, object>> expr)
        {
            return MemberImpl(expr);
        }

        public static MemberInfo Member<T, TA>(Expression<Func<T, TA>> expr)
        {
            return MemberImpl(expr);
        }

        /// <summary>
        /// Obtains the MemberInfo object corresponding to the expression.
        /// </summary>
        /// <param name="expr">An expression pointing to a member.</param>
        /// <returns>The MemberInfo corresponding to the expression.</returns>
        public static MemberInfo Member(Expression<Action> expr)
        {
            return MemberImpl(expr);
        }

        public static object GetValueFromContactExpression(Expression expression)
        {
            Expression conversion = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(conversion);
            var getter = getterLambda.Compile();
            return getter();

        }

        /// <summary>
        /// Obtains the MemberInfo object corresponding to the expression.
        /// </summary>
        /// <typeparam name="T">The type of the class to recover the member for.</typeparam>
        /// <param name="expr">An expression pointing to a member.</param>
        /// <returns>The MemberInfo corresponding to the expression.</returns>
        public static MemberInfo Member<T>(Expression<Action<T>> expr)
        {
            return MemberImpl(expr);
        }

        public static MemberInfo Member<T, TA>(Expression<Action<T, TA>> expr)
        {
            return MemberImpl(expr);
        }

        /// <summary>
        /// Obtains the FieldInfo object corresponding to the expression.
        /// </summary>
        /// <param name="expr">An expression pointing to a field.</param>
        /// <returns>The FieldInfo corresponding to the expression.</returns>
        public static FieldInfo Field(Expression<Func<object>> expr)
        {
            var fi = Member(expr) as FieldInfo;

            if (fi != null)
            {
                return fi;
            }
            else
            {
                throw new ArgumentException("Target is not a field.");
            }
        }

        /// <summary>
        /// Obtains the FieldInfo object corresponding to the expression.
        /// </summary>
        /// <typeparam name="T">The type of the class to recover the field for.</typeparam>
        /// <param name="expr">An expression pointing to a field.</param>
        /// <returns>The FieldInfo corresponding to the expression.</returns>
        public static FieldInfo Field<T>(Expression<Func<T, object>> expr)
        {
            var fi = Member(expr) as FieldInfo;

            if (fi != null)
            {
                return fi;
            }
            else
            {
                throw new ArgumentException("Target is not a field.");
            }
        }

        /// <summary>
        /// Obtains the PropertyInfo object corresponding to the expression.
        /// </summary>
        /// <param name="expr">An expression pointing to a property.</param>
        /// <returns>The PropertyInfo corresponding to the expression.</returns>
        public static PropertyInfo Property<T>(Expression<Func<T>> expr)
        {
            var pi = Member<T>(expr) as PropertyInfo;

            if (pi != null)
            {
                return pi;
            }
            else
            {
                throw new ArgumentException("Target is not a property.");
            }
        }

        public static PropertyInfo Property<T, TA>(Expression<Func<T, TA>> expr)
        {
            var pi = Member<T, TA>(expr) as PropertyInfo;

            if (pi != null)
            {
                return pi;
            }
            else
            {
                throw new ArgumentException("Target is not a property.");
            }
        }

        public static PropertyInfo Property(Expression<Func<object>> expr)
        {
            var pi = Member(expr) as PropertyInfo;

            if (pi != null)
            {
                return pi;
            }
            else
            {
                throw new ArgumentException("Target is not a property.");
            }
        }

        /// <summary>
        /// Obtains the PropertyInfo object corresponding to the expression.
        /// </summary>
        /// <typeparam name="T">The type of the class to recover the property for.</typeparam>
        /// <param name="expr">An expression pointing to a property.</param>
        /// <returns>The PropertyInfo corresponding to the expression.</returns>
        public static PropertyInfo Property<T>(Expression<Func<T, object>> expr)
        {
            var pi = Member(expr) as PropertyInfo;

            if (pi != null)
            {
                return pi;
            }
            else
            {
                throw new ArgumentException("Target is not a property.");
            }
        }

        /// <summary>
        /// Obtains the MethodInfo object corresponding to the expression.
        /// </summary>
        /// <param name="expr">An expression pointing to a method.</param>
        /// <returns>The MethodInfo corresponding to the expression.</returns>
        public static MethodInfo Method(Expression<Action> expr)
        {
            var mi = Member(expr) as MethodInfo;

            if (mi != null)
            {
                return mi;
            }
            else
            {
                throw new ArgumentException("Target is not a method.");
            }
        }

        /// <summary>
        /// Obtains the MethodInfo object corresponding to the expression.
        /// </summary>
        /// <param name="expr">An expression pointing to a method.</param>
        /// <returns>The MethodInfo corresponding to the expression.</returns>
        public static MethodInfo Method(Expression<Func<object>> expr)
        {
            var mi = Member(expr) as MethodInfo;

            if (mi != null)
            {
                return mi;
            }
            else
            {
                throw new ArgumentException("Target is not a method.");
            }
        }

        /// <summary>
        /// Obtains the MethodInfo object corresponding to the expression.
        /// </summary>
        /// <typeparam name="T">The type of the class to get the method for.</typeparam>
        /// <param name="expr">An expression pointing to a method.</param>
        /// <returns>The MethodInfo corresponding to the expression.</returns>
        public static MethodInfo Method<T>(Expression<Action<T>> expr)
        {
            var mi = Member(expr) as MethodInfo;

            if (mi != null)
            {
                return mi;
            }
            else
            {
                throw new ArgumentException("Target is not a method.");
            }
        }

        /// <summary>
        /// Obtains the MethodInfo object corresponding to the expression.
        /// </summary>
        /// <typeparam name="T">The type of the class to get the method for.</typeparam>
        /// <param name="expr">An expression pointing to a method.</param>
        /// <returns>The MethodInfo corresponding to the expression.</returns>
        public static MethodInfo Method<T>(Expression<Func<T, object>> expr)
        {
            var mi = Member(expr) as MethodInfo;

            if (mi != null)
            {
                return mi;
            }
            else
            {
                throw new ArgumentException("Target is not a method.");
            }
        }

        /// <summary>
        /// Obtains the EventInfo object corresponding to the expression.
        /// </summary>
        /// <param name="expr">The expression pointing to an Event.</param>
        /// <returns>The EventInfo corresponding to the expression.</returns>
        public static EventInfo Event(Expression<Func<object>> expr)
        {
            var mi = Member(expr);

            var ei = mi.DeclaringType.GetEvent(mi.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            if (ei != null)
            {
                return ei;
            }
            else
            {
                throw new ArgumentException("Target is not an event.");
            }
        }

        /// <summary>
        /// Obtains the EventInfo object corresponding to the expression.
        /// </summary>
        /// <typeparam name="T">The type of the object containing the event.</typeparam>
        /// <param name="expr">The expression pointing to an Event.</param>
        /// <returns>The EventInfo corresponding to the expression.</returns>
        public static EventInfo Event<T>(Expression<Func<T, object>> expr)
        {
            var mi = Member(expr);

            var ei = typeof(T).GetEvent(mi.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            if (ei != null)
            {
                return ei;
            }
            else
            {
                throw new ArgumentException("Target is not an event.");
            }
        }

        /// <summary>
        /// Recovers the MemberInfo object corresponding to the expression.
        /// </summary>
        /// <typeparam name="T">The type of the expression.</typeparam>
        /// <param name="expr">An expression pointing to a member.</param>
        /// <returns>The MemberInfo corresponding to the expression.</returns>
        private static MemberInfo MemberImpl<T>(Expression<T> expr)
        {
            var body = expr.Body;

            // if there is a conversion unary expression on top of the expression, extract the underlying operand
            var unary = body as UnaryExpression;
            if (unary != null && unary.NodeType == ExpressionType.Convert)
            {
                body = unary.Operand;
            }

            var memberExpression = body as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member;
            }

            var methodCallExpression = body as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return methodCallExpression.Method;
            }

            throw new ArgumentException("Expression is not a valid MemberExpression or MethodCallExpression.");
        }
    }
}
