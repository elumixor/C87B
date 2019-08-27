using Character;
using Meta;
using UnityEngine;

namespace Scenes.FightScene.SpecializedParts {
    [CreateAssetMenu(fileName = "Powered Part", menuName = "Character Parts/Powered Part", order = 2)]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class PoweredPart : CharacterPart {
        [SerializeField] protected float requiredPower;
    }
}