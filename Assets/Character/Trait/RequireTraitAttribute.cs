using System;
using JetBrains.Annotations;

namespace Character.Trait {
    /// <summary>
    ///     This attribute marks target class to require external <see cref="TraitAttribute"/>
    ///     from <see cref="Character"/>'s <see cref="Character.parts"/>
    /// </summary>
    /// <remarks>
    ///     Requires inheritance from <see cref="TraitConsumer"/> base class, that manages how
    ///     traits are retrieved
    /// </remarks>
    /// <example>
    /// <code>
    ///     [RequireTrait(typeof(Bar), count = 3)]
    ///     class Foo : TraitConsumer { ...  }
    /// 
    ///
    ///     class Engine : CharacterPart {
    ///         [Trait]
    ///         private Bar bar;
    ///
    ///         ...
    ///     }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    [BaseTypeRequired(typeof(TraitConsumer))]
    public class RequireTraitAttribute : Attribute {
        /// <summary>
        /// Required type to be provided
        /// </summary>
        public readonly Type requiredType;

        /// <summary>
        /// How many different providers required
        /// </summary>
        public int count;

        /// <summary>
        /// Annotate target to require external trait provider
        /// </summary>
        /// <param name="requiredType">Type of required trait(s)</param>
        public RequireTraitAttribute(Type requiredType) {
            this.requiredType = requiredType;
            count = 5;
        }
    }
}