using UnityEditor;
using UnityEngine;

namespace Shared.Path {
    public static class Path2DExtensions {
        private const float MinSize = 0.001f;

        public static Path2D OnInspectorGUI(this Path2D path, Object recordObject = null) => path.OnInspectorGUI(Vector2.zero, recordObject);
        public static Path2D OnInspectorGUI(this Path2D path, Vector2 offset, Object recordObject = null) {
            path += offset;

            var oldPosition = path.Position;
            var newPosition = EditorGUILayout.Vector2Field("Position", oldPosition);
            if (newPosition != oldPosition) {
                if (recordObject != null) Undo.RecordObject(recordObject, "Change position");
                path.Position = newPosition;
            }

            var oldSize = path.Size;
            var newSize = EditorGUILayout.FloatField("Size", oldSize);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (oldSize != newSize) {
                if (recordObject != null) Undo.RecordObject(recordObject, "Change size");
                path.Size = Mathf.Max(MinSize, newSize);
            }

            using (new EditorGUI.IndentLevelScope()) {
                var oldStartPoint = path.StartPosition;
                var newStartPoint = EditorGUILayout.Vector2Field("Start point", oldStartPoint);
                if (newStartPoint != oldStartPoint) {
                    if (recordObject != null) Undo.RecordObject(recordObject, "Change start point");
                    path.StartPosition = newStartPoint;
                }

                var oldEndPosition = path.EndPosition;
                var newEndPoint = EditorGUILayout.Vector2Field("End point", oldEndPosition);
                if (newEndPoint != oldEndPosition) {
                    if (recordObject != null) Undo.RecordObject(recordObject, "Change end point");
                    path.EndPosition = newEndPoint;
                }

                var oldStartTangent = path.StartTangent;
                var newStartTangent = EditorGUILayout.Vector2Field("Start tangent", oldStartTangent);
                if (newStartTangent != oldStartTangent) {
                    if (recordObject != null)  Undo.RecordObject(recordObject, "Change start tangent");
                    path.StartTangent = newStartTangent;
                }

                var oldEndTangent = path.EndTangent;
                var newEndTangent = EditorGUILayout.Vector2Field("End tangent", oldEndTangent);
                if (newEndTangent != oldEndTangent) {
                    if (recordObject != null) Undo.RecordObject(recordObject, "Change end tangent");
                    path.EndTangent = newEndTangent;
                }
            }

            path -= offset;

            var oldSpacing = path.spacing;
            var newSpacing = EditorGUILayout.Slider("Spacing", oldSpacing, 0.1f, 100f);
            if (newSpacing != oldSpacing) {
                if (recordObject != null) Undo.RecordObject(recordObject, "Change spacing");
                path.spacing = newSpacing;
            }

            return path;
        }
        public static Path2D OnSceneGUI(this Path2D path, Object recordObject = null) => path.OnSceneGUI(Vector2.zero, recordObject);

        public static Path2D OnSceneGUI(this Path2D path, Vector2 offset, Object recordObject = null) {
            path += offset;

            var oldPosition = path.Position;
            var newPosition = (Vector2) Handles.FreeMoveHandle(oldPosition, Quaternion.identity, 10,
                Vector3.one, Handles.CircleHandleCap);
            if (newPosition != oldPosition) {
                if (recordObject != null) Undo.RecordObject(recordObject, "Change path position");
                path.Position = newPosition;
            }


            var oldSize = path.Size * .5f;
            var newSize = Handles.RadiusHandle(Quaternion.identity, newPosition, oldSize);
            if (newSize != oldSize) {
                if (recordObject != null) Undo.RecordObject(recordObject, "Change path size");
                path.Size = Mathf.Max(newSize * 2f, MinSize);
            }


            // Draw and set path control points
            for (var i = 0; i < Path2D.Length; i++) {
                var oldPointPosition = path[i];
                var newPointPosition = (Vector2) Handles.FreeMoveHandle(path[i], Quaternion.identity, 10f,
                    Vector3.one, Handles.CircleHandleCap);
                if (newPointPosition != oldPointPosition) {
                    if (recordObject != null) Undo.RecordObject(recordObject, "Change path control point");
                    path[i] = newPointPosition;
                }
            }

            // Draw connecting bezier curve
            Handles.DrawBezier(
                path.StartPosition,
                path.EndPosition,
                path.StartTangent,
                path.EndTangent,
                Color.green, null, 2f);

            // Draw control lines from positions to tangents
            Handles.DrawLine(path.StartPosition, path.StartTangent);
            Handles.DrawLine(path.EndPosition, path.EndTangent);

            path -= offset;
            return path;
        }
    }
}