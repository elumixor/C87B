using Combo.DataContainers;
using Shared.Behaviours;
using UnityEditor;

namespace Combo.Items.Designers.Editors {
    public abstract class ComboItemDesignerEditor<T> : DesignerEditor<T> where T : ComboItemData {
        protected override void OnEnable() {
            base.OnEnable();
            Tools.hidden = true;
        }
        protected override void OnSceneGUI() {
            onSceneGUI();
            Repaint();
        }

        protected override string FileName { get; } = "Combo Item";

        protected virtual void OnDisable() {
            Tools.hidden = false;
        }
    }
}