using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combo.DataContainers;
using Shared.Behaviours.Animable;
using Shared.EditorScripts;
using Shared.Path.PathDrag;
using UnityEditor;
using UnityEngine;

namespace Combo.Items.Slider {
    [DisallowMultipleComponent]
    public class ComboSlider : ComboItem<ComboSliderData> {
        [SerializeField] private SVGImage arrowImage;
        [SerializeField] private SVGImage segmentImage;

        /// <summary>
        /// Maximum distance from position of <see cref="arrow"/> to actual path
        /// </summary>
        [SerializeField, Range(0, 100)] private float accuracyThreshold = 10;

        /// <summary>
        /// Instantiated draggable arrow
        /// </summary>
        private SliderArrow arrow;

        /// <summary>
        /// References to created segments' transforms and images
        /// </summary>
        private SliderSegment[] segments;

        /// <summary>
        /// Total accuracy across all <see cref="segments"/>
        /// </summary>
        private float accumulatedAccuracy;

//        /// <summary>
//        /// Assign components and <see cref="accuracyThreshold"/> on script change
//        /// </summary>
//        private void OnValidate() {
//            if (arrow != null) arrow.accuracyThreshold = accuracyThreshold;
//        }

        private void Update() {
//            if (shouldDestroy && segments.All(i => i == null) && sliderArrowInstance == null) Destroy(gameObject);
        }

        /// <summary>
        /// Start function creates draggable arrow and evenly spaced <see cref="segments"/> along slider path
        /// </summary>
        private void Awake() {
            var settings = Settings;
            if (settings == null) return;

            var arrowInstance = new GameObject("Slider Arrow", typeof(SliderArrow));
            arrowInstance.transform.SetParent(transform);
            arrow = arrowInstance.GetComponent<SliderArrow>();
            arrow.image = arrowImage;

            var evenlySpacedPoints = settings.path.EvenlySpacedPoints();

            var pathDraggable = arrow.pathDraggable;
            pathDraggable.path = settings.path;
            pathDraggable.dragDirection = PathDraggable.DragDirection.OnlyForward;
            pathDraggable.startPointIndex = 0;
            pathDraggable.accuracyThreshold = accuracyThreshold;

            pathDraggable.OnDragProgress += (accuracy, index, total) => {
                accumulatedAccuracy += accuracy;
                segments[index].AnimateHit();
            };

            pathDraggable.OnDragStopped += (completed, index, total) => {
                if (completed) OnHit(accumulatedAccuracy / evenlySpacedPoints.Count);
                else OnMissed();
            };

            // Coroutine to create segments
            IEnumerator createSegments() {
                segments = new SliderSegment[evenlySpacedPoints.Count - 1];

                for (var i = 1; i < evenlySpacedPoints.Count; i++) {
                    var segmentInstance = new GameObject($"Segment {i}", typeof(SliderSegment));
                    segmentInstance.transform.SetParent(transform);
                    segments[i - 1] = segmentInstance.GetComponent<SliderSegment>();
                    segments[i - 1].transform.localPosition = evenlySpacedPoints[i];
                    yield return null;
                }
            }

            StartCoroutine(createSegments());
        }


        /// <summary>
        /// Executes when slider arrow was successfully and accurately dragged to the end
        /// </summary>
        public override bool OnHit(float accuracy = 1f) {
            if (!base.OnHit(accuracy)) return false;

            arrow.animationDraggable.AnimateHit();
            return true;
        }

        /// <summary>
        /// Executes when slider arrow was not accurately dragged to the end of the slider
        /// </summary>
        public override bool OnMissed() {
            if (!base.OnMissed()) return false;

            arrow.animationDraggable.AnimateMiss();
            foreach (var segment in segments) segment.AnimateMiss();

            return true;
        }

        public override void OnSettingsChanged() { }
    }
}