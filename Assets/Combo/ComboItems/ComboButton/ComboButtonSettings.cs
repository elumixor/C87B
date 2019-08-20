using UnityEngine;

namespace Combo.ComboItems.ComboButton {
    public class ComboButtonSettings : ComboItemSettings {
        [Header("General")]
        [Range(0, 10)]public float size;
        [Range(0, 10)] public int marksCount;

        [Header("Marks position")]
        [Range(0, 5)] public float markDistance;
        [Range(0, 360)] public float offsetAngle;
        [Range(-180, 180)] public float markRotation;

        [Header("Marks scale")]
        [Range(0, 10)] public float markRatio;
        [Range(0f, 1f)] public float markUniformScale;

        [Header("Sprite")]
        public Sprite markSprite;
    }
}