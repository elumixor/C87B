using Combo.Items;
using UnityEngine;

namespace Combo.Frame.Types {
    public abstract class SimultaneousFrame : ComboFrame {
        /// <summary>
        /// Time of first item hit
        /// </summary>
        private float firstHitTime;
        /// <summary>
        /// Time since <see cref="firstHitTime"/>, in which other hits are treated as simultaneous.
        /// If time of item hit is greater than <c>firstHitTime + simultaneousToleranceTime</c> then hit events are treated
        /// as separate 
        /// </summary>
        [SerializeField] protected float simultaneousToleranceTime = .1f;
        /// <summary>
        /// Handler for when slider has completed
        /// </summary>
        protected override void HandleHit(ComboItem item, float accuracy, int index) {
            if (hitCount == 0) firstHitTime = Time.time;
            else {
                if (Time.time > firstHitTime + simultaneousToleranceTime) OnMissed();
                else base.HandleHit(item, accuracy, index);
            }   
        }
        protected override void Update() {
            base.Update();
            
            // we need to check that all items are finished simultaneously
            if (hitCount > 0 && Time.time > firstHitTime + simultaneousToleranceTime && hitCount < items.Count) OnMissed();
        }
    }
}