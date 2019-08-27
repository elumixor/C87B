using Combo.DataContainers;
using Shared.Path;
using UnityEditor;
using UnityEngine;

namespace Combo.Items.Editors {
    [CustomEditor(typeof(ComboSliderData))]
    public class ComboSliderDataEditor : ComboItemDataEditor<ComboSliderData> {
        protected override void OnEnable() {
            base.OnEnable();
            if (itemData.path == null) itemData.path = new Path2D();
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var oldStartPoint = itemData.path.StartPosition;
            var newStartPoint = EditorGUILayout.Vector2Field("Start point", oldStartPoint);
            if (newStartPoint != oldStartPoint) {
                Undo.RecordObject(itemData, "Change start point");
                itemData.path.StartPosition = newStartPoint;
            }

            var oldEndPosition = itemData.path.EndPosition;
            var newEndPoint = EditorGUILayout.Vector2Field("End point", oldEndPosition);
            if (newEndPoint != oldEndPosition) {
                Undo.RecordObject(itemData, "Change end point");
                itemData.path.EndPosition = newEndPoint;
            }

            var oldStartTangent = itemData.path.StartTangent;
            var newStartTangent = EditorGUILayout.Vector2Field("Start tangent", oldStartTangent);
            if (newStartTangent != oldStartTangent) {
                Undo.RecordObject(itemData, "Change start tangent");
                itemData.path.StartTangent = newStartTangent;
            }

            var oldEndTangent = itemData.path.EndTangent;
            var newEndTangent = EditorGUILayout.Vector2Field("End tangent", oldEndTangent);
            if (newEndTangent != oldEndTangent) {
                Undo.RecordObject(itemData, "Change end tangent");
                itemData.path.EndTangent = newEndTangent;
            }
        }

        public override void OnSceneGUI() {
            base.OnSceneGUI();

            // Draw and set path control points
            for (var i = 0; i < Path2D.Length; i++) {
                var oldPosition = itemData.path[i];
                var newPosition = (Vector2) Handles.FreeMoveHandle(itemData.path[i], Quaternion.identity, 10f,
                    Vector3.one, Handles.CircleHandleCap);
                if (newPosition != oldPosition) {
                    Undo.RecordObject(itemData, "Change slider control point");
                    itemData.path[i] = newPosition;
                }
            }

            // Draw connecting bezier curve 
            Handles.DrawBezier(
                itemData.path.StartPosition,
                itemData.path.EndPosition,
                itemData.path.StartTangent,
                itemData.path.EndTangent,
                Color.green, null, 2f);

            // Draw control lines from positions to tangents
            Handles.DrawLine(itemData.path.StartPosition, itemData.path.StartTangent);
            Handles.DrawLine(itemData.path.EndPosition, itemData.path.EndTangent);
        }
    }
}