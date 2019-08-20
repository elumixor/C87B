using Combo.ComboItems.ComboButton;
using Combo.ComboItems.ComboSlider;
using UnityEngine;
using CFrame = Combo.ComboFrame.ComboFrame;

namespace Combo {
    /// <summary>
    /// Combo class represents collection of combo frames, encapsulates execution logic
    /// </summary>
    public class Combo : HitMissItem {
        public Domain.Combo combo;

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
        private CFrame currentFrame;

        private void Awake() {
            NextFrame();
        }

        private void NextFrame() {
            if (currentFrameIndex == combo.Length) ItemHit(accumulatedAccuracy / combo.Length);
            else {
                currentFrame = CFrame.Create(buttonPrefab, sliderPrefab, combo[currentFrameIndex++], transform);
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
        /// Creates new instance of combo frame 
        /// </summary>
        public static Combo Create(ComboSlider sliderPrefab, ComboButton buttonPrefab, Domain.Combo combo, Transform parent) {
            var instance = new GameObject("Combo", typeof(RectTransform), typeof(Combo));
            instance.transform.SetParent(parent);

            var comboComponent = instance.GetComponent<Combo>();
            comboComponent.sliderPrefab = sliderPrefab;
            comboComponent.buttonPrefab = buttonPrefab;
            comboComponent.combo = Domain.Combo.Create(combo);

            return comboComponent;
        }
    }
}