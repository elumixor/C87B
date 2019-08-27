using Combo.DataContainers;
using UnityEditor;

namespace Combo.Items.Designers.Editors {
    [CustomEditor(typeof(ComboSliderDesigner))]
    public class ComboSliderDesignerEditor : ComboItemDesignerEditor<ComboSliderData> {
        protected override string DirectoryName => Helper.Directories.comboSliders;
        protected override string FileName { get; } = "Combo Slider";
    }
}