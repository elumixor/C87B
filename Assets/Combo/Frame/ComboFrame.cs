using System;
using System.Linq;
using Combo.DataContainers;
using Combo.Frame.Types;
using Combo.Items.Button;
using Combo.Items.Slider;
using Shared;
using Shared.Behaviours;
using UnityEditor;
using UnityEngine;
using ComboItem = Combo.Items.ComboItem;

namespace Combo.Frame {
    /// <summary>
    /// Encapsulates basic single combo frame execution logic where items can be executed in free order
    /// </summary>
    public abstract class ComboFrame : HitMissItem {
        /// <summary>
        /// ComboFrame data
        /// </summary>
        public ComboFrameData frameData;

        /// <summary>
        /// Prefab of <see cref="ComboSlider"/>
        /// </summary>
        public ComboSlider sliderPrefab;

        /// <summary>
        /// Prefab of <see cref="ComboButton"/>
        /// </summary>
        public ComboButton buttonPrefab;

        /// <summary>
        /// References to created items components
        /// </summary>
        protected ComboItem[] items;

        /// <summary>
        /// Time, elapsed since frame creation. When it's bigger than <see cref="ComboFrameData.executionTime"/> frame is
        /// destroyed
        /// </summary>
        private float elapsed;

        /// <summary>
        /// Total accuracy across all <see cref="items"/>
        /// </summary>
        private float accumulatedAccuracy;

        /// <summary>
        /// Count of already hit items
        /// </summary>
        protected int hitCount;

        /// <summary>
        /// Flag to check for destroyed children, to determine if self should be destroyed
        /// </summary>
        private bool shouldDestroy;

        protected virtual void Start() {
            items = frameData.items.Select(item => {
                if (item is ComboButtonData buttonData) {
                    var instance = (ComboButton) PrefabUtility.InstantiatePrefab(buttonPrefab, transform);
                    instance.transform.localPosition = buttonData.Position;
                    var rect = instance.GetComponent<RectTransform>().rect;
                    instance.size = Mathf.Max(rect.width, rect.height) / buttonData.Size;
                    return (ComboItem) instance;
                } else {
                    var sliderData = (ComboSliderData) item;
                    var instance = (ComboSlider) PrefabUtility.InstantiatePrefab(sliderPrefab, transform);
                    instance.path = sliderData.path;
                    return (ComboItem) instance;
                }
            }).ToArray();

            for (var index = 0; index < items.Length; index++) {
                var item = items[index];
                var index1 = index;
                item.OnHit += accuracy => HandleHit(accuracy, index1);
                item.OnMissed += () => shouldDestroy = true;
            }
        }

        protected virtual void Update() {
            if (shouldDestroy) {
                if (items.All(i => i == null)) Destroy(gameObject);
            } else {
                elapsed += Time.deltaTime;
                if (elapsed >= frameData.executionTime) ItemMissed();
            }
        }

        public override void ItemMissed() {
            base.ItemMissed();

            foreach (var item in items) {
                item.ItemMissed();
            }

            shouldDestroy = true;
        }

        protected virtual void HandleHit(float accuracy, int index) {
            accumulatedAccuracy += accuracy;
            if (++hitCount == items.Length) ItemHit(accumulatedAccuracy / items.Length);
        }

        private static Type OfType(ComboFrameType frameType) {
            switch (frameType) {
                case ComboFrameType.Ordered:
                    return typeof(OrderedFrame);
                case ComboFrameType.SimultaneousButton:
                    return typeof(SimultaneousButtonFrame);
                case ComboFrameType.SimultaneousSlider:
                    return typeof(SimultaneousSliderFrame);
                default:
                    return typeof(UnorderedFrame);
            }
        }

        public static ComboFrame Create(ComboButton buttonPrefab, ComboSlider sliderPrefab, ComboFrameData comboFrameData,
            Transform parent) {
            
            var frameType = OfType(comboFrameData.frameType);

            var instance = new GameObject("Combo Frame", typeof(RectTransform));

            instance.transform.SetParent(parent);

            var comboFrameComponent = (ComboFrame)instance.AddComponent(frameType);

            comboFrameComponent.frameData = comboFrameData;
            comboFrameComponent.sliderPrefab = sliderPrefab;
            comboFrameComponent.buttonPrefab = buttonPrefab;

            var (_, comboFrame, _, comboButtons, comboSliders) = Helper.Directories;

            if (!comboFrameData.IsSavedFile()) comboFrameData.SaveNew(comboFrame, "Combo Frame");

            foreach (var comboItemData in comboFrameData.items.Where(item => !item.IsSavedFile())) {
                if (comboItemData is ComboButtonData)
                    comboItemData.SaveNew(comboButtons, "Combo Button");
                else
                    comboItemData.SaveNew(comboSliders, "Combo Slider");
            }

            return comboFrameComponent;
        }
    }
}