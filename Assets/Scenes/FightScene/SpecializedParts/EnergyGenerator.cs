using Character;
using Character.Trait;
using JetBrains.Annotations;
using UnityEngine;

namespace Scenes.FightScene.SpecializedParts {
    /// <summary>
    /// Energy generators provide energy to the ship
    /// </summary>
    [CreateAssetMenu(fileName = "Energy Generator", menuName = "Character Parts/Energy Generator", order = 101)]
    public class EnergyGenerator : CharacterPart {
        /// <summary>
        /// Provided energy points
        /// </summary>
        [SerializeField] private float generatedEnergy;

        [Trait, PublicAPI] public float GeneratedEnergy => generatedEnergy;
    }
}