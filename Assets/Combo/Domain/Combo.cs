using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combo.Domain {
    /// <summary>
    /// Combo is an ordered collection of <see cref="ComboFrame"/>s, that appear consecutively.
    /// All containing <see cref="frames"/> should be executed correctly in order to correctly execute the combo.
    /// </summary>
    public class Combo : ScriptableObject, IEnumerable<ComboFrame> {
        /// <summary>
        /// Combo frames
        /// </summary>
        public ComboFrame[] frames;

        /// <summary>
        /// Shortcut to <see cref="frames"/>
        /// </summary>
        /// <param name="i"></param>
        public ComboFrame this[int i] => frames[i];

        /// <summary>
        /// Number of <see cref="ComboFrame"/>s
        /// </summary>
        public int Length => frames.Length;

        /// <summary>
        /// Iterate over <see cref="frames"/>
        /// </summary>
        public IEnumerator<ComboFrame> GetEnumerator() {
            return (IEnumerator<ComboFrame>) frames.GetEnumerator();
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
        public static Combo Create(ComboFrame[] frames) {
            var instance = CreateInstance<Combo>();
            instance.frames = frames;
            return instance;
        }

        public static implicit operator ComboFrame[](Combo combo) => combo.frames;
    }
}