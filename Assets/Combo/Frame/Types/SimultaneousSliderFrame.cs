using System.Collections.Generic;
using System.Linq;
using Combo.Items.Slider;
using UnityEngine;

namespace Combo.Frame.Types {
    public class SimultaneousSliderFrame : SimultaneousFrame {
        private IEnumerable<ComboSlider> sliders;

        protected override void Start() {
            base.Start();
            sliders = items.Select(i => (ComboSlider) i);

            // Let's subscribe to sliders' events
            foreach (var slider in sliders) {
//                slider.PathDrag.OnDragStarted += (index, total) => HandleStart();
            }
        }

        private float firstStartTime;
        private int startedCount;

        /// <summary>
        /// Handler for when user starts dragging the slider
        /// </summary>
        private void HandleStart() {
            if (startedCount++ == 0) firstStartTime = Time.time;
            else if (Time.time > firstStartTime + simultaneousToleranceTime) OnMissed();
        }

        protected override void Update() {
            base.Update();

            // we need to check that all items are started simultaneously
            if (startedCount > 0 && Time.time > firstStartTime + simultaneousToleranceTime && startedCount < items.Count) OnMissed();
        }
    }
}