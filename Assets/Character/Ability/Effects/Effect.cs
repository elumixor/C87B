using Character.Targeting;
using Meta;
using UnityEngine;

namespace Character.Ability.Effects {
    [CreateAssetMenu(fileName = "Effect", menuName = "Custom/Effect")]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class Effect : ScriptableObject {
        [SerializeField] private float damage;
        [SerializeField] private float heal;

        [SerializeField] private bool healFirst;

        /// <summary>
        /// Applies this effect to target
        /// </summary>
        public void ApplyTo(Targetable target, float accuracy) {
            var scaledDamage = damage * accuracy;
            var scaledHeal = heal * accuracy;
            
            if (healFirst) {
                target.ReceiveHeal(scaledHeal);
                target.ReceiveDamage(scaledDamage);
            } else {
                target.ReceiveHeal(scaledHeal);
                target.ReceiveDamage(scaledDamage);
            }
        }
        
        

//        public Buff[] addedBuffs;
//        public Buff[] removedBuffs;
    }

//    public class Buff : ScriptableObject {
//    }
}