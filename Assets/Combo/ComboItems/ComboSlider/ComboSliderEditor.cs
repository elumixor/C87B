using UnityEditor;
using UnityEngine;

namespace Combo.ComboItems.ComboSlider {
    [CustomEditor(typeof(ComboSlider))]
    public class ComboSliderEditor : Editor {
        private ComboSlider slider;

        private void OnEnable() {
            slider = (ComboSlider) target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = slider.CanCreateSegments;

            if (GUILayout.Button("Create Segments")) {
                slider.InstantiateSegments();

                foreach (Transform segment in slider.transform) {
                    Undo.RegisterCreatedObjectUndo(segment.gameObject, "Create segments");
                }
            }
        }
    }
}