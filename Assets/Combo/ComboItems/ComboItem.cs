using System;
using UnityEngine;

namespace Combo.ComboItems {
    [ExecuteInEditMode]
    public abstract class ComboItem : HitMissItem {
        [Serializable]
        public abstract class Data : ScriptableObject {
            [Range(0, 500)] public float size;
            public abstract Vector2 Position { get; set; }
        }
        
        /// <summary>
        /// Size of the object
        /// </summary>
        [Range(0, 10)] public float size;
#pragma warning disable 0649
        /// <summary>
        /// Accent of marks
        /// </summary>
        [SerializeField] protected Color accent;
#pragma warning restore 0649

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