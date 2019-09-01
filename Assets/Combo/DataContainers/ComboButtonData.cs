using Meta;
using UnityEngine;

namespace Combo.DataContainers {
    [CreateAssetMenu(fileName = "Combo Button Data", menuName = "Custom/Combo/Button Data")]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class ComboButtonData : ComboItemData {
        [SerializeField] private float size;
        [SerializeField] private Vector2 position;

        /// <summary>
        /// Position of button's center from bottom left corner
        /// </summary>
        // ReSharper disable once ConvertToAutoProperty (Unity serialization)
        public override Vector2 Position {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// Button's diameter
        /// </summary>
        // ReSharper disable once ConvertToAutoProperty (Unity serialization)
        public override float Size {
            get => size;
            set => size = value;
        }
    }
}