using UnityEngine;
using UnityEngine.EventSystems;

namespace Combo.Items.Slider.Arrow {
    [RequireComponent(typeof(Animator))]
    public class SliderArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        /// <summary>
        /// Animator component reference
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// Animation Detail for this segment
        /// </summary>
        public ComboItem.AnimationDetail animationDetail;

        private static readonly int IsDragging = Animator.StringToHash("IsDragging");

        /// <summary>
        /// Assign components on script change
        /// </summary>
        private void OnValidate() {
            animator = GetComponent<Animator>();
            animationDetail = new ComboItem.AnimationDetail(animator);
        }

        public void OnPointerDown(PointerEventData eventData) {
            animator.SetBool(IsDragging, true);
        }
        public void OnPointerUp(PointerEventData eventData) {
            animator.SetBool(IsDragging, false);
        }
    }
}