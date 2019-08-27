using Character.Ability.Effects;

namespace Character.EffectModifier {
    /// <summary>
    /// Interface to mark modifiable effect
    /// </summary>
    /// <typeparam name="TInEffect">In effect type</typeparam>
    /// <typeparam name="TOutEffect">Out effect type</typeparam>
    public interface IEffectModifier<in TInEffect, out TOutEffect> where TInEffect : Effect where TOutEffect : Effect {
        /// <summary>
        /// Changes effect into a new one
        /// </summary>
        /// <param name="effect">In effect</param>
        /// <returns>Out effect</returns>
        TOutEffect ModifyEffect(TInEffect effect);
    }

    /// <summary>
    /// Generic effect modifier to custom out effect
    /// </summary>
    /// <typeparam name="TOutEffect">Out effect type</typeparam>
    public interface IEffectModifier<out TOutEffect> : IEffectModifier<Effect, TOutEffect> where TOutEffect : Effect { }

    /// <summary>
    /// Generic effect modifier
    /// </summary>
    public interface IEffectModifier : IEffectModifier<Effect, Effect> { }
}