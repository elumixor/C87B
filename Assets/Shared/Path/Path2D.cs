using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Shared.Path {
    [Serializable]
    public class Path2D : IEnumerable<Vector2> {
        /// <summary>
        /// Minimum of control points for path
        /// </summary>
        public const int Length = 4;

        /// <summary>
        /// Control points of path
        /// </summary>
        [SerializeField] private Vector2[] points;

        /// <summary>
        /// Distance between points
        /// </summary>
        [Range(.1f, 100f)] public float spacing = 1f;

        /// <summary>
        /// How close are the points to the actual curve
        /// </summary>
        [Range(.1f, 100f)] public float resolution = 1f;

        /// <summary>
        /// Create path as copy of another
        /// </summary>
        /// <param name="other">Path to copy</param>
        public Path2D(Path2D other) {
            spacing = other.spacing;
            resolution = other.resolution;
            points = new Vector2[Length];
            Array.Copy(other.points, points, Length);
        }

        /// <summary>
        /// Create default path
        /// </summary>
        public Path2D() {
            points = new[] {Vector2.left, (Vector2.left + Vector2.up) * .5f, (Vector2.right + Vector2.down) * .5f, Vector2.right};
        }

        /// <summary>
        /// Create path with center point
        /// </summary>
        public Path2D(Vector2 center) {
            points = new[] {
                Vector2.left + center,
                (Vector2.left + Vector2.up) * .5f + center,
                (Vector2.right + Vector2.down) * .5f + center,
                Vector2.right + center
            };
        }

        public static bool operator ==([CanBeNull] Path2D a, [CanBeNull] Path2D b) {
            if (ReferenceEquals(a, null)) {
                return ReferenceEquals(b, null);
            }

            if (ReferenceEquals(b, null)) return false;
            
            for (var i = 0; i < Length; i++) {
                if (a[i] != b[i]) return false;
            }

            return false;
        }

        public static bool operator !=(Path2D a, Path2D b) {
            return !(a == b);
        }

        private bool Equals(Path2D other) {
            return Equals(points, other.points);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Path2D) obj);
        }

        public override int GetHashCode() {
            return points != null ? points.GetHashCode() : 0;
        }

        /// <summary>
        /// Indexer as shortcut to <see cref="points"/>
        /// </summary>
        public Vector2 this[int i] {
            get => points[i];
            set => points[i] = value;
        }

        /// <summary>
        /// Start point of a path
        /// </summary>
        public Vector2 StartPosition {
            get => points[0];
            set => points[0] = value;
        }

        /// <summary>
        /// End point of a path
        /// </summary>
        public Vector2 EndPosition {
            get => points[3];
            set => points[3] = value;
        }

        /// <summary>
        /// Tangent of the starting point
        /// </summary>
        public Vector2 StartTangent {
            get => points[1];
            set => points[1] = value;
        }

        /// <summary>
        /// Tangent of the ending point
        /// </summary>
        public Vector2 EndTangent {
            get => points[2];
            set => points[2] = value;
        }

        /// <summary>
        /// Iterate over points
        /// </summary>
        public IEnumerator<Vector2> GetEnumerator() {
            return points.ToList().GetEnumerator();
        }

        /// <summary>
        /// Iterate over points
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Interpolates evenly spaced points on the path
        /// </summary>
        public List<Vector2> EvenlySpacedPoints() {
            if (spacing <= 0f || resolution <= 0f)
                throw new ArgumentException($"Spacing ({spacing}) and resolution ({resolution}) should be greater than zero.");

            var evenlySpaced = new List<Vector2> {StartPosition};
            var previousPoint = StartPosition;
            var traveledDistance = 0f;

            var controlNetLength = Vector2.Distance(points[0], points[1])
                                   + Vector2.Distance(points[1], points[2])
                                   + Vector2.Distance(points[2], points[3]);

            var estimatedLength = Vector2.Distance(points[0], points[3]) + controlNetLength * .5f;
            var divisions = Mathf.CeilToInt(estimatedLength * resolution * 10);

            var delta = .1f / divisions;
            var t = 0f;

            while (t <= 1f) {
                t += delta;

                var pointOnCurve = Bezier.Qubic(points[0], points[1], points[2], points[3], t);
                traveledDistance += Vector2.Distance(previousPoint, pointOnCurve);

                while (traveledDistance >= spacing) {
                    var overshoot = traveledDistance - spacing;
                    var newEvenlySpaced = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshoot;
                    evenlySpaced.Add(newEvenlySpaced);
                    traveledDistance = overshoot;
                    previousPoint = newEvenlySpaced;
                }

                previousPoint = pointOnCurve;
            }

            return evenlySpaced;
        }

        /// <summary>
        /// Creates mesh from path
        /// </summary>
        /// <param name="width">Distance from path that determines mesh' width</param>
        /// <returns></returns>
        public Mesh CreateMesh(float width) {
            var centerPoints = EvenlySpacedPoints();

            var vertices = new Vector3[centerPoints.Count * 2];
            var uvs = new Vector2[vertices.Length];
            var triangles = new int[6 * (centerPoints.Count - 1)];

            var vertIndex = 0;
            var triIndex = 0;

            for (var i = 0; i < centerPoints.Count; i++) {
                var forward = Vector2.zero;

                if (i < centerPoints.Count - 1) {
                    forward += centerPoints[i + 1] - centerPoints[i];
                }

                if (i > 0) {
                    forward += centerPoints[i] - centerPoints[i - 1];
                }

                forward.Normalize();

                var left = new Vector2(-forward.y, forward.x);
                vertices[vertIndex] = centerPoints[i] + width * .5f * left;
                vertices[vertIndex + 1] = centerPoints[i] - width * .5f * left;

                var completion = i / (float) (centerPoints.Count - 1);

                uvs[vertIndex] = new Vector2(0, completion);
                uvs[vertIndex + 1] = new Vector2(1, completion);

                if (i < centerPoints.Count - 1) {
                    triangles[triIndex] = vertIndex;
                    triangles[triIndex + 1] = vertIndex + 2;
                    triangles[triIndex + 2] = vertIndex + 1;

                    triangles[triIndex + 3] = vertIndex + 1;
                    triangles[triIndex + 4] = vertIndex + 2;
                    triangles[triIndex + 5] = vertIndex + 3;
                }

                vertIndex += 2;
                triIndex += 6;
            }

            var mesh = new Mesh {vertices = vertices, triangles = triangles, uv = uvs};
            return mesh;
        }

        public static Path2D operator +(Path2D path, Vector2 offset) {
            var newPath = new Path2D(path);
            for (var i = 0; i < Length; i++) newPath[i] = path[i] + offset;
            return newPath;
        }

        public static Path2D operator -(Path2D path, Vector2 offset) {
            return path + -offset;
        }

        /// <summary>
        /// Scales path around Vector2.zero
        /// </summary>
        public static Path2D operator *(Path2D path, float scale) {
            var newPath = new Path2D(path);
            for (var i = 0; i < Length; i++) newPath[i] = path[i] * scale;
            return newPath;
        }
    }
}