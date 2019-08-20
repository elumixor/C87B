using System;

namespace Character.HP {
    /// <summary>
    /// Represents health level some object
    /// </summary>
    [Serializable]
    public class HitPoints {
        public HPType type;
        public float value;

        public HitPoints() { }

        public HitPoints(HitPoints other) {
            type = other.type;
            value = other.value;
        }

        public HitPoints(HPType type, float value) {
            this.type = type;
            this.value = value;
        }

        public override string ToString() => $"{type}: {value}";
    }
}