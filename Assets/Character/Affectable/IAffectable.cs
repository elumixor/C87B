using Character.Ability.Effects;

namespace Character.Affectable {
    /// <summary>
    /// Affectable allows specific effect to do something. It defines behaviour under specific effect and also marks it
    /// as targetable for that specific effect
    /// </summary>
    public interface IAffectable<in TEffect> where TEffect : Effect {
        void AffectBy(TEffect effect);
    }
    /// <summary>
    /// Generic <see cref="IAffectable{TEffect}"/>
    /// </summary>
    public interface IAffectable : IAffectable<Effect> {}
}