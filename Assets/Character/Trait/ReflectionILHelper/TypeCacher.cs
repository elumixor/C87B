using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shared;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Character.Trait.ReflectionILHelper {
    public static class TypeCacher {
        /// <summary>
        /// <para>
        ///     Collection of types, deriving from <see cref="CharacterPart{TData}"/> and mapped publicly available types (traits)
        ///     via fields or properties with functions to access them.
        /// </para>
        /// <para>
        ///     Used to optimize trait provider registration time in <see cref="Character.AddPart"/>
        /// </para>
        /// </summary>
        public static Dictionary<Type, (Type, GetterAbstract)[]> type2ProvidedTypes;

        /// <summary>
        /// <para>
        ///     Dictionary of types, deriving from <see cref="TraitConsumer"/> to array of types,
        ///     provided via <see cref="TraitAttribute"/> 
        /// </para>
        /// <para>
        ///     Used to optimise trait consumer registration time in <see cref="Character.AddTraitConsumer"/>
        /// </para>
        /// </summary>
        public static Dictionary<Type, Type[]> consumerType2RequiredTypes;

        /// <summary>
        /// Binding flags
        /// </summary>
        private const BindingFlags FLags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Updates <see cref="type2ProvidedTypes"/> and <see cref="consumerType2RequiredTypes"/>
        /// </summary>
        [DidReloadScripts]
        private static void UpdateTypesDictionary() {
            // Get all types in current domain
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes())
                .ToArray();
            
            var characterPartType = typeof(CharacterPart);

            // Get all subclasses of CharacterPart
            var derivedTypes = types.Where(characterPartType.IsAssignableFrom);

            // Fill dictionary with provided types
            type2ProvidedTypes =  derivedTypes.ToDictionary(
                t => t,
                t => t.GetMembers(FLags).Where(member => Attribute.IsDefined(member, typeof(TraitAttribute), true))
                    .Select(mi => (Type: mi.GetUnderlyingType(), GetterAbstract: DelegateCreator.CreateDelegate(mi))).ToArray());
            
            var consumerTraitType = typeof(TraitConsumer);
            var requireTraitAttributeType = typeof(RequireTraitAttribute);

            var consumerTypes = types.Where(consumerTraitType.IsAssignableFrom);

            // Fill dictionary
            consumerType2RequiredTypes = consumerTypes.ToDictionary(
                t => t,
                t => t.GetCustomAttributes(requireTraitAttributeType, true)
                    .Select(attrib => ((RequireTraitAttribute) attrib).requiredType).ToArray());
        }
    }
}