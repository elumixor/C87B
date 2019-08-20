using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Shared {
    public static class GeneralExtensions {
        /// <summary>
        ///     Gets <see cref="FieldInfo.FieldType"/> for <see cref="FieldInfo"/>,
        ///     <see cref="PropertyInfo.PropertyType"/> for <see cref="PropertyInfo"/>,
        ///     and <see cref="MethodInfo.ReturnType"/> for <see cref="MethodInfo"/>
        /// </summary>
        /// <param name="memberInfo">Member info</param>
        /// <returns>Underlying type of member</returns>
        /// <exception cref="ArgumentException">
        ///     Throws ArgumentException if memberInfo argument is neither <see cref="FieldInfo"/>, <see cref="PropertyInfo"/> nor
        ///     <see cref="MethodInfo"/> or if given <see cref="MethodInfo"/> had either void return type or more than zero parameters
        /// </exception>
        [Pure, NotNull]
        public static Type GetUnderlyingType(this MemberInfo memberInfo) {
            if ((memberInfo.MemberType & MemberTypes.Field) != 0) return ((FieldInfo) memberInfo).FieldType;
            if ((memberInfo.MemberType & MemberTypes.Property) != 0) return ((PropertyInfo) memberInfo).PropertyType;
            if ((memberInfo.MemberType & MemberTypes.Method) != 0) return ((MethodInfo) memberInfo).ReturnType;
            throw new ArgumentException("Member should be a field, property or method");
        }

        /// <summary>
        /// Helper function to check <see cref="Type.IsAssignableFrom"/>, but works on generic types
        /// <example>
        /// <code>
        ///    class A&lt;T&gt; { ... }
        /// 
        ///
        ///    class B : A &lt;string&gt; { ... }
        /// 
        ///     typeof(B).IsAssignableFrom(typeof(A&lt;&gt;))   // false
        ///     typeof(B).IsGenericChild(typeof(A&lt;&gt;))     // true
        /// </code>
        /// </example>
        /// </summary>
        public static bool IsGenericChild(this Type baseType, Type childType) {
            while (childType != null && childType != typeof(object)) {
                var cur = childType.IsGenericType ? childType.GetGenericTypeDefinition() : childType;
                if (baseType == cur) return true;
                childType = childType.BaseType;
            }
            return false;
        }
    }
}