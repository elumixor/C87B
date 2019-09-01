using System.Collections;
using System.Linq;
using Combo.DataContainers;
using Combo.Items.Slider.Arrow;
using Combo.Items.Slider.Segment;
using Shared.EditorScripts;
using Shared.Path;
using Shared.Path.PathDrag;
using UnityEditor;
using UnityEngine;

namespace Combo.Items.Slider {
    [DisallowMultipleComponent]
    public class ComboSlider : ComboItem<ComboSliderData> {
        public Path2D path;

        [SerializeField]
        private GameObject segmentObject;
        [SerializeField]
        private SliderArrow sliderArrow;

        [SerializeField, Range(0, 100)]
        private float maxDiffDistance = 10;

        [HideInInspector, SerializeField]
        private SliderSegment segmentComponent;

        private SliderArrow sliderArrowInstance;
        public PathDrag PathDrag { get; private set; }

        /// <summary>
        /// References to created segments
        /// </summary>
        private SliderSegment[] segments;

        /// <summary>
        /// Points where <see cref="segments"/> are created
        /// </summary>
        private Vector2[] evenlySpacedPoints;

        /// <summary>
        /// Alias to check if segments can be created1
        /// </summary>
        public bool CanCreateSegments => segmentComponent != null;

        /// <summary>
        /// Total accuracy across all <see cref="segments"/>
        /// </summary>
        private float totalAccuracy;

        /// <summary>
        /// Flag to check for destroyed children, to determine if self should be destroyed
        /// </summary>
        private bool shouldDestroy;

        /// <summary>
        /// Assign components and <see cref="maxDiffDistance"/> on script change
        /// </summary>
        private void OnValidate() {
            if (segmentObject != null) segmentComponent = segmentObject.GetComponent<SliderSegment>();
            if (PathDrag != null) PathDrag.maxDiffDistance = maxDiffDistance;
        }

        private void Update() {
            if (shouldDestroy && segments.All(i => i == null) && sliderArrowInstance == null) Destroy(gameObject);
        }

        /// <summary>
        /// Executes when slider arrow was successfully and accurately dragged to the end
        /// </summary>
        public override bool OnHit(float accuracy = 1f) {
            if (!base.OnHit(accuracy)) return false;

            sliderArrowInstance.animator.SetTrigger("Hit");
            return true;
        }

        /// <summary>
        /// Executes when slider arrow was not accurately dragged to the end of the slider
        /// </summary>
        public override bool OnMissed() {
            if (!base.OnMissed()) return false;

            sliderArrowInstance.animator.SetTrigger("Miss");
            foreach (var segment in segments) {
                if (segment != null) segment.animator.SetTrigger("Miss");
            }

            return shouldDestroy = true;
        }

        /// <summary>
        /// Start function creates evenly spaced <see cref="evenlySpacedPoints"/>,
        /// instantiates and configures slider segments alongside with slider arrow
        /// </summary>
        private void Start() {
            if (!Application.isPlaying) return;

            Requirements.Require(CanCreateSegments);

            evenlySpacedPoints = path.EvenlySpacedPoints().ToArray();

            sliderArrowInstance = (SliderArrow) PrefabUtility.InstantiatePrefab(sliderArrow, transform);
            var arrowTransform = sliderArrowInstance.transform;

            arrowTransform.localPosition = path.StartPosition;
            SetArrowRotation(arrowTransform, 0);

            #region Configure PathDrag component
            PathDrag = sliderArrowInstance.GetComponent<PathDrag>();

            PathDrag.path = path;
            PathDrag.maxDiffDistance = maxDiffDistance;

            PathDrag.OnDragProgress += (accuracy, index, total) => {
                if (index < total - 1) SetArrowRotation(sliderArrowInstance.transform, index);
                totalAccuracy += accuracy;
                segments[index].animator.SetTrigger("Hit");
            };
            PathDrag.OnDragStopped += (completed, index, total) => {
                if (completed) OnHit(totalAccuracy / evenlySpacedPoints.Length);
                else OnMissed();
            };
            #endregion

            // Coroutine to create segments
            IEnumerator createSegments() {
                var i = 0;
                segments = new SliderSegment[evenlySpacedPoints.Length];

                // Skip first point
                foreach (var p in evenlySpacedPoints.Skip(1)) {
                    var instance = (GameObject) PrefabUtility.InstantiatePrefab(segmentObject, transform);
                    instance.transform.localPosition = p;
                    instance.transform.SetAsFirstSibling();
                    var sliderSegment = instance.GetComponent<SliderSegment>();
                    segments[i++] = sliderSegment;

                    yield return null;
                }
            }

            StartCoroutine(createSegments());
        }

        /// <summary>
        /// Set rotation of slider arrow transform
        /// </summary>
        /// <param name="arrowTransform">Transform of slider arrow</param>
        /// <param name="currentPointIndex">Index of point, where the arrow is currently positioned</param>
        private void SetArrowRotation(Transform arrowTransform, int currentPointIndex) {
            arrowTransform.localRotation =
                Quaternion.FromToRotation(Vector3.up,
                    (Vector3) evenlySpacedPoints[currentPointIndex + 1] - arrowTransform.localPosition);
        }
        public override void OnSettingsChanged() {
            Debug.Log("OnSettingsChanged Slider");
        }
    }
}