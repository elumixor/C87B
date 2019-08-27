using System.Collections.Generic;
using System.Linq;
using Combo.DataContainers;
using Combo.Items.Editors;
using Shared.Behaviours;
using UnityEditor;

namespace Combo.Frame {
    [CustomEditor(typeof(ComboFrameDesigner))]
    public class ComboFrameDesignerEditor : DesignerEditor<ComboFrameData> {
        private List<Editor> editors;

        protected override void OnEnable() {
            base.OnEnable();
            editors = designer.itemData.items.Select(CreateEditor).ToList();
        }

        protected override void OnSceneGUI() {
            base.OnSceneGUI();
            foreach (var editor in editors) {
                if (editor is ComboButtonDataEditor buttonDataEditor) buttonDataEditor.OnSceneGUI();
                if (editor is ComboSliderDataEditor sliderDataEditor) sliderDataEditor.OnSceneGUI();
            }
        }
        
        protected override string DirectoryName => Helper.Directories.comboFrame;
    }
}