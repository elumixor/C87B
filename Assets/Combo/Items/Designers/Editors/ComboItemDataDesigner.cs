using Combo.DataContainers;
using Shared.Behaviours;
using UnityEngine;

namespace Combo.Items.Designers.Editors {
    public abstract class ComboItemDesignerEditor<T> : DesignerEditor<T> where T : ComboItemData {
        protected override void OnSceneGUI() {
            var position = designer.transform.position;
            itemData.Position += (Vector2) position;
            onSceneGUI();
            itemData.Position -= (Vector2) position;
            Repaint();
        }
        
        protected override string FileName { get; } = "Combo Item";
    }
}