using System.Collections;
using System.Collections.Generic;
using Combo.ComboItems;
using UnityEngine;

namespace Combo.Domain {
    /// <summary>
    /// Combo frame is a single part of <see cref="Combo"/> that keeps <see cref="ComboItem"/>s layout,
    /// <see cref="ComboFrameType"/> and it's <see cref="executionTime"/>. From containing data an animation and items are
    /// later instantiated.
    /// </summary>
    public class ComboFrame : ScriptableObject, IEnumerable<ComboItem.Data> {
        /// <summary>
        /// Specifies how the player should execute <see cref="Combo.Combo"/>
        /// </summary>
        public enum ComboFrameType {
            /// <summary>
            /// Items can be executed in free order
            /// </summary>
            Unordered,
            /// <summary>
            /// Items should be executed in specified order
            /// </summary>
            Ordered,
            /// <summary>
            /// Items should be executed simultaneously, buttons
            /// </summary>
            SimultaneousButton,
            /// <summary>
            /// Items should be executed simultaneously, sliders
            /// </summary>
            SimultaneousSlider
        }

        /// <summary>
        /// Type of combo: determines how the player executed <see cref="items"/>
        /// </summary>
        public ComboFrameType frameType;
        /// <summary>
        /// Combo items
        /// </summary>
        public ComboItem.Data[] items;
        /// <summary>
        /// Maximum time for frame execution. After the time has expired the frame (and whole <see cref="Combo"/>)
        /// will have failed
        /// </summary>
        [Range(0f, 2f)] public float executionTime;
        /// <summary>
        /// Shortcut to <see cref="items"/>
        /// </summary>
        public ComboItem.Data this[int i] => items[i];
        /// <summary>
        /// Number or combo items
        /// </summary>
        public int Length => items.Length;
        /// <summary>
        /// Iterate over <see cref="items"/>
        /// </summary>
        public IEnumerator<ComboItem.Data> GetEnumerator() {
            return (IEnumerator<ComboItem.Data>) items.GetEnumerator();
        }
        /// <summary>
        /// Iterate over <see cref="items"/>
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        
    }
}