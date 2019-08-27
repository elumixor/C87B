using System.Linq;
using Character.Ability.Effects;
using Character.Targeting;
using Scenes.FightScene.SpecializedParts;
using Unity.Collections.LowLevel.Unsafe;

namespace Scenes.FightScene.Characters {
    /// <summary>
    /// Ship is a character with ship parts
    /// </summary>
    public class Ship : Character.Character {
        public float ProvidedEnergy => parts
            .OfType<EnergyGenerator>()
            .Select(generator => generator.GeneratedEnergy)
            .Sum();

        public override void ReceiveDamage(float damage) {
            throw new System.NotImplementedException();
        }

        public override void ReceiveHeal(float heal) {
            throw new System.NotImplementedException();
        }
    }
}