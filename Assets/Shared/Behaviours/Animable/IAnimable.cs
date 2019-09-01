using UnityEngine;

namespace Shared.Behaviours.Animable {
    /// <summary>
    /// IAnimable interface exposes <see cref="Animator"/> component, allowing extension methods in <see cref="AnimableExtensions"/>
    /// </summary>
    public interface IAnimable {
        /// <summary>
        /// Animator component
        /// </summary>
        Animator Animator { get; }
    }
}