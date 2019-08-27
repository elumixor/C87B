using Character;
using Character.HP;
using Character.Trait;
using JetBrains.Annotations;
using UnityEngine;

namespace Scenes.FightScene.SpecializedParts {
    [CreateAssetMenu(fileName = "Hull", menuName = "Character Parts/Hull", order = 201)]
    public class Hull : CharacterPart {
        /// <summary>
        /// Provided armor points
        /// </summary>
        [SerializeField] private float hullPoints;
        
        [Trait, PublicAPI] private HitPoints HP => new HitPoints(HPType.Armor, hullPoints);
        
        // todo: Damage Blocker? Heal blocker? Damage target?
    }
}