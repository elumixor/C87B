using Character;
using Character.HP;
using Character.Trait;

namespace Scenes.FightScene.CharacterParts {
    public class ShieldGenerator : CharacterPart {
        [Trait] private HitPoints HP => new HitPoints(HPType.Shields, 300);
    }
}