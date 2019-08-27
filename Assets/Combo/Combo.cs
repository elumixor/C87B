using Combo.DataContainers;
using Combo.Frame;
using Combo.Items.Button;
using Combo.Items.Slider;
using Shared.Behaviours;
using UnityEngine;

namespace Combo {
    /// <summary>
    /// Combo class represents collection of combo frames, encapsulates execution logic
    /// </summary>
    public class Combo : HitMissItem {
        /// <summary>
        /// Data of combo
        /// </summary>
        public ComboData comboData;

        /// <summary>
        /// Prefab of <see cref="ComboSlider"/>
        /// </summary>
        public ComboSlider sliderPrefab;

        /// <summary>
        /// Prefab of <see cref="ComboButton"/>
        /// </summary>
        public ComboButton buttonPrefab;
        
        private int currentFrameIndex;
        private float accumulatedAccuracy;
        private ComboFrame currentFrame;

        private void Start() => NextFrame();

        private void NextFrame() {
            if (currentFrameIndex == comboData.Length) ItemHit(accumulatedAccuracy / comboData.Length);
            else {
                currentFrame = ComboFrame.Create(buttonPrefab, sliderPrefab, comboData[currentFrameIndex++], transform);
                currentFrame.OnHit += accuracy => {
                    accumulatedAccuracy += accuracy;
                    NextFrame();
                };
                currentFrame.OnMissed += () => {
                    ItemMissed();
                    Destroy(gameObject);
                };
            }
        }
        
        /// <summary>
        /// Factory method for combo frame 
        /// </summary>
        public static Combo Create(ComboSlider sliderPrefab, ComboButton buttonPrefab, ComboData comboData, Transform parent) {
            var instance = new GameObject("Combo", typeof(RectTransform), typeof(Combo));
            instance.transform.SetParent(parent);

            var comboComponent = instance.GetComponent<Combo>();
            comboComponent.sliderPrefab = sliderPrefab;
            comboComponent.buttonPrefab = buttonPrefab;
            comboComponent.comboData = comboData;

            return comboComponent;
        }
    }
}