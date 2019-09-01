using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Minimum distance between cursor and actual point (<see cref="points"/>) on curve
        /// </summary>
        [Range(0, 100)]
        public float accuracyThreshold;

        /// <summary>
        /// Point on path where the object is currently located
        /// </summary>
        public int startPointIndex;

        private bool isDragging;
        private Vector2 dragOffset;
        private List<Vector2> points;

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

        private void Start() {
            points = path.EvenlySpacedPoints();
            transform.localPosition = points[startPointIndex];
        }

        public void OnPointerDown(PointerEventData eventData) {
            dragOffset = eventData.position - new Vector2(Screen.width / 2f, Screen.height / 2f) - (Vector2) transform.localPosition;
            isDragging = true;
            OnDragStarted?.Invoke(startPointIndex, points.Count);
        }

        public void OnPointerUp(PointerEventData eventData) {
            isDragging = false;
            OnDragStopped?.Invoke(false, startPointIndex, points.Count);
        }

        private void Update() {
            if (!isDragging) return;

            var newPosition = (Vector2) Input.mousePosition - new Vector2(Screen.width / 2f, Screen.height / 2f) - dragOffset;

            var distances = points.Select(p => Vector2.Distance(p, newPosition)).ToArray();
            var distance = distances.Min();
            var accuracy = 1f - distance / accuracyThreshold;

            if (accuracy <= 0) {
                isDragging = false;
                OnDragStopped?.Invoke(false, startPointIndex, points.Count);
                return;
            }

            var index = Array.IndexOf(distances, distance);

            switch (dragDirection) {
                case DragDirection.OnlyForward when index <= startPointIndex:
                case DragDirection.OnlyBackward when index >= startPointIndex:
                    return;
            }

//            if (index < total - 1) SetArrowRotation(sliderArrowInstance.transform, index);
            OnDragProgress?.Invoke(accuracy, startPointIndex, points.Count);
            transform.localPosition = points[startPointIndex = index];

            if (startPointIndex == points.Count - 1) OnDragStopped?.Invoke(true, startPointIndex, points.Count);
        }

        /// <summary>
        /// Set rotation of slider arrow transform
        /// </summary>
        /// <param name="arrowTransform">Transform of slider arrow</param>
        /// <param name="currentPointIndex">Index of point, where the arrow is currently positioned</param>
//        private void SetRotation(Transform arrowTransform, int currentPointIndex) {
//            arrowTransform.localRotation =
//                Quaternion.FromToRotation(Vector3.up,
//                    (Vector3) evenlySpacedPoints[currentPointIndex + 1] - arrowTransform.localPosition);
//        }
    }
}