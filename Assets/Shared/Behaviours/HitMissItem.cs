using UnityEngine;

namespace Shared.Behaviours {
    public abstract class HitMissItem : MonoBehaviour {
        public delegate void OnHitHandler(float accuracy);

        public delegate void OnMissedHandler();

        /// <summary>
        /// Event is raised when item is executed successfully with accuracy
        /// </summary>
        public event OnHitHandler OnHit;

        /// <summary>
        /// Event is raised when item failed to be executed accurately enough
        /// </summary>
        public event OnMissedHandler OnMissed;

        /// <summary>
        /// Call this on item execution success
        /// </summary>
        public virtual void ItemHit(float accuracy = 1f) {
            OnHit?.Invoke(accuracy);
        }

        /// <summary>
        /// Call this on item execution failure
        /// </summary>
        public virtual void ItemMissed() {
            OnMissed?.Invoke();
        }
    }
}