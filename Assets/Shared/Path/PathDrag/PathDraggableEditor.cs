using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace Shared.Path.PathDrag {
    [CustomEditor(typeof(PathDraggable))]
    public class PathDraggableEditor : Editor {
        private PathDraggable pathDraggable;
        private bool showEvenlySpacedPoints;
        private List<Vector2> evenlySpacedPoints;
        private RectTransform canvas;
        public Vector2 offset;

        private void OnEnable() {
            pathDraggable = (PathDraggable) target;

            GeneralExtensions.EnableCanvasOffset(ref canvas, out offset);
            pathDraggable.SetEvenlySpacedPoints(evenlySpacedPoints = pathDraggable.path.EvenlySpacedPoints());
        }

        public override void OnInspectorGUI() {
            GeneralExtensions.OnInspectorCanvasOffset(ref canvas, ref offset);

            var newPath = pathDraggable.path.OnInspectorGUI();

            pathDraggable.accuracyThreshold = EditorGUILayout.Slider("Accuracy threshold", pathDraggable.accuracyThreshold, 0f, 100f);
            pathDraggable.dragDirection =
                (PathDraggable.DragDirection) EditorGUILayout.EnumPopup("Drag direction", pathDraggable.dragDirection);

            pathDraggable.startPointIndex = EditorGUILayout.IntSlider("Start point index", pathDraggable.startPointIndex,
                0, evenlySpacedPoints.Count - 1);

            pathDraggable.UpdatePosition();
            pathDraggable.UpdateRotation();

            if (newPath != pathDraggable.path) pathDraggable.SetEvenlySpacedPoints(evenlySpacedPoints = newPath.EvenlySpacedPoints());
            pathDraggable.path = newPath;

            EditorUtility.SetDirty(target);
        }

        private void OnSceneGUI() {
            Handles.color = Color.white;
            var newPath = pathDraggable.path.OnSceneGUI(offset);
            if (newPath != pathDraggable.path) pathDraggable.SetEvenlySpacedPoints(evenlySpacedPoints = newPath.EvenlySpacedPoints());
            pathDraggable.path = newPath;

            var e = Event.current;
            if (e.alt) pathDraggable.path.DrawEvenlySpacedPoints(evenlySpacedPoints, offset);

            // Bezier's width is relative to screen's zoom, making this not useful
//            if (pathDraggable.accuracyThreshold > 0) {
//                Handles.DrawBezier(pathDraggable.path.StartPosition,
//                    pathDraggable.path.EndPosition,
//                    pathDraggable.path.StartTangent,
//                    pathDraggable.path.EndTangent, new Color(0f, 1f, 0f, 0.16f), null, pathDraggable.accuracyThreshold);
//            }

            Handles.color = new Color(0.74f, 0.02f, 0.3f);

            var start = evenlySpacedPoints[pathDraggable.startPointIndex] + offset;
            Handles.DrawSolidDisc(start, Vector3.forward, 5f);

            if ((pathDraggable.dragDirection == PathDraggable.DragDirection.OnlyBackward ||
                 pathDraggable.dragDirection == PathDraggable.DragDirection.Free)
                && pathDraggable.startPointIndex > 0) {
                var end = evenlySpacedPoints[pathDraggable.startPointIndex - 1] + offset;
                Handles.ArrowHandleCap(0, start, Quaternion.LookRotation(end - start), (end - start).magnitude, EventType.Repaint);
            }

            if ((pathDraggable.dragDirection == PathDraggable.DragDirection.OnlyForward ||
                 pathDraggable.dragDirection == PathDraggable.DragDirection.Free)
                && pathDraggable.startPointIndex < evenlySpacedPoints.Count - 1) {
                var end = evenlySpacedPoints[pathDraggable.startPointIndex + 1] + offset;
                Handles.ArrowHandleCap(0, start, Quaternion.LookRotation(end - start), (end - start).magnitude, EventType.Repaint);
            }

            EditorUtility.SetDirty(target);
        }
    }
}