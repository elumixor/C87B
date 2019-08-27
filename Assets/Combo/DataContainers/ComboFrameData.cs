using System.Collections.Generic;
using Meta;
using UnityEngine;

namespace Combo.DataContainers {
    /// <summary>
    /// Combo frame is a single part of <see cref="Combo"/> that keeps <see cref="ComboItem"/>s layout,
    /// <see cref="ComboFrameType"/> and it's <see cref="executionTime"/>. From containing data an animation and items are
    /// later instantiated.
    /// </summary>
    [CreateAssetMenu(fileName = "Combo Frame", menuName = "Custom/Combo/Combo Frame", order = 101)]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class ComboFrameData : ScriptableObject {
        /// <summary>
        /// Type of combo: determines how should the player execute <see cref="items"/>
        /// </summary>
        public ComboFrameType frameType;

        /// <summary>
        /// Combo items
        /// </summary>
        public List<ComboItemData> items;

        /// <summary>
        /// Maximum time for frame execution. After the time has expired the frame (and whole <see cref="Combo"/>)
        /// will have failed
        /// </summary>
        [Range(0f, 2f)] public float executionTime;
    }
}