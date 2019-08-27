using System.Collections.Generic;
using System.Linq;
using Character.Targeting;
using Character.Trait;
using Combo;
using Shared.PropertyDrawers;
using UnityEditor;
using UnityEngine;

namespace Character.AbilityDisplay {
    /// <summary>
    /// Ability displayer is a component for displaying abilities on a character
    /// </summary>
    [RequireComponent(typeof(Targeter))]
    public class AbilityDisplayer : TraitConsumer {
        [SerializeField, Required] private Targeter targeter;
        [SerializeField, Required] private ComboManager comboManager;

        [SerializeField, Required] private AbilityIcon iconPrefab;

        private Transform iconsContainer;

        /// <summary>
        /// List of icons components with corresponding abilities
        /// </summary>
        private List<(AbilityIcon icon, Ability.Ability ability)> icons;

        /// <summary>
        /// Gets distinct abilities of maximum level
        /// </summary>
        private IEnumerable<Ability.Ability> Abilities => GetTraits<Ability.Ability[]>().SelectMany(abs => abs).GroupBy(abs => abs.level)
            .Select(abs => abs.OrderByDescending(a => a.level).First());

        /// <summary>
        /// Assigns targeter reference
        /// </summary>
        protected override void Reset() {
            base.Reset();
            targeter = GetComponent<Targeter>();
        }

        /// <summary>
        /// Updates <see cref="icons"/> to match <see cref="Abilities"/>
        /// </summary>
        protected override void OnProvidersUpdate() {
            var iconsAbilities = icons.Select(icon => icon.ability).ToList();
            var foundAbilities = new List<Ability.Ability>();
            var toBeCreated = new List<Ability.Ability>();

            foreach (var ability in Abilities)
                if (iconsAbilities.Contains(ability))
                    foundAbilities.Add(ability);
                else
                    toBeCreated.Add(ability);

            var toBeRemoved = icons.Where(icon => !foundAbilities.Contains(icon.ability));

            foreach (var iconAbility in toBeRemoved) {
                Destroy(iconAbility.icon);
                icons.Remove(iconAbility);
            }

            foreach (var ability in toBeCreated) icons.Add(InstantiateAbilityIcon(ability));
        }

        /// <summary>
        /// Create abilities from provided abilities 
        /// </summary>
        protected override void Start() {
            base.Start();

            iconsContainer = new GameObject("Ability icons container").transform;
            iconsContainer.SetParent(transform);
            icons = Abilities.Select(InstantiateAbilityIcon).ToList();
        }

        private (AbilityIcon icon, Ability.Ability ability) InstantiateAbilityIcon(Ability.Ability ability) {
            var icon = (AbilityIcon) PrefabUtility.InstantiatePrefab(iconPrefab, iconsContainer);
            icon.ability = ability;
            icon.targeter = targeter;
            icon.comboManager = comboManager;
            return (icon, ability);
        }
    }
}