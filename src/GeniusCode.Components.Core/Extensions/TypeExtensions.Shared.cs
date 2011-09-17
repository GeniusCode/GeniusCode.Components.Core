using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeniusCode.Components.Support.Reflection;

namespace GeniusCode.Components.Extensions
{
    public static class TypeExtensions
    {

        public static bool HasAttribute<TAttribute>(this Type input, bool includeInheritance = false)
        where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(input, includeInheritance) != null;
        }
        public static TAttribute GetAttribute<TAttribute>(this Type input, bool includeInheritance = false) where TAttribute : Attribute
        {
            var attribute = input.GetCustomAttributes<TAttribute>(includeInheritance).SingleOrDefault();
            return attribute;
        }

        public static List<T> GetCustomAttributes<T>(this MemberInfo input, bool includeInheritance = false)
            where T : Attribute
        {
            if (input == null) throw new ArgumentNullException("input");

            return input.GetCustomAttributes(typeof(T), includeInheritance).Cast<T>().ToList(); //.ConvertToList<T>();
        }

        public static MethodInfo MakeGenericMethod(this MethodInfo input, params Type[] types)
        {
            if (input == null) throw new ArgumentNullException("input");

            return input.MakeGenericMethod(types);
        }

        public static object CreateObject(this Type input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return Activator.CreateInstance(input);
        }

        public static bool IsVoidReturnType(this MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException("methodInfo");
            return methodInfo.ReturnType == typeof(void);
        }

        public static object GetDefaultValue(this Type input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return ReflectionHelper.GetDefaultValueForType(input);
        }


        public static bool HasDefaultConstructor(this Type type)
        {
            if (type.IsValueType)
                return true;

            var constructor = type.GetConstructor(Type.EmptyTypes);

            return constructor != null;
        }

        public static bool IsEnumerableOf<T>(this Type enumerableType)
        {
            if (enumerableType == null) throw new ArgumentNullException("enumerableType");

            return IsEnumerableOf(enumerableType, typeof(T));
        }

        public static bool IsEnumerableOf(this Type enumerableType, Type itemType)
        {
            if (enumerableType == null) throw new ArgumentNullException("enumerableType");
            if (itemType == null) throw new ArgumentNullException("itemType");

            var realItemType = TryAsEnumerableGetEnumeratedItemType(enumerableType);
            return realItemType == itemType;
        }

        public static bool IsEnumerableGenericType(this Type enumerableType)
        {
            if (enumerableType == null) throw new ArgumentNullException("enumerableType");


            // Try get item type.  If not null, then it is a generic collection
            return enumerableType.TryAsEnumerableGetEnumeratedItemType() != null;
        }

        public static Type TryAsEnumerableGetEnumeratedItemType(this Type enumerableType)
        {
            if (enumerableType == null) throw new ArgumentNullException("enumerableType");

            // get interfaces
            var interfaces = enumerableType.GetInterfaces().ToList();
            // add existing, if also interface
            // this is needed for IEnumerable<T> being directly entered
            if (enumerableType.IsInterface)
                interfaces.Add(enumerableType);

            // get interface that is generic implementer of IEnumerable<>
            var innerType = from iface in interfaces
                            where iface.IsGenericType
                            where iface.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                            let genericArg = iface.GetGenericArguments().Single()
                            select genericArg;

            // return contrained type
            return innerType.SingleOrDefault();
        }

        public static bool IsAnonymous(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type.Name.StartsWith("<>");
        }

        public static string ToNameString(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var typeParameters = type.GetGenericArguments();

            // If contains open generic parameters - get generic defnition
            // This allows us to capture the full namespace of the generic type
            if (type.IsGenericType && type.ContainsGenericParameters)
                type = type.GetGenericTypeDefinition();

            var typeName = type.Name;
            if (!string.IsNullOrEmpty(type.FullName))
                typeName = type.FullName;

            if (!type.IsGenericType)
                return typeName;

            var value = typeName.Substring(0, typeName.IndexOf('`')) + "<";
            var list = new List<string>();
            for (var i = 0; i < typeParameters.Length; i++)
            {
                value += "{" + i + "},";
                var s = ToNameString(typeParameters[i]);
                list.Add(s);
            }
            value = value.TrimEnd(',');
            value += ">";
            value = string.Format(value, list.ToArray<string>());
            return value;

        }
    }
}
