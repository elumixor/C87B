using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Shared.Behaviours {
    public abstract class HitMissItem : MonoBehaviour {
        /// <summary>
        /// Event is raised when item is executed successfully with accuracy
        /// </summary>
        public event EventHandler<float> Hit;

        /// <summary>
        /// Event is raised when item failed to be executed accurately enough
        /// </summary>
        public event EventHandler Missed;

        /// <summary>
        /// Tracks if item <see cref="OnHit"/> or <see cref="OnMissed"/> was called to prevent further hits or misses
        /// </summary>
        protected bool Resolved { get; private set; }

        /// <summary>
        /// When item is successfully executed with give accuracy
        /// </summary>
        [PublicAPI]
        public virtual bool OnHit(float accuracy = 1f) {
            if (Resolved) return false;

            Hit?.Invoke(this, accuracy);
            return Resolved = true;
        }

        /// <summary>
        /// When item is either executed not accurately enough, or missed
        /// </summary>
        [PublicAPI]
        public virtual bool OnMissed() {
            if (Resolved) return false;

            Missed?.Invoke(this, EventArgs.Empty);
            return Resolved = true;
        }
    }
}