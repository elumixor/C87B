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
            get => path.Aggregate(Vector2.zero, (a, b) => a + b, v2 => v2 / Path2D.Length);
            set => path += value - Position;
        }

        /// <summary>
        /// Size is distance between two farthest points
        /// </summary>
        public override float Size {
            get => path.Select(p1 => path.Select(p2 => Vector2.Distance(p1, p2)).Max()).Max();
            set {
                var center = Position;
                var size = Size;

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (size == 0f) return;

                path -= center;
                path *= value / size;
                path += center;
            }
        }
    }
}