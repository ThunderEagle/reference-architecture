using System;
using System.Linq.Expressions;

namespace Entity.Core.BaseObjects
{
    /// <summary>
    /// 	MemberNameResolver is a utility class that resolves names of members from expressions.
    /// </summary>
    public static class MemberNameResolver
    {
        /// <summary>
        /// 	Gets the name.
        /// </summary>
        /// <typeparam name = "TSource">The type of the source.</typeparam>
        /// <typeparam name = "TMember">The type of the member.</typeparam>
        /// <param name = "memberExpression">The member expression.</param>
        /// <returns></returns>
        public static string GetName<TSource, TMember>(Expression<Func<TSource, TMember>> memberExpression)
        {
            return GetName(typeof(TSource), memberExpression);
        }

        /// <summary>
        /// 	Members the name.
        /// </summary>
        /// <typeparam name = "TSource">The type of the source.</typeparam>
        /// <typeparam name = "TMember">The type of the member.</typeparam>
        /// <param name = "source">The source.</param>
        /// <param name = "memberExpression">The member expression.</param>
        /// <returns></returns>
        public static string GetName<TSource, TMember>(this TSource source, Expression<Func<TSource, TMember>> memberExpression)
        {
            return GetName(memberExpression);
        }

        /// <summary>
        /// 	Members the name.
        /// </summary>
        /// <param name = "sourceType">Type of the source.</param>
        /// <param name = "memberExpression">The member expression.</param>
        /// <returns></returns>
        public static string GetName(Type sourceType, LambdaExpression memberExpression)
        {
            MemberExpression expression = memberExpression.Body as MemberExpression;
            if (expression == null || !expression.Member.DeclaringType.IsAssignableFrom(sourceType))
            {
                throw new ArgumentException("Expression must be a source type member expression.");
            }
            return expression.Member.Name;
        }
    }

}
