using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GeniusCode.Components.Extensions;

namespace GeniusCode.Components.Support.Reflection
{
    public static class ReflectionHelper
    {
        static public string GetAssemblyDirectory(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            var codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var output = Path.GetDirectoryName(path);
            return output;
        }

        public static object GetDefaultValueForProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");

            var i = propertyInfo.GetCustomAttributes<DefaultValueAttribute>(true).SingleOrDefault();
            return i!= null ? i.Value : GetDefaultValueForType(propertyInfo.PropertyType);
        }

        public static object GetDefaultValueForType(Type myType)
        {
            if (myType == null) throw new ArgumentNullException("myType");

            return !myType.IsValueType ? null : Activator.CreateInstance(myType);
        }


        public static BindingFlags DefaultFlags
        {
            get
            {
                return BindingFlags.Default | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                BindingFlags.NonPublic;
            }
        }

        public static IEnumerable<TMemberInfo> GetMembersWithAttribute<TAttribute, TMemberInfo>(IEnumerable<TMemberInfo> members)
            where TMemberInfo : MemberInfo
            where TAttribute : Attribute
        {

            var toDraw = from TMemberInfo c in members
                         where c.GetCustomAttributes(typeof(TAttribute), true).Count() > 1
                         select c;

            return toDraw;
        }

        public static PropertyInfo ExtractPropertyInfo<TObject, TValue>(Expression<Func<TObject, TValue>> expr)
        {
            return ExtractPropertyInfoForLambdaExpression(expr);
        }

        public static PropertyInfo ExtractPropertyInfo<TValue>(Expression<Func<TValue>> expr)
        {
            return ExtractPropertyInfoForLambdaExpression(expr);
        }

        public static void RaiseNotifyPropertyChanged<TValue>(this INotifyPropertyChanged input, Expression<Func<TValue>> expr, PropertyChangedEventHandler handler)
        {
            if (handler == null) return;
            var propertyInfo = ExtractPropertyInfo(expr);
            handler.Invoke(input, new PropertyChangedEventArgs(propertyInfo.Name));
        }

        private static PropertyInfo ExtractPropertyInfoForLambdaExpression(LambdaExpression expr)
        {
            if (expr == null) throw new ArgumentNullException("expr");

            var memberExpression = expr.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("The expression is not a member access expression.", "expr");

            var property = memberExpression.Member as PropertyInfo;

            if (property == null)
                throw new ArgumentException("The member access expression does not access a property.", "expr");

            return property;
        }


    }

}
