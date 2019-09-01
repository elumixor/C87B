using System;
using UnityEditor;

namespace Shared.Path.PathDrag {
    [CustomEditor(typeof(PathDraggable))]
    public class PathDraggableEditor : Editor {
        private PathDraggable pathDraggable;

        private void OnEnable() {
            pathDraggable = (PathDraggable) target;
        }

        public override void OnInspectorGUI() {
            pathDraggable.path = pathDraggable.path.OnInspectorGUI();
            EditorUtility.SetDirty(target);
        }

        private void OnSceneGUI() {
            pathDraggable.path = pathDraggable.path.OnSceneGUI();
            EditorUtility.SetDirty(target);
        }
    }
}