using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shared.Path.PathDrag {
    /// <summary>
    /// Makes an object be dragged along <see cref="Path2D"/> path
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class PathDraggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        /// <summary>
        /// Path along which to drag an object
        /// </summary>
        public Path2D path;

        /// <summary>
        /// Minimum distance between cursor and actual point (<see cref="evenlySpacedPoints"/>) on curve
        /// </summary>
        public float accuracyThreshold;

        /// <summary>
        /// Point on path where the object is currently located
        /// </summary>
        public int startPointIndex;

        /// <summary>
        /// Storage for generated evenly spaced points
        /// </summary>
        [SerializeField] private List<Vector2> evenlySpacedPoints;

        private bool isDragging;
        private Vector2 dragOffset;

        public enum DragDirection {
            Free,
            OnlyForward,
            OnlyBackward
        }

        public DragDirection dragDirection;

        public delegate void DragStoppedHandler(bool completed, int atIndex, int total);

        public delegate void DragStartedHandler(int atIndex, int total);

        public delegate void DragProgressHandler(float accuracy, int atIndex, int total);

        public event DragStoppedHandler OnDragStopped;
        public event DragStartedHandler OnDragStarted;
        public event DragProgressHandler OnDragProgress;

        public void OnPointerDown(PointerEventData eventData) {
            dragOffset = eventData.position - new Vector2(Screen.width / 2f, Screen.height / 2f) - (Vector2) transform.localPosition;
            isDragging = true;
            OnDragStarted?.Invoke(startPointIndex, evenlySpacedPoints.Count);
        }

        public void OnPointerUp(PointerEventData eventData) {
            isDragging = false;
            OnDragStopped?.Invoke(false, startPointIndex, evenlySpacedPoints.Count);
        }

        private void Update() {
            if (!isDragging) return;

            // todo: skips points, should  check if next point is in close proximity etc.
            // todo: does not work well with self-intersecting paths
            var newPosition = (Vector2) Input.mousePosition - new Vector2(Screen.width / 2f, Screen.height / 2f) - dragOffset;

            var distances = evenlySpacedPoints.Select(p => Vector2.Distance(p, newPosition)).ToArray();
            var distance = distances.Min();
            var accuracy = 1f - distance / accuracyThreshold;

            if (accuracy <= 0) {
                isDragging = false;
                OnDragStopped?.Invoke(false, startPointIndex, evenlySpacedPoints.Count);
                return;
            }

            var index = Array.IndexOf(distances, distance);

            switch (dragDirection) {
                case DragDirection.OnlyForward when index <= startPointIndex:
                case DragDirection.OnlyBackward when index >= startPointIndex:
                    return;
            }

            transform.localPosition = evenlySpacedPoints[startPointIndex = index];
            SetRotation();
            OnDragProgress?.Invoke(accuracy, startPointIndex, evenlySpacedPoints.Count);

            // todo: works only if dragDirection = forward
            if (startPointIndex == evenlySpacedPoints.Count - 1) OnDragStopped?.Invoke(true, startPointIndex, evenlySpacedPoints.Count);
        }

        /// <summary>
        /// Rotates towards next point in <see cref="evenlySpacedPoints"/>, or previous if
        /// <see cref="dragDirection"/> is <see cref="DragDirection.OnlyBackward"/>
        /// </summary>
        private void SetRotation() {
            var start = evenlySpacedPoints[startPointIndex];
            var isFirst = startPointIndex == 0;
            var isLast = startPointIndex == evenlySpacedPoints.Count - 1;

            if (dragDirection == DragDirection.OnlyForward || dragDirection == DragDirection.Free) {
                if (!isLast) {
                    var end = evenlySpacedPoints[startPointIndex + 1];
                    transform.localRotation = Quaternion.LookRotation(Vector3.forward, end - start);
                } else if (!isFirst) {
                    var previous = evenlySpacedPoints[startPointIndex - 1];
                    transform.localRotation = Quaternion.LookRotation(Vector3.forward, start - previous);
                } // else there is <= 1 points, so rotation is undetermined
            } else { // Backward rotation
                if (!isFirst) {
                    var end = evenlySpacedPoints[startPointIndex - 1];
                    transform.localRotation = Quaternion.LookRotation(Vector3.forward, end - start);
                } else if (!isLast) {
                    var previous = evenlySpacedPoints[startPointIndex + 1];
                    transform.localRotation = Quaternion.LookRotation(Vector3.forward, start - previous);
                } // else there is <= 1 points, so rotation is undetermined
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void SetEvenlySpacedPoints(List<Vector2> points) {
            if (EditorApplication.isPlaying) return;

            evenlySpacedPoints = points;
            SetPosition();
            SetRotation();
        }

        [Conditional("UNITY_EDITOR")]
        public void SetPosition() {
            if (EditorApplication.isPlaying) return;

            transform.localPosition = evenlySpacedPoints[startPointIndex];
        }

        [Conditional("UNITY_EDITOR")]
        public void SetRotationEditor() {
            if (EditorApplication.isPlaying) return;

            SetRotation();
        }
    }
}