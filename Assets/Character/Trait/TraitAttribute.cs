using System;

namespace Character.Trait {
    /// <summary>
    /// <para>
    ///     Trait attribute marks member is a providable trait for <see cref="TraitConsumer"/>, marked
    ///     with <see cref="RequireTraitAttribute"/>
    /// </para>
    /// <example>
    /// <code>
    ///     public class Foo {
    ///         [Trait] public int Bar = 5;               // Public field as trait
    ///         [Trait] private string FBar => "example"; // Private property as trait
    ///         [Trait] private AnotherClass() { ... }    // Private method as trait 
    ///     }
    ///
    /// 
    ///     [RequireTrait(typeof(Bar), count = 3)]
    ///     class Foo : TraitConsumer { ...  }
    /// </code>
    /// </example>
    /// <remarks>
    ///    If used on method, it should have non-void return type and take no arguments
    /// </remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class TraitAttribute : Attribute { }
}