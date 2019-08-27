using Character.Targeting;
using Combo;
using UnityEngine;

namespace Character.AbilityDisplay {
    /// <summary>
    /// 
    /// </summary>
    public class AbilityIcon : MonoBehaviour {
        public Ability.Ability ability;
        public ComboManager comboManager;
        public Targeter targeter;

        public void OnMouseDown() => comboManager.BeginCombo(ability.ComboData, OnSuccess, OnFail);

        private void OnSuccess(float accuracy) {
            foreach (var (effect, targetType) in ability.effects) {
                effect.ApplyTo(targeter[targetType], accuracy);
            }
        }

        private void OnFail() { }
    }
}