using System;
using System.Collections.Generic;
using System.Diagnostics;
using Shared.PropertyDrawers;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Combo.Items.Button.MarkerButton {
    /// <summary>
    /// Button with floating markers
    /// </summary>
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class MarkerButton : MonoBehaviour {
        /// <summary>
        /// Reference to container of transform containers (root of containers)
        /// </summary>
        [SerializeField]
        private RectTransform markersContainer;

        [SerializeField, Required]
        private Sprite markerImage;

        [Range(0, 10), SerializeField]
        private int markersCount;

        /// <summary>
        /// Distance of mark from center
        /// </summary>
        [SerializeField, Range(0, 2)]
        private float markerDistance;

        /// <summary>
        /// Rotation of all marks with origin in object's center (rotation of containers)
        /// </summary>
        [SerializeField, Range(0, 360)]
        public float offsetAngle;

        /// <summary>
        /// Individual marks rotation
        /// </summary>
        [SerializeField, Range(-180, 180)]
        private float markerRotation;

        /// <summary>
        /// Size of mark
        /// </summary>
        [SerializeField, Range(0, 2)]
        private float markerSize;

        [SerializeField]
        private Color accent;

        /// <summary>
        /// List of references to marks
        /// </summary>
        [SerializeField]
        public List<(RectTransform container, RectTransform mark, SVGImage svg)> markers;

        private void Update() {
            if (markersContainer == null || markers == null || markersCount == 0) return;

            var angleBetweenMarks = 360 / markersCount;
            for (var index = 0; index < markersCount; index++) {
                var (container, mark, svg) = markers[index];
                var angle = angleBetweenMarks * index + offsetAngle;

                container.localRotation = Quaternion.Euler(0f, 0f, angle);
                container.localPosition = Vector3.zero;
                container.localScale = Vector3.one;
                container.sizeDelta = Vector2.one;

                mark.localPosition = new Vector3(0, markerDistance, 0);
                mark.localRotation = Quaternion.Euler(0, 0, markerRotation);
                mark.localScale = Vector2.one * markerSize;
                mark.sizeDelta = Vector2.one;

                svg.color = accent;
            }
        }

        private void Start() {
            if (markers == null) FindMarkers();
        }

        [Conditional("UNITY_EDITOR")]
        public void FindMarkers() {
            markers = new List<(RectTransform container, RectTransform mark, SVGImage svg)>(markersContainer.childCount);
            foreach (Transform markContainer in markersContainer) {
                var mark = markContainer.GetChild(0);
                markers.Add((markContainer.GetComponent<RectTransform>(),
                    mark.GetComponent<RectTransform>(),
                    mark.GetComponent<SVGImage>()));
            }
        }

        public void CreateMarkers() {
            if (markersContainer == null)
                markersContainer = new GameObject("Markers Container", typeof(RectTransform)).GetComponent<RectTransform>();

            markersContainer.SetParent(transform);

            markersContainer.sizeDelta = Vector2.one;
            markersContainer.localPosition = Vector3.zero;
            markersContainer.localScale = Vector3.one;

            markers = new List<(RectTransform container, RectTransform mark, SVGImage svg)>(markersContainer.childCount);

            for (var i = 0; i < markersCount; i++) {
                var container = new GameObject($"Mark {i}", typeof(RectTransform)).GetComponent<RectTransform>();
                container.SetParent(markersContainer);

                var mark = new GameObject("Mark", typeof(RectTransform), typeof(SVGImage)).GetComponent<RectTransform>();
                mark.SetParent(container);

                var svg = mark.GetComponent<SVGImage>();
                svg.sprite = markerImage;

                markers.Add((container, mark, svg));
            }
        }
    }
}