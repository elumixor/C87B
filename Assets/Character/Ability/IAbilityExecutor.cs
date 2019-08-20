using Character.Targeting;

namespace Character.Ability {
    /// <summary>
    /// Ability executor allows executing abilities
    /// </summary>
    public interface IAbilityExecutor {
        /// <summary>
        /// On successful ability execution
        /// </summary>
        /// <param name="ability">Ability to be executed</param>
        /// <param name="targeter">Targeter</param>
        /// <param name="accuracy">Execution accuracy</param>
        void ExecuteAbility(Ability ability, ITargeter targeter, float accuracy);
    }
}