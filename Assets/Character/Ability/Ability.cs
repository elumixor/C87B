using System;
using Character.Ability.Effects;
using Character.Targeting;
using Combo;
using Combo.DataContainers;
using Meta;
using UnityEngine;

namespace Character.Ability {
    /// <summary>
    /// Active ability that can be executed
    /// Executing specific ability can have multiple effects
    /// </summary>
    [CreateAssetMenu(fileName = "Ability", menuName = "Custom/Ability")]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class Ability : ScriptableObject {
        [SerializeField] private ComboData comboData;
        [SerializeField] private new string name;
        [SerializeField, Range(1, 100)] public int level;

        /// <summary>
        /// Combo, required for ability execution
        /// </summary>
        public ComboData ComboData => comboData;

        /// <summary>
        /// Name of the ability
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Ability level to sort same abilities of 
        /// </summary>
        public int Level => level;
        
        // todo: custom mesh, color, icon, texture, etc
        
        /// <summary>
        /// Struct to workaround unserializable <see cref="Tuple"/> that stores
        /// effect with corresponding target type
        /// </summary>
        [Serializable]
        public struct TargetEffect {
            public Effect effect;
            public TargetType targetType;
            
            public void Deconstruct(out Effect outEffect, out TargetType outTargetType) {
                outEffect = effect;
                outTargetType = targetType;
            }
        }

        /// <summary>
        /// Ability effects
        /// </summary>
        public TargetEffect[] effects;

        public void ApplyTo(Targeter targeter, float accuracy) {
            foreach (var (effect, targetType) in effects) {
                effect.ApplyTo(targeter[targetType], accuracy);
            }
        }
    }
}