using Shared.PropertyDrawers;
using UnityEngine;

namespace Shared.Behaviours {
    /// <summary>
    /// Makes component's rotation always look at <see cref="target"/>
    /// </summary>
    [ExecuteInEditMode]
    public class FaceTransform : MonoBehaviour {
#pragma warning disable 649
        /// <summary>
        /// Transform to face
        /// </summary>
        [Required] public Transform target;
#pragma warning restore 649

        /// <summary>
        /// Updates rotation to constantly look at target, also executes in edit mode
        /// </summary>
        private void Update() {
            if (target) transform.LookAt(target.position);
        }
    }
}