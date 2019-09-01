using System;
using System.Collections.Generic;
using Combo.DataContainers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combo.Items.Button {
    [SelectionBase, DisallowMultipleComponent]
    public class ComboButton : ComboItem<ComboButtonData>, IPointerDownHandler {
        /// <summary>
        /// Minimum accuracy required to succeed with this combo item
        /// </summary>
        [SerializeField]
        private float thresholdAccuracy;

        /// <summary>
        /// Checks accuracy on pointer down and decides if item succeeded with accuracy or failed
        /// </summary>
        public void OnPointerDown(PointerEventData eventData) {
            var settings = Settings;
            if (settings == null) return;

            var accuracy = (eventData.pressPosition - settings.Position).magnitude / settings.Size;
            if (accuracy > thresholdAccuracy) OnHit(accuracy);
            else OnMissed();
        }

        /// <summary>
        /// Update local position and scale on settings changed
        /// </summary>
        public override void OnSettingsChanged() {
            var settings = Settings;
            if (settings == null) return;

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = settings.Position;
            rectTransform.localScale = new Vector2(settings.Size, settings.Size);
        }
    }
}