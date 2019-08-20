using UnityEditor;
using UnityEngine;

namespace Shared.Behaviours.Progressable.CubicProgressable {
    [CustomEditor(typeof(CubicProgressable))]
    public class CubicProgressableEditor : Editor {
        private SerializedProperty scale;
        private Transform transform;

        private void OnEnable() {
            scale = serializedObject.FindProperty("maxScale");
            transform = ((CubicProgressable) target).transform;
        }

        private void OnSceneGUI() {
            scale.vector3Value = Handles.ScaleHandle(scale.vector3Value, transform.localPosition, Quaternion.identity, 1);
            serializedObject.ApplyModifiedProperties();
        }
    }
}