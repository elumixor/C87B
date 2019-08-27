using System.Collections.Generic;
using Character.Ability.Effects;
using Character.Affectable;
using Character.EffectModifier;
using UnityEngine;

namespace Character.Targeting {
    /// <summary>
    /// Targetable by <see cref="Effect"/>s.
    /// Allows to be targeted with abilities, that apply various effects.
    /// Thus should expose list of affectables and traits
    /// </summary>
    public abstract class Targetable : MonoBehaviour, IAffectableContainer, IEffectModifierContainer {
        public abstract IEnumerable<IAffectable> Affectables { get; }
        public abstract IEnumerable<IEffectModifier> Modifiers { get; }
        public abstract void ReceiveDamage(float damage);
        public abstract void ReceiveHeal(float heal);
    }
}