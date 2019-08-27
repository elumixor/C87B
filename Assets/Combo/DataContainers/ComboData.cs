using System.Collections;
using System.Collections.Generic;
using Meta;
using UnityEngine;

namespace Combo.DataContainers {
    /// <summary>
    /// Combo is an ordered collection of <see cref="ComboFrameData"/>s, that appear consecutively.
    /// All containing <see cref="frames"/> should be executed correctly in order to correctly execute the combo.
    /// </summary>
    [CreateAssetMenu(fileName = "Combo", menuName = "Custom/Combo/Combo", order = 1)]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class ComboData : ScriptableObject, IEnumerable<ComboFrameData> {
        /// <summary>
        /// Combo frames
        /// </summary>
        public ComboFrameData[] frames;

        /// <summary>
        /// Shortcut to <see cref="frames"/>
        /// </summary>
        /// <param name="i"></param>
        public ComboFrameData this[int i] => frames[i];

        /// <summary>
        /// Number of <see cref="ComboFrameData"/>s
        /// </summary>
        public int Length => frames.Length;

        /// <summary>
        /// Iterate over <see cref="frames"/>
        /// </summary>
        public IEnumerator<ComboFrameData> GetEnumerator() {
            return (IEnumerator<ComboFrameData>) frames.GetEnumerator();
        }

        /// <summary>
        /// Iterate over  <see cref="frames"/>
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates DCombo instance from frames
        /// </summary>
        public static ComboData Create(ComboFrameData[] frames) {
            var instance = CreateInstance<ComboData>();
            instance.frames = frames;
            return instance;
        }

        public static implicit operator ComboFrameData[](ComboData comboData) => comboData.frames;
    }
}