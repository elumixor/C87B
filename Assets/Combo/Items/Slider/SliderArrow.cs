using System;
using Shared.Path.PathDrag;
using UnityEngine;

namespace Combo.Items.Slider {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(PathDraggable), typeof(AnimationDraggable))]
    public class SliderArrow : MonoBehaviour {
        public PathDraggable pathDraggable;
        public AnimationDraggable animationDraggable;
        [HideInInspector] public RectTransform arrowTransform;
        [HideInInspector] public SVGImage image;
        public Sprite sprite;

        private void Awake() {
            pathDraggable = GetComponent<PathDraggable>();
            animationDraggable = GetComponent<AnimationDraggable>();
            var arrow = new GameObject("Arrow", typeof(RectTransform), typeof(SVGImage));
            arrowTransform = arrow.GetComponent<RectTransform>();
            arrowTransform.SetParent(transform);
            arrowTransform.localPosition = Vector3.zero;
            arrowTransform.localScale = Vector3.one;
            arrowTransform.localRotation = Quaternion.identity;
            arrowTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);
            arrowTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);

            image = arrow.GetComponent<SVGImage>();
            if (sprite != null) image.sprite = sprite;
        }

        private void Reset() => Awake();
    }
}