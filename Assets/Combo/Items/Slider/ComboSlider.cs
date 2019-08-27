using System.Collections;
using System.Linq;
using Combo.Items.Slider.Arrow;
using Combo.Items.Slider.Segment;
using Shared.EditorScripts;
using Shared.Path;
using Shared.Path.PathDrag;
using UnityEditor;
using UnityEngine;

namespace Combo.Items.Slider {
    public class ComboSlider : ComboItem {
        public Path2D path;

        [SerializeField] private GameObject segmentObject;
        [SerializeField] private GameObject sliderArrow;

        [SerializeField, Range(0, 100)] private float maxDiffDistance = 10;

        [HideInInspector, SerializeField] private SliderSegment segmentComponent;

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
            AssignComponents();
            if (PathDrag != null) PathDrag.maxDiffDistance = maxDiffDistance;
        }

        private void Update() {
            if (shouldDestroy) {
                if (segments.All(i => i == null) && sliderArrowInstance == null) Destroy(gameObject);
            } else {
                if (segments != null)
                    foreach (var segment in segments) {
                        if (segment != null) segment.accent = accent;
                    }
            }
        }

        /// <summary>
        /// Executes when slider arrow was successfully and accurately dragged to the end
        /// </summary>
        public override void ItemHit(float accuracy = 1f) {
            base.ItemHit(accuracy);
            sliderArrowInstance.animationDetail.Hit();
        }

        /// <summary>
        /// Executes when slider arrow was not accurately dragged to the end of the slider
        /// </summary>
        public override void ItemMissed() {
            base.ItemMissed();
            sliderArrowInstance.animationDetail.Miss();
            foreach (var segment in segments) {
                if (segment != null) segment.animationDetail.Miss();
            }

            shouldDestroy = true;
        }

        /// <summary>
        /// Start function creates evenly spaced <see cref="evenlySpacedPoints"/>,
        /// instantiates and configures slider segments alongside with slider arrow
        /// </summary>
        private void Awake() {
            if (!Application.isPlaying) return;

            Requirements.Require(CanCreateSegments);

            evenlySpacedPoints = path.EvenlySpacedPoints().ToArray();

            var arrowInstance = (GameObject) PrefabUtility.InstantiatePrefab(sliderArrow, transform);
            var arrowTransform = arrowInstance.transform;

            arrowTransform.localPosition = path.StartPosition;
            SetArrowRotation(arrowTransform, 0);

            sliderArrowInstance = arrowInstance.GetComponent<SliderArrow>();
            SetPathDragComponent(arrowInstance);
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

        /// <summary>
        /// Configures <see cref="PathDrag"/> component
        /// </summary>
        /// <param name="arrowInstance">Slider arrow Game Object</param>
        private void SetPathDragComponent(GameObject arrowInstance) {
            PathDrag = arrowInstance.GetComponent<PathDrag>();

            PathDrag.path = path;
            PathDrag.maxDiffDistance = maxDiffDistance;

            PathDrag.OnDragProgress += (accuracy, index, total) => {
                if (index < total - 1) SetArrowRotation(arrowInstance.transform, index);
                totalAccuracy += accuracy;
                segments[index].animationDetail.Hit();
            };
            PathDrag.OnDragStopped += (completed, index, total) => {
                if (completed) ItemHit(totalAccuracy / evenlySpacedPoints.Length);
                else ItemMissed();
            };

            StartCoroutine(CreateSegments());
        }

        /// <summary>
        /// Coroutine to create slider segments in direction of drag
        /// </summary>
        private IEnumerator CreateSegments() {
            var i = 0;
            segments = new SliderSegment[evenlySpacedPoints.Length];

            foreach (var p in evenlySpacedPoints.Skip(1)) {
                var instance = (GameObject) PrefabUtility.InstantiatePrefab(segmentObject, transform);
                instance.transform.localPosition = p;
                instance.transform.SetAsFirstSibling();
                var sliderSegment = instance.GetComponent<SliderSegment>();
//                sliderSegment.accent = accent;
                segments[i++] = sliderSegment;

                yield return null;
            }
        }

        /// <summary>
        /// Alias function to assign components
        /// </summary>
        private void AssignComponents() {
            if (segmentObject != null) segmentComponent = segmentObject.GetComponent<SliderSegment>();
        }

        /// <summary>
        /// Inspector function to instantiate segments along the path
        /// </summary>
        public void InstantiateSegments() {
            if (!CanCreateSegments) return;

            evenlySpacedPoints = path.EvenlySpacedPoints().ToArray();
            foreach (var p in evenlySpacedPoints) {
                var instance = (GameObject) PrefabUtility.InstantiatePrefab(segmentObject, transform);
                instance.GetComponent<SliderSegment>().accent = accent;
                instance.transform.localPosition = p;
            }
        }
    }
}