using System;
using Shared.Path.PathDrag;
using UnityEngine;

namespace Combo.Items.Slider {
    [RequireComponent(typeof(RectTransform), typeof(SVGImage))]
    [RequireComponent(typeof(PathDraggable), typeof(AnimationDraggable))]
    public class SliderArrow : MonoBehaviour {
        public PathDraggable pathDraggable;
        public AnimationDraggable animationDraggable;
        public SVGImage image;

        private void Reset() {
            pathDraggable = GetComponent<PathDraggable>();
            animationDraggable = GetComponent<AnimationDraggable>();
            image = GetComponent<SVGImage>();
        }
    }
}