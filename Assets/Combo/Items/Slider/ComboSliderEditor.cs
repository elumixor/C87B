using System.Collections.Generic;
using System.Linq;
using Combo.DataContainers;
using JetBrains.Annotations;
using Shared;
using Shared.Path;
using UnityEditor;
using UnityEngine;

namespace Combo.Items.Slider {
    [CustomEditor(typeof(ComboSlider))]
    public class ComboSliderEditor : ComboItemEditor<ComboSliderData> {
        private ComboSlider slider;
        private RectTransform canvas;
        public Vector2 offset;

        [CanBeNull]
        private List<Vector2> evenlySpacedPoints;
        protected override void OnEnable() {
            base.OnEnable();
            slider = (ComboSlider) target;

            GeneralExtensions.EnableCanvasOffset(ref canvas, out offset);
            evenlySpacedPoints = slider.Path?.EvenlySpacedPoints();
        }

        public override void OnInspectorGUI() {
            GeneralExtensions.OnInspectorCanvasOffset(ref canvas, ref offset);
            var oldPath = slider.Path;
            base.OnInspectorGUI();
            if (oldPath != slider.Path) evenlySpacedPoints = slider.Path?.EvenlySpacedPoints();
        }
        protected override void OnSceneGUI() {
            Handles.color = Color.white;

            var oldPath = slider.Path;
            base.OnSceneGUI();
            if (oldPath != slider.Path) evenlySpacedPoints = slider.Path?.EvenlySpacedPoints();


            if (evenlySpacedPoints != null && evenlySpacedPoints.Count > 0) {
                Handles.color = Color.cyan;
                slider.arrowSize = Handles.RadiusHandle(Quaternion.identity, evenlySpacedPoints[0] + offset, slider.arrowSize * .5f) * 2f;
                foreach (var point in evenlySpacedPoints) {
                    Handles.color = Color.magenta;
                    slider.segmentSize = Handles.RadiusHandle(Quaternion.identity, point + offset, slider.segmentSize * .5f) * 2;
                }
            }

            var e = Event.current;
            if (e.alt) slider.Path.DrawEvenlySpacedPoints(evenlySpacedPoints, offset);
        }
    }
}