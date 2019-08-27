using UnityEngine;

namespace Combo.DataContainers {
    public abstract class ComboItemData : ScriptableObject {
        public abstract Vector2 Position { get; set; }
        public abstract float Size { get; set; }
    }
}