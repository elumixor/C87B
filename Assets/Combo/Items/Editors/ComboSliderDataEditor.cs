using Combo.DataContainers;
using Shared;
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
            OffsetInspectorGUI();

            itemData.path = itemData.path.OnInspectorGUI(offset, itemData);
            EditorUtility.SetDirty(target);
            itemData.UpdateUsages();
        }

        public override void OnSceneGUI() {
            itemData.path = itemData.path.OnSceneGUI(offset, itemData);
            EditorUtility.SetDirty(target);
            itemData.UpdateUsages();
        }
    }
}