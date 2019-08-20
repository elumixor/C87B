using System;
using Shared.PropertyDrawers;
using UnityEngine;

namespace Shared.Behaviours.Progressable.CubicProgressable {
    public class CubicProgressable : ProgressableBehaviour {
        [Flags]
        private enum ScaleDirection {
            Forward = 1,
            Backward = 2
        }

        [EnumFlag, SerializeField] private ScaleDirection scaleDirection;

        [Flags]
        private enum ScaleAxis {
            X = 1,
            Y = 2,
            Z = 4
        }

        [EnumFlag, SerializeField] private ScaleAxis scaleAxis;

        [SerializeField] private Vector3 maxScale;
        [SerializeField, Range(0f, 1f)] private float progress;

        public override float Progress {
            get => progress;
            set {
                progress = value;
                UpdatePosition();
            }
        }

        private void UpdatePosition() {
            transform.localScale = new Vector3(
                scaleAxis.HasFlag(ScaleAxis.X) ? maxScale.x * progress : maxScale.x,
                scaleAxis.HasFlag(ScaleAxis.Y) ? maxScale.y * progress : maxScale.y,
                scaleAxis.HasFlag(ScaleAxis.Z) ? maxScale.z * progress : maxScale.z
            );
        }

        private void OnValidate() {
            UpdatePosition();
        }

        /// <summary>
        /// Draw max scale
        /// </summary>
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red * .5f;
            Gizmos.matrix = Matrix4x4.Translate(transform.position) * Matrix4x4.Rotate(transform.localRotation);
            Gizmos.DrawWireCube(Vector3.zero, maxScale);
        }
    }
}