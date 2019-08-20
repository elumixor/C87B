using System.Collections.Generic;
using System.Linq;
using Character.Ability.Effects;

namespace Character.Affectable {
    public static class AffectableContainerExtensions {
        /// <summary>
        /// Get affectables of type
        /// </summary>
        /// <param name="container">Affectable container</param>
        /// <typeparam name="TEffect">Affectable effect type</typeparam>
        /// <returns></returns>
        public static IEnumerable<IAffectable<TEffect>> GetAffectables<TEffect>(this IAffectableContainer container) where TEffect : IEffect =>
            container.Affectables.OfType<IAffectable<TEffect>>();

        /// <summary>
        /// Get first affectable ot type
        /// </summary>
        /// <param name="container">Affectable container</param>
        /// <typeparam name="TEffect">Effect type</typeparam>
        /// <returns></returns>
        public static IAffectable<TEffect> GetAffectable<TEffect>(this IAffectableContainer container) where TEffect : IEffect =>
            container.GetAffectables<TEffect>().First();
    }
}