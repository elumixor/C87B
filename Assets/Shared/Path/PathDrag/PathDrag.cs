using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shared.Path.PathDrag {
    public class PathDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        /// <summary>
        /// Path creator component
        /// </summary>
        public Path2D path;
        /// <summary>
        /// Minimum distance between cursor and actual point (<see cref="points"/>) on curve
        /// </summary>
        [Range(0, 100)] public float maxDiffDistance;
        /// <summary>
        /// Point on path where the object is currently located
        /// </summary>
        public int pathPointIndex;

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
            transform.localPosition = points[pathPointIndex];
        }

        public void OnPointerDown(PointerEventData eventData) {
            dragOffset = eventData.position - new Vector2(Screen.width / 2f, Screen.height / 2f) - (Vector2) transform.localPosition;
            isDragging = true;
            OnDragStarted?.Invoke(pathPointIndex, points.Count);
        }

        public void OnPointerUp(PointerEventData eventData) {
            isDragging = false;
            OnDragStopped?.Invoke(false, pathPointIndex, points.Count);
        }

        private void Update() {
            if (!isDragging) return;

            var newPosition = (Vector2) Input.mousePosition - new Vector2(Screen.width / 2f, Screen.height / 2f) - dragOffset;

            var distances = points.Select(p => Vector2.Distance(p, newPosition)).ToArray();
            var distance = distances.Min();
            var accuracy = 1f - distance / maxDiffDistance;

            if (accuracy <= 0) {
                isDragging = false;
                OnDragStopped?.Invoke(false, pathPointIndex, points.Count);
                return;
            }

            var index = Array.IndexOf(distances, distance);

            switch (dragDirection) {
                case DragDirection.OnlyForward when index <= pathPointIndex:
                case DragDirection.OnlyBackward when index >= pathPointIndex:
                    return;
            }

            OnDragProgress?.Invoke(accuracy, pathPointIndex, points.Count);
            transform.localPosition = points[pathPointIndex = index];

            if (pathPointIndex == points.Count - 1) OnDragStopped?.Invoke(true, pathPointIndex, points.Count);
        }
    }
}