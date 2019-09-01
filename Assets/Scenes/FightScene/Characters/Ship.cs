using System.Linq;
using Character.Ability.Effects;
using Character.Targeting;
using Scenes.FightScene.SpecializedParts;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

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
            Debug.Log($"{name} received damage of: {damage}");
        }

        public override void ReceiveHeal(float heal) {
            Debug.Log($"{name} received heal of: {heal}");
        }
    }
}