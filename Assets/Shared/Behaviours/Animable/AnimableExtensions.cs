using UnityEngine;

namespace Shared.Behaviours.Animable {
    public static class AnimableExtensions {
        #region Animator triggers names
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int MissHash = Animator.StringToHash("Miss");
        #endregion

        public static void AnimateHit(this IAnimable animable) => animable.Animator.SetTrigger(HitHash);
        public static void AnimateMiss(this IAnimable animable) => animable.Animator.SetTrigger(MissHash);
    }
}