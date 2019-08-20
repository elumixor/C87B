using Character.Ability;
using Shared.PropertyDrawers;
using UnityEngine;

namespace Scenes.FightScene.Data {
    
    /// <summary>
    /// Abilities tree contains info about various abilities and their progress.
    /// </summary>
    public class AbilitiesTree : ScriptableObject {
        [NamedArray("Lvl.")]
        public Ability[] laser;

        [NamedArray("Lvl.")]
        public Ability[] cannon;
    }
}