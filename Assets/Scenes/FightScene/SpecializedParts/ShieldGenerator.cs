using Character.HP;
using Character.Trait;
using JetBrains.Annotations;
using UnityEngine;

namespace Scenes.FightScene.SpecializedParts {
    [CreateAssetMenu(fileName = "Shield Generator", menuName = "Character Parts/Shield Generator", order = 102)]
    public class ShieldGenerator : PoweredPart {
        [SerializeField] private float shieldValue;
        [Trait, PublicAPI] private HitPoints HP => new HitPoints(HPType.Shields, shieldValue);
    }
}