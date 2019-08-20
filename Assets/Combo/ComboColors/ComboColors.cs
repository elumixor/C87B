using UnityEngine;

namespace Combo.ComboColors {
    [CreateAssetMenu(fileName = "New Combo Colors", menuName = "ComboColors")]
    public class ComboColors : ScriptableObject {
        public Color unordered;
        public Color ordered;
        public Color simultaneous;
    }
}