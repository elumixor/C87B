using System;
using Combo.ComboItems.ComboButton;
using Combo.ComboItems.ComboSlider;
using UnityEngine;
using UnityEngine.UI;

namespace Combo {
    /// <summary>
    /// Combo Manager is responsible for displaying and handling <see cref="Combo"/>
    /// </summary>
    public class ComboManager : MonoBehaviour {
        /// <summary>
        /// Execution canvas
        /// </summary>
        public Canvas executionCanvas;

        public Image background;
        
        /// <summary>
        /// Prefab of <see cref="ComboSlider"/>
        /// </summary>
        public ComboSlider sliderPrefab;

        /// <summary>
        /// Prefab of <see cref="ComboButton"/>
        /// </summary>
        public ComboButton buttonPrefab;
        
        /// <summary>
        /// Instantiates 
        /// </summary>
        public void BeginCombo(Domain.Combo combo, Action<float> onSuccess, Action onFail) {
            var backgroundInstance = Instantiate(background, executionCanvas.transform);
            var comboInstance = Combo.Create(sliderPrefab, buttonPrefab, combo, executionCanvas.transform);
            comboInstance.OnHit += accuracy => {
                Destroy(backgroundInstance.gameObject);
                onSuccess(accuracy);
            };
            comboInstance.OnMissed += () => {
                Destroy(backgroundInstance.gameObject);
                onFail();
            };
        }
    }
}