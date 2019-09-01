using System;
using System.Collections.Generic;
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
        protected List<ComboItem> items;

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
//                    instance.size = Mathf.Max(rect.width, rect.height) / buttonData.Size;
                    return (ComboItem) instance;
                } else {
                    var sliderData = (ComboSliderData) item;
                    var instance = (ComboSlider) PrefabUtility.InstantiatePrefab(sliderPrefab, transform);
//                    instance.path = sliderData.path;
                    return (ComboItem) instance;
                }
            }).ToList();

            for (var i = 0; i < items.Count; i++) {
                var item = items[i];
                var index = i;
                item.Hit += (sender, accuracy) => HandleHit(item, accuracy, index);
                item.Missed += (sender, args) => shouldDestroy = true;
            }
        }

        protected virtual void Update() {
            if (shouldDestroy) {
                if (items.All(i => i == null)) Destroy(gameObject);
            } else {
                elapsed += Time.deltaTime;
                if (elapsed >= frameData.executionTime) OnMissed();
            }
        }

        public override bool OnMissed() {
            if (!base.OnMissed()) return false;
            foreach (var item in items) item.OnMissed();

            return shouldDestroy = true;
        }

        protected virtual void HandleHit(ComboItem item, float accuracy, int index) {
            accumulatedAccuracy += accuracy;
            items.Remove(item);
            if (items.Count == 0) OnHit(accumulatedAccuracy / items.Count);
        }

        /// <summary>
        /// Factory method to instantiate <see cref="ComboFrame"/>
        /// </summary>
        public static ComboFrame Instantiate(ComboButton button, ComboSlider slider, ComboFrameData data, Transform parent) {
            Type ofType(ComboFrameType type) {
                switch (type) {
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

            var frameType = ofType(data.frameType);

            var instance = new GameObject("Combo Frame", typeof(RectTransform), frameType);

            instance.transform.SetParent(parent);

            var comboFrameComponent = (ComboFrame) instance.GetComponent(frameType);

            comboFrameComponent.frameData = data;
            comboFrameComponent.sliderPrefab = slider;
            comboFrameComponent.buttonPrefab = button;

            // should be asserted somewhere before this
//            var (_, comboFrame, _, comboButtons, comboSliders) = Helper.Directories;
//            if (!data.IsSavedFile()) data.SaveNew(comboFrame, "Combo Frame");
//
//            foreach (var comboItemData in data.items.Where(item => !item.IsSavedFile())) {
//                if (comboItemData is ComboButtonData)
//                    comboItemData.SaveNew(comboButtons, "Combo Button");
//                else
//                    comboItemData.SaveNew(comboSliders, "Combo Slider");
//            }

            return comboFrameComponent;
        }
    }
}