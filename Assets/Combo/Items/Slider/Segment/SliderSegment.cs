using UnityEngine;

namespace Combo.Items.Slider.Segment {
    [ExecuteInEditMode]
    [RequireComponent(typeof(SVGImage), typeof(Animator))]
    public class SliderSegment : MonoBehaviour {
        /// <summary>
        /// Segment opacity
        /// </summary>
        [Range(0, 1)] public float opacity = 1f;
        /// <summary>
        /// Segment maximum size (radius)
        /// </summary>
        [Range(0, 2)] public float maxSize = 1f;
        /// <summary>
        ///  Segment size (radius): 1f (maximum) corresponds to <see cref="maxSize"/>
        /// </summary>
        [Range(0, 2)] public float size = 1f;
        /// <summary>
        /// Accent color
        /// </summary>
        public Color accent = Color.white;
        /// <summary>
        /// SVGImage reference
        /// </summary>
        [SerializeField, HideInInspector] private SVGImage image;
        /// <summary>
        /// Animator component reference
        /// </summary>
        [SerializeField] public Animator animator;

        /// <summary>
        /// Assign components on script change
        /// </summary>
        private void OnValidate() {
            image = GetComponent<SVGImage>();
            animator = GetComponent<Animator>();
        }
        /// <summary>
        /// Reflect script variables change
        /// </summary>
        private void Update() {
            transform.localScale = Vector3.one * (size * maxSize);
            var color = accent;
            color.a = opacity;
            image.color = color;
        }
    }
}