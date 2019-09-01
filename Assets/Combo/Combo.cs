using Combo.DataContainers;
using Combo.Frame;
using Combo.Items.Button;
using Combo.Items.Slider;
using Shared.Behaviours;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Combo {
    /// <summary>
    /// Combo class represents collection of combo frames, encapsulates execution logic
    /// </summary>
    public class Combo : HitMissItem {
        /// <summary>
        /// Ordered collection of combo frames data
        /// <seealso cref="ComboFrameData"/>
        /// </summary>
        private ComboData comboData;

        /// <summary>
        /// Prefab of <see cref="ComboSlider"/>
        /// </summary>
        private ComboSlider sliderPrefab;

        /// <summary>
        /// Prefab of <see cref="ComboButton"/>
        /// </summary>
        private ComboButton buttonPrefab;

        private int currentFrameIndex;
        private float accumulatedAccuracy;
        private ComboFrame currentFrame;

        /// <summary>
        /// On start create new frame (not Awake to allow correct data setting in <see cref="Instantiate"/> factory)
        /// </summary>
        private void Start() => NextFrame();

        /// <summary>
        /// Loops through combo frames and displays them sequentially
        /// </summary>
        private void NextFrame() {
            if (currentFrameIndex == comboData.Length) {
                OnHit(accumulatedAccuracy / comboData.Length);
                return;
            }

            currentFrame = ComboFrame.Instantiate(buttonPrefab, sliderPrefab, comboData[currentFrameIndex++], transform);
            currentFrame.Hit += (sender, accuracy) => {
                accumulatedAccuracy += accuracy;
                NextFrame();
            };

            currentFrame.Missed += (sender, args) => OnMissed();
        }

        public override bool OnMissed() {
            if (shouldDestroy) return false;

            shouldDestroy = true;

            base.OnMissed();
            if (currentFrame != null) currentFrame.OnMissed();

            return true;
        }

        private bool shouldDestroy;
        private void Update() {
            if (shouldDestroy && currentFrame == null) Destroy(gameObject);
        }

        /// <summary>
        /// Factory method to instantiate <see cref="Combo"/>
        /// </summary>
        public static Combo Instantiate(ComboSlider slider, ComboButton button, ComboData data, Transform parent) {
            var instance = new GameObject("Combo", typeof(RectTransform), typeof(Combo));
            instance.transform.SetParent(parent);

            var comboComponent = instance.GetComponent<Combo>();
            comboComponent.sliderPrefab = slider;
            comboComponent.buttonPrefab = button;
            comboComponent.comboData = data;

            return comboComponent;
        }
    }
}