using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Shared.Path.PathDrag {
    [CustomEditor(typeof(PathDraggable))]
    public class PathDraggableEditor : Editor {
        private PathDraggable pathDraggable;
        private bool showEvenlySpacedPoints;
        private List<Vector2> evenlySpacedPoints;

        private void OnEnable() {
            pathDraggable = (PathDraggable) target;
            evenlySpacedPoints = pathDraggable.path.EvenlySpacedPoints();
        }

        public override void OnInspectorGUI() {
            var newPath = pathDraggable.path.OnInspectorGUI();
            if (newPath != pathDraggable.path) evenlySpacedPoints = newPath.EvenlySpacedPoints();
            pathDraggable.path = newPath;

            EditorUtility.SetDirty(target);
        }

        private void OnSceneGUI() {
            var newPath = pathDraggable.path.OnSceneGUI();
            if (newPath != pathDraggable.path) evenlySpacedPoints = newPath.EvenlySpacedPoints();
            pathDraggable.path = newPath;

            var e = Event.current;
            if (e.alt) {
                Debug.Log("drawing");
                pathDraggable.path.DrawEvenlySpacedPoints(evenlySpacedPoints);
            }

            EditorUtility.SetDirty(target);
        }
    }
}