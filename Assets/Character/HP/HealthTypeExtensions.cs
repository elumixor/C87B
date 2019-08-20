using System.Collections.Generic;

namespace Character.HP {
    public static class HealthTypeExtensions {
        public static IEnumerable<HPType> HealthTypeValues {
            get {
                yield return HPType.Shields;
                yield return HPType.Armor;
            }
        }
    }
}