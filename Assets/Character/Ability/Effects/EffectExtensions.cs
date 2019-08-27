using System.Collections.Generic;
using Character.Affectable;
using Character.EffectModifier;

namespace Character.Ability.Effects {
    public static class EffectExtensions {
        /// <summary>
        /// Extension for <see cref="IEffectModifier{TInEffect,TOutEffect}.ModifyEffect"/>
        /// </summary>
        public static TOutEffect Modify<TInEffect, TOutEffect>(this TInEffect effect, IEffectModifier<TInEffect, TOutEffect> modifier)
            where TInEffect : Effect where TOutEffect : Effect => modifier.ModifyEffect(effect);

        /// <summary>
        /// Extension for <see cref="IAffectable.AffectBy"/>
        /// </summary>
        public static void Affect<TEffect>(this TEffect effect, IAffectable<TEffect> affectable) where TEffect : Effect {
            affectable.AffectBy(effect);
        }
    }
}