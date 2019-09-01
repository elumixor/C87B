using UnityEngine;
using UnityEngine.EventSystems;

namespace Combo.Items.Slider.Arrow {
    [RequireComponent(typeof(Animator))]
    public class SliderArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        /// <summary>
        /// Animator component reference
        /// </summary>
        [SerializeField] public Animator animator;

        private static readonly int IsDragging = Animator.StringToHash("IsDragging");

        /// <summary>
        /// Assign components on script change
        /// </summary>
        private void OnValidate() {
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