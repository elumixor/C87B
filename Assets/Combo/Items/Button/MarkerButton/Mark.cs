using System;
using UnityEngine;

namespace Combo.Items.Button {
    /// <summary>
    /// Structure to conveniently hold references to mark data
    /// </summary>
    [Serializable]
    public struct Mark {
        /// <summary>
        /// Mark's container's transform
        /// </summary>
        public RectTransform container;

        /// <summary>
        /// Mark's transform
        /// </summary>
        public RectTransform mark;

        /// <summary>
        /// Mark's <see cref="SVGImage"/> component
        /// </summary>
        public SVGImage svg;
    }
}