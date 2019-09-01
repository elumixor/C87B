using Combo.DataContainers;
using Shared;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;

namespace Combo.Items.Editors {
    public class ComboItemDataEditor<T> : Editor where T : ComboItemData {
        protected T itemData;
        private const float MinSize = 0.001f;
        private RectTransform canvas;
        public Vector2 offset;

        protected virtual void OnEnable() {
            itemData = (T) target;
            var c = FindObjectOfType<Canvas>();
            if (c != null) {
                canvas = c.GetComponent<RectTransform>();
                var rect = canvas.rect;
                offset = new Vector2(rect.width * .5f, rect.height * .5f);
            } else offset = Vector2.zero;
        }

        public override void OnInspectorGUI() {
            var newCanvas = (RectTransform) EditorGUILayout.ObjectField("Canvas", canvas, typeof(RectTransform), true);
            if (newCanvas != canvas) {
                canvas = newCanvas;
                if (canvas != null) {
                    var rect = canvas.rect;
                    offset = new Vector2(rect.width * .5f, rect.height * .5f);
                } else offset = Vector2.zero;
            }

            itemData.Position += offset;
            var oldPosition = itemData.Position;
            var newPosition = EditorGUILayout.Vector2Field("Position", oldPosition);
            if (newPosition != oldPosition) {
                Undo.RecordObject(itemData, "Change position");
                itemData.Position = newPosition;
            }

            itemData.Position -= offset;

            var oldSize = itemData.Size;
            var newSize = EditorGUILayout.FloatField("Size", oldSize);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (oldSize != newSize) {
                Undo.RecordObject(itemData, "Change size");
                itemData.Size = Mathf.Max(MinSize, newSize);
            }

            itemData.UpdateUsages();
        }

        public virtual void OnSceneGUI() {
            itemData.Position += offset;

            var oldPosition = itemData.Position;
            var newPosition = (Vector2) Handles.FreeMoveHandle(oldPosition, Quaternion.identity, 10,
                Vector3.one, Handles.CircleHandleCap);
            if (newPosition != oldPosition) {
                Undo.RecordObject(itemData, "Change item position");
                itemData.Position = newPosition;
            }

            itemData.Position -= offset;

            var oldSize = itemData.Size * .5f;
            var newSize = Handles.RadiusHandle(Quaternion.identity, newPosition, oldSize);
            if (newSize != oldSize) {
                Undo.RecordObject(itemData, "Change item size");
                itemData.Size = Mathf.Max(newSize * 2f, MinSize);
            }

            itemData.UpdateUsages();
        }
    }
}