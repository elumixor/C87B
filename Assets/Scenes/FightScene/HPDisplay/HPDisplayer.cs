using System;
using System.Collections.Generic;
using System.Linq;
using Character.HP;
using Character.Trait;
using Scenes.FightScene.HPDisplay.DisplayData;
using Shared.Behaviours.Progressable;
using Shared.PropertyDrawers;
using UnityEngine;

namespace Scenes.FightScene.HPDisplay {
    /// <summary>
    /// Manages health values and display. Attaching this to a <see cref="Character"/> allows
    /// displaying of health visually. Visual representation varies depending on <see cref="ProgressableBehaviour"/> prefab 
    /// </summary>
    [RequireTrait(typeof(HitPoints))]
    [AddComponentMenu("Character/HP Displayer")]
    public class HPDisplayer : TraitConsumer {
        #region Unity serializable fields

        /// <summary>
        /// Progressable MonoBehaviour prefab to be instantiated
        /// </summary>
        [SerializeField] private ProgressableBehaviour progressablePrefab;

        /// <summary>
        /// Information about health order, colors etc.
        /// </summary>
        [SerializeField, Required] private HPDataManager hpDataManager;

        /// <summary>
        /// Offset position of health bars
        /// </summary>
        [SerializeField] private Vector3 position;

        #endregion

        /// <summary>
        /// Character's health (received from <see cref="TraitConsumer.character"/>)
        /// </summary>
        private Dictionary<HPType, float> hitPoints;

        /// <summary>
        /// <para>
        ///     Value that determines the scale of all health bars
        /// </para>
        /// <para>
        ///     It is set in <see cref="Start"/> and whenever health bars are added or deleted as the maximum
        ///     <see cref="HitPoints.value"/> of all items in <see cref="hitPoints"/> array
        /// </para>
        /// </summary>
        private float maxHealthValue;

        /// <summary>
        /// References to health bars, representing health
        /// </summary>
        private readonly Dictionary<HPType, ProgressableBehaviour> hpBars =
            new Dictionary<HPType, ProgressableBehaviour>();

        /// <summary>
        /// Use container to better orient in hierarchy
        /// </summary>
        private Transform healthBarsContainer;

        /// <summary>
        /// Update health bars on providers update
        /// </summary>
        protected override void OnProvidersUpdate() {
            hitPoints = GetTraits<HitPoints>().ToDictionary(hp => hp.type, hp => hp.value);

            // Check if new health bar types count are the same as previous
            if (hitPoints.Keys.Count != hpBars.Keys.Count) {
                foreach (var healthType in HealthTypeExtensions.HealthTypeValues) {
                    if (hitPoints.ContainsKey(healthType) && !hpBars.ContainsKey(healthType)) {
                        Destroy(hpBars[healthType]);
                        hpBars.Remove(healthType);
                    } else if (!hitPoints.ContainsKey(healthType) && hpBars.ContainsKey(healthType)) {
                        hpBars[healthType] = InstantiateHealthBar(healthType);
                    }
                }
            }

            UpdateHPBars();
        }

        /// <summary>
        /// Checks for hit points providers, gets values and creates health bars 
        /// </summary>
        protected override void Start() {
            base.Start();

            hitPoints = GetTraits<HitPoints>().ToDictionary(hp => hp.type, hp => hp.value);
            maxHealthValue = hitPoints.Values.Max();

            // Create container
            healthBarsContainer = new GameObject(nameof(HPDisplayer)).transform;
            healthBarsContainer.SetParent(transform);
            healthBarsContainer.localPosition = Vector3.zero;

            // Create health bars
            foreach (var hitPointsType in hitPoints.Keys) hpBars[hitPointsType] = InstantiateHealthBar(hitPointsType);

            // Set values
            UpdateHPBars();
        }

        /// <summary>
        /// Instantiates health bar of given type
        /// </summary>
        /// <param name="hpType">Health type</param>
        /// <returns>Reference to progressable</returns>
        private ProgressableBehaviour InstantiateHealthBar(HPType hpType) {
            var instance = Instantiate(progressablePrefab, healthBarsContainer);

            var progressable = instance.GetComponent<ProgressableBehaviour>();

            if (progressable is IHPDataAdjustable hpAdjustable)
                hpAdjustable.SetHealthData(hpDataManager[hpType]);

            return progressable;
        }

        /// <summary>
        /// Update <see cref="hpBars"/> to match <see cref="hitPoints"/> values
        /// </summary>
        private void UpdateHPBars() {
            foreach (var hp in hitPoints) hpBars[hp.Key].Progress = hp.Value / maxHealthValue;
        }
    }
}