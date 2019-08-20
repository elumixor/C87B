using UnityEngine;

namespace Shared.GameObjectManagement.Normalize {
    public static class NormalizeTransform {
        public static Transform Position(Transform transform1) {
            transform1.localPosition = Vector3.zero;
            return transform1;
        }

        public static Transform Rotation(Transform transform1) {
            transform1.rotation = Quaternion.identity;
            return transform1;
        }

        public static Transform Scale(Transform transform1) {
            transform1.localScale = Vector3.one;
            return transform1;
        }

        public static GameObject All(Transform transform1) {
            return Position(Scale(Rotation(transform1))).gameObject;
        }

        public static GameObject All(GameObject go) {
            return All(go.transform);
        }
    }
}