using System;
using UnityEngine;

namespace Scenes.FightScene.HPDisplay.DisplayData {
    [Serializable]
    public struct HPDisplayData {
        /// <summary>
        /// Color of health-related objects (e.g. health bar)
        /// </summary>
        public Material material; // material instead of color?

        /// <summary>
        /// Order of health type in list
        /// </summary>
        public int order;
    }
}