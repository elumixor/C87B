using System.Linq;
using UnityEngine;

namespace Shared.Path {
    public static class Bezier {
        public static Vector2 Quadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t) {
            var l1 = Vector2.Lerp(p0, p1, t);
            var l2 = Vector2.Lerp(p1, p2, t);
            return Vector2.Lerp(l1, l2, t);
        }

        public static Vector2 Qubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t) {
            var l1 = Quadratic(p0, p1, p2, t);
            var l2 = Quadratic(p1, p2, p3, t);
            return Vector2.Lerp(l1, l2, t);
        }

        public static Vector2 Recursive(float t, params Vector2[] p) {
            if (p.Length != 3) return Vector2.Lerp(Recursive(t, p.Take(p.Length - 1).ToArray()), Recursive(t, p.Skip(1).ToArray()), t);

            var l1 = Vector2.Lerp(p[0], p[1], t);
            var l2 = Vector2.Lerp(p[1], p[2], t);
            return Vector2.Lerp(l1, l2, t);
        }
    }
}