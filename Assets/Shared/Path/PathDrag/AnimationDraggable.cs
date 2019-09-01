using Shared.Behaviours.Animable;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shared.Path.PathDrag {
    [RequireComponent(typeof(Animator))]
    public class AnimationDraggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IAnimable {
        /// <summary>
        /// Animator component reference
        /// </summary>
        [SerializeField]
        private Animator animator;
        public Animator Animator => animator;

        private static readonly int IsDragging = Animator.StringToHash("IsDragging");

        /// <summary>
        /// Assign animator component
        /// </summary>
        private void Reset() {
            animator = GetComponent<Animator>();
        }

        public void OnPointerDown(PointerEventData eventData) {
            animator.SetBool(IsDragging, true);
        }
        public void OnPointerUp(PointerEventData eventData) {
            animator.SetBool(IsDragging, false);
        }
    }
}