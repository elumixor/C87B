using Character.Ability.Effects;

namespace Character.Ability {
    /// <summary>
    /// Active ability that can be executed
    /// Executing specific ability can have multiple effects
    /// </summary>
    public class Ability {
        /// <summary>
        /// Combo, required for ability execution
        /// </summary>
        public Combo.Domain.Combo combo;

        /// <summary>
        /// Ability effects
        /// </summary>
        public IEffect[] effects;
    }
}