using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace FightScene.Behaviours.Character.Traits {
    public interface ICharacterTrait {}

    public interface ICharacterTraitContainer {
        /// <summary>
        /// Traits, that modify effect
        /// </summary>
        ICharacterTrait[] Traits { get; }
    }
    
    public static class CharacterTraitContainerExtensions {
        [CanBeNull]
        public static T GetTrait<T>(this ICharacterTraitContainer traitContainer) where T : class, ICharacterTrait =>
            traitContainer.Traits.First(a => a is T) as T;

        public static IEnumerable<T> GetTraits<T>(this ICharacterTraitContainer traitContainer) where T : class, ICharacterTrait =>
            traitContainer.Traits.Select(a => a as T).Where(a => a != null);

        public static bool HasTrait<T>(this ICharacterTraitContainer traitContainer, T trait) where T : ICharacterTrait =>
            traitContainer.Traits.Contains(trait);
    }
}