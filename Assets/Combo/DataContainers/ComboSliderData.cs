using System.Linq;
using Meta;
using Shared.Path;
using UnityEngine;

namespace Combo.DataContainers {
    [CreateAssetMenu(fileName = "Combo Slider Data", menuName = "Custom/Combo/Slider Data")]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class ComboSliderData : ComboItemData {
        public Path2D path;

        /// <summary>
        /// Position is equal to center of a line between two farthest points
        /// </summary>
        public override Vector2 Position {
            get => path.Position;
            set => path.Position = value;
        }

        /// <summary>
        /// Size is distance between two farthest points
        /// </summary>
        public override float Size {
            get => path.Size;
            set => path.Size = value;
        }
    }
}