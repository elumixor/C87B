using System;
using UnityEditor;
using UnityEngine;

namespace Combo.Items.Button.MarkerButton {
    [CustomEditor(typeof(MarkerButton))]
    public class MarkerButtonEditor : Editor {
        private MarkerButton button;
//        private SerializedProperty markerContainer;
        private SerializedProperty markerImage;
        private SerializedProperty markerCount;

        private void OnEnable() {
            button = (MarkerButton) target;
//            markerImage = serializedObject.FindProperty("markerImage");
//            markerContainer = serializedObject.FindProperty("markerImage");
//            markerCount = serializedObject.FindProperty("markerContainer");
        }
//
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

//            EditorGUILayout.ObjectField(markerImage);
//            var hasImage = markerImage.objectReferenceValue != null;
//            GUI.enabled = hasImage;
//            if (GUILayout.Button("Reset marks")) {
//                button.ResetMarkers();
//            }
//
//            GUI.enabled = true;
//            if (!hasImage) EditorGUILayout.HelpBox("Assign Mark Image to create marks", MessageType.Info);
//

            //            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Find Markers")) {
                button.FindMarkers();
            }

            if (GUILayout.Button("Create Markers")) {
                button.CreateMarkers();
            }
        }
    }
}