using Combo.DataContainers;
using UnityEditor;

namespace Combo.Items.Designers.Editors {
    [CustomEditor(typeof(ComboButtonDesigner))]
    public class ComboButtonDesignerEditor : ComboItemDesignerEditor<ComboButtonData> {
        protected override string DirectoryName => Helper.Directories.comboButtons;
        protected override string FileName { get; } = "Combo Button";
    }
}