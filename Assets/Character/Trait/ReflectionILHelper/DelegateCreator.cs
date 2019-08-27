using System;
using System.Reflection;
using System.Reflection.Emit;
using JetBrains.Annotations;
using Shared;

namespace Character.Trait.ReflectionILHelper {
    /// <summary>
    /// <para>
    ///     Abstract getter for field, property or method value of a type
    /// </para>
    /// <para>
    ///     Used at <see cref="TypeCacher.UpdateTypesDictionary"/> after scripts recompile to
    ///     assign generated methods to delegates and cache them
    /// </para>
    /// </summary>
    /// <param name="classInstance">Instance of a class, that the getter was created for</param>
    public delegate object GetterAbstract([NotNull] object classInstance);

    /// <summary>
    ///     Like <see cref="GetterAbstract"/>, but with classInstance already substituted with
    ///     particular instance
    /// </summary>
    public delegate object Getter();

    public static class DelegateCreator {
        /// <summary>
        /// Creates <see cref="GetterAbstract"/> delegate for field, property, or method
        /// </summary>
        /// <param name="memberInfo">Member to be created delegate for</param>
        /// <returns>Getter delegate</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        [Pure, NotNull]
        public static GetterAbstract CreateDelegate([NotNull] this MemberInfo memberInfo) {
            var underlyingType = memberInfo.GetUnderlyingType();
            if (memberInfo.DeclaringType == null) throw new ArgumentNullException(nameof(memberInfo.DeclaringType));

            var method = new DynamicMethod($"getter_{memberInfo.Name}", typeof(object), new[] {typeof(object)}, true);
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, memberInfo.DeclaringType);

            if ((memberInfo.MemberType & MemberTypes.Field) != 0) il.Emit(OpCodes.Ldfld, (FieldInfo) memberInfo);
            if ((memberInfo.MemberType & MemberTypes.Property) != 0) il.Emit(OpCodes.Callvirt, ((PropertyInfo) memberInfo).GetMethod);
            if ((memberInfo.MemberType & MemberTypes.Method) != 0) {
                var methodInfo = (MethodInfo) memberInfo;
                if (underlyingType == typeof(void) || methodInfo.GetGenericArguments().Length > 0)
                    throw new ArgumentException("Method should have zero arguments and have non-void return type");

                il.Emit(OpCodes.Callvirt, methodInfo);
            }
            
            if (underlyingType.IsValueType) il.Emit(OpCodes.Box, underlyingType);

            il.Emit(OpCodes.Ret);
            return (GetterAbstract) method.CreateDelegate(typeof(GetterAbstract));
        }

        [Pure, NotNull]
        public static Getter Substitute(this GetterAbstract getter, [NotNull] object instance) => () => getter(instance);
    }
}