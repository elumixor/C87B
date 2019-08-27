using System;
using Shared.Behaviours;
using UnityEngine;

namespace Combo.Items {
    /// <summary>
    /// Combo item is an actual <see cref="MonoBehaviour"/>
    /// </summary>
    [ExecuteInEditMode]
    public abstract class ComboItem : HitMissItem {
        /// <summary>
        /// Accent of marks
        /// </summary>
        [SerializeField] protected Color accent;

        /// <summary>
        /// Class to encapsulate common animation logic for when some part of combo item should animate hit or miss
        /// </summary>
        [Serializable]
        public class AnimationDetail {
            /// <summary>
            /// Animator component
            /// </summary>
            [SerializeField] private Animator animator;

            #region String Hashes

            private static readonly int HitHash = Animator.StringToHash("Hit");
            private static readonly int MissHash = Animator.StringToHash("Miss");

            #endregion

            /// <summary>
            /// Create new instance or animation detail
            /// </summary>
            /// <param name="animator">Animator component of <see cref="GameObject"/></param>
            public AnimationDetail(Animator animator) {
                this.animator = animator;
            }

            /// <summary>
            /// Play animation on hit
            /// </summary>
            public void Hit() {
                animator.SetTrigger(HitHash);
            }

            /// <summary>
            /// Play animation on hit
            /// </summary>
            public void Miss() {
                animator.SetTrigger(MissHash);
            }
        }
    }
}