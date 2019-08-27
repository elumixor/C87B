using Combo.DataContainers;
using UnityEditor;
using UnityEngine;

namespace Combo.Items.Editors {
    public class ComboItemDataEditor<T> : Editor where T : ComboItemData {
        protected T itemData;
        private const float MinSize = 0.001f;

        protected virtual void OnEnable() {
            itemData = (T) target;
        }

        public override void OnInspectorGUI() {
            var oldPosition = itemData.Position;
            var newPosition = EditorGUILayout.Vector2Field("Position", oldPosition);
            if (newPosition != oldPosition) {
                Undo.RecordObject(itemData, "Change position");
                itemData.Position = newPosition;
            }

            var oldSize = itemData.Size;
            var newSize = EditorGUILayout.FloatField("Size", oldSize);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (oldSize != newSize) {
                Undo.RecordObject(itemData, "Change size");
                itemData.Size = Mathf.Max(MinSize, newSize);
            }
        }

        public virtual void OnSceneGUI() {
            var oldPosition = itemData.Position;
            var newPosition = (Vector2) Handles.FreeMoveHandle(oldPosition, Quaternion.identity, 10,
                Vector3.one, Handles.CircleHandleCap);
            if (newPosition != oldPosition) {
                Undo.RecordObject(itemData, "Change item position");
                itemData.Position = newPosition;
            }

            var oldSize = itemData.Size * .5f;
            var newSize = Handles.RadiusHandle(Quaternion.identity, newPosition, oldSize);
            if (newSize != oldSize) {
                Undo.RecordObject(itemData, "Change item size");
                itemData.Size = Mathf.Max(newSize * 2f, MinSize);
            }
        }
    }
}