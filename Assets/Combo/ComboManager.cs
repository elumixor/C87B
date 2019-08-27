using System;
using Combo.DataContainers;
using Combo.Items.Button;
using Combo.Items.Slider;
using Shared;
using Shared.PropertyDrawers;
using UnityEngine;
using UnityEngine.UI;

namespace Combo {
    /// <summary>
    /// Combo Manager is responsible for displaying and handling <see cref="Combo"/>
    /// </summary>
    [RequireComponent(typeof(Canvas)), RequireComponent(typeof(Image))]
    public class ComboManager : MonoBehaviour {
        /// <summary>
        /// Execution canvas
        /// </summary>
        [Required, SerializeField]
        private Canvas executionCanvas;

        /// <summary>
        /// Reference to background component
        /// </summary>
        [Required, SerializeField]
        private Image background;

        /// <summary>
        /// Prefab of <see cref="ComboSlider"/>
        /// </summary>
        [Required, SerializeField]
        private ComboSlider sliderPrefab;

        /// <summary>
        /// Prefab of <see cref="ComboButton"/>
        /// </summary>
        [Required, SerializeField]
        private ComboButton buttonPrefab;

        /// <summary>
        /// Assigns execution canvas on awake
        /// </summary>
        private void Reset() {
            executionCanvas = GetComponent<Canvas>();
            background = GetComponent<Image>();
            background.SetColorAlpha(0f);
        }

        /// <summary>
        /// Instantiates 
        /// </summary>
        public void BeginCombo(ComboData comboData, Action<float> onSuccess, Action onFail) {
            background.SetColorAlpha(1f); // todo: gradually / animate
            var comboInstance = Combo.Create(sliderPrefab, buttonPrefab, comboData, executionCanvas.transform);
            comboInstance.OnHit += accuracy => {
                background.SetColorAlpha(0f);
                onSuccess(accuracy);
            };
            comboInstance.OnMissed += () => {
                background.SetColorAlpha(0f);
                onFail();
            };
        }
    }
}