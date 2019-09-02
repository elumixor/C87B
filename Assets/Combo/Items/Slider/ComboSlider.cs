using System.Collections;
using Combo.DataContainers;
using JetBrains.Annotations;
using Shared.Behaviours.Animable;
using Shared.Path;
using Shared.Path.PathDrag;
using Shared.PropertyDrawers;
using UnityEditor.Animations;
using UnityEngine;

namespace Combo.Items.Slider {
    [DisallowMultipleComponent]
    public class ComboSlider : ComboItem<ComboSliderData> {
        [SerializeField] private Sprite arrowImage;
        [SerializeField] private Sprite segmentImage;

        [Range(0, 100)] public float arrowSize;
        [Range(0, 100)] public float segmentSize;

        [SerializeField, Required] private AnimatorController arrowAnimationController;
        [SerializeField, Required] private AnimatorController segmentsAnimationController;

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

        [CanBeNull]
        public Path2D Path {
            [Pure]
            get => Settings != null ? Settings.path : null;
        }

        /// <summary>
        /// Start function creates draggable arrow and evenly spaced <see cref="segments"/> along slider path
        /// </summary>
        private void Awake() {
            var settings = Settings;
            if (settings == null) return;

            var arrowInstance = new GameObject("Slider Arrow", typeof(SliderArrow));
            var arrowTransform = arrowInstance.GetComponent<RectTransform>();

            arrowTransform.SetParent(transform);
            arrowTransform.localScale = Vector3.one * arrowSize;
            arrowTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);
            arrowTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);

            arrow = arrowInstance.GetComponent<SliderArrow>();
            arrow.image.sprite = arrowImage;
            arrow.animationDraggable.Animator.runtimeAnimatorController = arrowAnimationController;

            var evenlySpacedPoints = settings.path.EvenlySpacedPoints();

            var pathDraggable = arrow.pathDraggable;
            pathDraggable.path = settings.path;
            pathDraggable.evenlySpacedPoints = evenlySpacedPoints;
            pathDraggable.dragDirection = PathDraggable.DragDirection.OnlyForward;
            pathDraggable.startPointIndex = 0;
            pathDraggable.accuracyThreshold = accuracyThreshold;

            pathDraggable.OnDragProgress += (accuracy, index, total) => {
                accumulatedAccuracy += accuracy;
                segments[index - 1].AnimateHit();
            };

            pathDraggable.OnDragStopped += (completed, index, total) => {
                if (completed) OnHit(accumulatedAccuracy / evenlySpacedPoints.Count);
                else OnMissed();
            };

            pathDraggable.UpdatePosition();
            pathDraggable.UpdateRotation();


            // Coroutine to create segments
            IEnumerator createSegments() {
                var segmentScale = Vector3.one * segmentSize;
                segments = new SliderSegment[evenlySpacedPoints.Count - 1];

                for (var i = 1; i < evenlySpacedPoints.Count; i++) {
                    var segmentInstance = new GameObject($"Segment {i}", typeof(SliderSegment));

                    var segment = segmentInstance.GetComponent<SliderSegment>();
                    var segmentTransform = segment.GetComponent<RectTransform>();

                    segmentTransform.SetParent(transform);

                    segmentTransform.localPosition = evenlySpacedPoints[i];
                    segmentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);
                    segmentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);
                    segmentTransform.localScale = segmentScale;
                    segmentTransform.SetAsFirstSibling();

                    segment.image.sprite = segmentImage;

                    segment.Animator.runtimeAnimatorController = segmentsAnimationController;

                    segments[i - 1] = segment;
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