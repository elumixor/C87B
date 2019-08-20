using Character;
using Character.Ability;
using Character.Targeting;
using Combo;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scenes.FightScene {
    /// <summary>
    /// Ability is an intractable mesh on <see cref="Character"/>'s UI, that invokes specific combo on touch
    /// </summary>
    public class AbilityButton : MonoBehaviour, IPointerDownHandler {
        /// <summary>
        /// Ability to be executed on touch
        /// </summary>
        [SerializeField] private Ability ability;

        public IAbilityExecutor executor;
        public ITargeter targeter;
        
        public ComboManager comboManager;

        public void OnPointerDown(PointerEventData eventData) {
            comboManager.BeginCombo(ability.combo, accuracy => executor.ExecuteAbility(ability, targeter, accuracy), () => { });
        }
    }
}