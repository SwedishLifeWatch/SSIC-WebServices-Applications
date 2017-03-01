using System;
using System.Linq.Expressions;

namespace ArtDatabanken.WebApplication
{
    /// <summary>
    /// Extract property name out of a property
    /// </summary>
    public static class ReflectionUtility //todo - flytta till ArtDatabanken.WebApplication
    {
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            var body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }
    }
}
