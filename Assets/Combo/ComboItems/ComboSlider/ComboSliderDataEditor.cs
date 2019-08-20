using UnityEditor;
using UnityEngine;

namespace Combo.ComboItems.ComboSlider {
    [CustomEditor(typeof(ComboSlider.Data))]
    public class ComboSliderDataEditor : Editor {
        private SerializedProperty spacing;
        private SerializedProperty resolution;
        private SerializedProperty startPosition;
        private SerializedProperty endPosition;
        private SerializedProperty startTangent;
        private SerializedProperty endTangent;
        private void OnEnable() {
            var path = serializedObject.FindProperty("path");

            spacing = path.FindPropertyRelative("spacing");
            resolution = path.FindPropertyRelative("resolution");

            var points = path.FindPropertyRelative("points");

            startPosition = points.GetArrayElementAtIndex(0);
            endPosition = points.GetArrayElementAtIndex(3);
            startTangent = points.GetArrayElementAtIndex(1);
            endTangent = points.GetArrayElementAtIndex(2);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(startPosition, new GUIContent("Start Position"));
            EditorGUILayout.PropertyField(endPosition, new GUIContent("End Position"));
            EditorGUILayout.PropertyField(startTangent, new GUIContent("Start Tangent"));
            EditorGUILayout.PropertyField(endTangent, new GUIContent("End Tangent"));
            EditorGUILayout.PropertyField(spacing);
            EditorGUILayout.PropertyField(resolution);
            serializedObject.ApplyModifiedProperties();
        }
    }
}