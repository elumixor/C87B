using System;
using System.Linq;
using Combo.ComboFrame.FrameTypes;
using Combo.ComboItems.ComboButton;
using Combo.ComboItems.ComboSlider;
using UnityEditor;
using UnityEngine;
using ComboItem = Combo.ComboItems.ComboItem;

namespace Combo.ComboFrame {
    /// <summary>
    /// Encapsulates basic single combo frame execution logic where items can be executed in free order
    /// </summary>
    public abstract class ComboFrame : HitMissItem{
        /// <summary>
        /// ComboFrame data
        /// </summary>
        public Domain.ComboFrame frame;
        
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
        /// Time, elapsed since frame creation. When it's bigger than <see cref="Scenes.FightScene.Combo.Domain.ComboFrame.executionTime"/> frame is
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

        protected virtual void Awake() {
            items = frame.items.Select<ComboItem.Data, ComboItem>(item => {
                if (item is ComboButton.Data button) {
                    var instance = (ComboButton) PrefabUtility.InstantiatePrefab(buttonPrefab, transform);
                    instance.transform.localPosition = button.Position;
                    var rect = instance.GetComponent<RectTransform>().rect;
                    instance.size = Mathf.Max(rect.width, rect.height) / button.size;
                    return instance;
                } else {
                    var slider = (ComboSlider.Data) item;
                    var instance = (ComboSlider) PrefabUtility.InstantiatePrefab(sliderPrefab, transform);
                    instance.path = slider.path;
                    instance.size = slider.size;
                    return instance;
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
                if (elapsed >= frame.executionTime) ItemMissed();
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
        
        private static Type OfType(Domain.ComboFrame.ComboFrameType frameType) {
            switch (frameType) {
                case Domain.ComboFrame.ComboFrameType.Ordered:
                    return typeof(OrderedFrame);
                case Domain.ComboFrame.ComboFrameType.SimultaneousButton:
                    return typeof(SimultaneousButtonFrame);
                case Domain.ComboFrame.ComboFrameType.SimultaneousSlider:
                    return typeof(SimultaneousSliderFrame);
                default:
                    return typeof(UnorderedFrame);
            }
        }
        public static ComboFrame Create(ComboButton buttonPrefab, ComboSlider sliderPrefab, Domain.ComboFrame comboFrame, Transform parent) {
            var frameType = OfType(comboFrame.frameType);
            
            var instance = new GameObject("Combo Frame", typeof(RectTransform), frameType);
            
            instance.transform.SetParent(parent);
            
            var comboFrameComponent = instance.GetComponent<UnorderedFrame>();

            comboFrameComponent.frame = comboFrame;
            comboFrameComponent.sliderPrefab = sliderPrefab;
            comboFrameComponent.buttonPrefab = buttonPrefab;

            return comboFrameComponent;
        }
    }
}