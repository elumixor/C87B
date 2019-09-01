using System;
using Combo.DataContainers;
using Combo.Items.Button;
using Combo.Items.Slider;
using JetBrains.Annotations;
using Shared;
using Shared.PropertyDrawers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combo {
    /// <summary>
    /// Combo Manager is responsible for displaying and handling <see cref="Combo"/>
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(Image))]
    public class ComboManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
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

        private Combo comboInstance;

        /// <summary>
        /// Instantiates 
        /// </summary>
        public void BeginCombo(ComboData comboData, Action<float> onSuccess, Action onFail) {
            BlocksRaycasts = isCombo = true;
            background.SetColorAlpha(1f); // todo: gradually / animate
            comboInstance = Combo.Instantiate(sliderPrefab, buttonPrefab, comboData, executionCanvas.transform);
            comboInstance.Hit += (sender, accuracy) => {
                background.SetColorAlpha(0f);
                onSuccess(accuracy);
                BlocksRaycasts = isCombo = false;
                pointerDownWillTriggerFail = false;
            };
            comboInstance.Missed += (sender, args) => {
                background.SetColorAlpha(0f);
                onFail();
                BlocksRaycasts = isCombo = false;
                pointerDownWillTriggerFail = false;
            };
        }

        public static bool BlocksRaycasts { get; private set; } = false;

        private bool pointerDownWillTriggerFail;
        private bool isCombo;

        public void OnPointerDown(PointerEventData eventData) {
            if (pointerDownWillTriggerFail && comboInstance != null) comboInstance.OnMissed();
            pointerDownWillTriggerFail = false;
        }
        public void OnPointerUp(PointerEventData eventData) {
            if (isCombo) pointerDownWillTriggerFail = true;
        }
    }
}