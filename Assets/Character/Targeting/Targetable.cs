using Character.Ability.Effects;
using Character.Affectable;
using Character.EffectModifier;

namespace Character.Targeting {
    /// <summary>
    /// Targetable by <see cref="IEffect"/>s.
    /// Allows to be targeted with abilities, that apply various effects.
    /// Thus should expose list of affectables and traits
    /// </summary>
    public interface ITargetable : IAffectableContainer, IEffectModifierContainer {}
}