using UnityEngine;

namespace Shared.Behaviours.Progressable {
    public abstract class ProgressableBehaviour : MonoBehaviour, IProgressable {
        /// <summary>
        /// Current progress
        /// </summary>
        public abstract float Progress { get; set; }
    }
}