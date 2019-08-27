using Meta;
using UnityEngine;

namespace Combo.DataContainers {
    [CreateAssetMenu(fileName = "Combo Button Data", menuName = "Custom/Combo/Button Data")]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class ComboButtonData : ComboItemData {
        [SerializeField] private float size;
        [SerializeField] private Vector2 position;

        // ReSharper disable once ConvertToAutoProperty (Unity serialization)
        public override Vector2 Position {
            get => position;
            set => position = value;
        }

        // ReSharper disable once ConvertToAutoProperty (Unity serialization)
        public override float Size {
            get => size;
            set => size = value;
        }
    }
}