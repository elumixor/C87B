using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combo.ComboItems.ComboButton {
    [SelectionBase]
    [RequireComponent(typeof(Animator))]
    public class ComboButton : ComboItem, IPointerDownHandler {
        [Serializable]
        public new class Data : ComboItem.Data {
            [SerializeField]
            private Vector2 position; // Private backing field to be serializable
            public override Vector2 Position {
                get => position;
                set => position = value;
            }
        }
        /// <summary>
        /// Reference to container of transform containers (root of containers)
        /// </summary>
#pragma warning disable 0649
        [SerializeField] private Transform markersContainer;

        public Transform MarkersContainer => markersContainer;
#pragma warning restore 0649
        /// <summary>
        /// Distance of mark from center
        /// </summary>
        [Header("Marks settings")] [SerializeField, Range(0, 5)]
        private float markDistance;

        /// <summary>
        /// Rotation of all marks with origin in object's center (rotation of containers)
        /// </summary>
        [SerializeField, Range(0, 360)] public float offsetAngle;

        /// <summary>
        /// Individual marks rotation
        /// </summary>
        [Header("Individual mark settings")] [SerializeField, Range(-180, 180)]
        private float markRotation;

        /// <summary>
        /// Size of mark
        /// </summary>
        [SerializeField, Range(0f, 1f)] private float markSize;

        /// <summary>
        /// List of references to marks
        /// </summary>
        [HideInInspector, SerializeField] private List<Mark> marks;

        /// <summary>
        /// Minimum accuracy required to succeed with this combo item
        /// </summary>
        [SerializeField] private float thresholdAccuracy;

        /// <summary>
        /// Animation Detail for this segment
        /// </summary>
        [HideInInspector, SerializeField] private AnimationDetail animationDetail;

        /// <summary>
        /// Marks count
        /// </summary>
        public int MarksCount => marks?.Count ?? 0;

        /// <summary>
        /// Structure to conveniently hold references to mark data
        /// </summary>
        [Serializable]
        public struct Mark {
            /// <summary>
            /// Mark's container's transform
            /// </summary>
            public RectTransform container;

            /// <summary>
            /// Mark's transform
            /// </summary>
            public RectTransform mark;

            /// <summary>
            /// Mark's <see cref="SVGImage"/> component
            /// </summary>
            public SVGImage svg;
        }

        /// <summary>
        /// Checks accuracy on pointer down and decides if item succeeded with accuracy or failed
        /// </summary>
        public void OnPointerDown(PointerEventData eventData) {
            var rect = GetComponent<RectTransform>().rect.size * size;
            var accuracy = (eventData.pressPosition - (Vector2) transform.localPosition).magnitude / Mathf.Min(rect.x, rect.y);
            if (accuracy > thresholdAccuracy) ItemHit(accuracy);
            else ItemMissed();
        }

        /// <summary>
        /// Reflect script variables change
        /// </summary>
        private void Update() {
            UpdateView();
        }

        /// <summary>
        /// Used to reflect changes in script variables when edit mode
        /// </summary>
        private void OnValidate() {
            animationDetail = new AnimationDetail(GetComponent<Animator>());
            PopulateMarksList();
            UpdateView();
        }

        /// <summary>
        /// Updates view to reflect changes in script variables (e.g. used in animation)
        /// </summary>
        private void UpdateView() {
            transform.localScale = new Vector2(size, size);

            var angleBetweenMarks = 360 / marks.Count;
            for (var index = 0; index < marks.Count; index++) {
                var mark = marks[index];
                var angle = angleBetweenMarks * index + offsetAngle;
                SetMarkContainer(mark.container, angle);
                SetMark(mark.mark, mark.svg);
            }
        }

        /// <summary>
        /// Sets correct mark container transform
        /// </summary>
        /// <param name="container">Container's transform reference</param>
        /// <param name="angle">Container's clockwise rotation angle</param>
        private static void SetMarkContainer(RectTransform container, float angle) {
            container.localRotation = Quaternion.Euler(0f, 0f, angle);
            container.localPosition = Vector3.zero;
            container.localScale = Vector3.one;
            container.sizeDelta = Vector2.one;
        }

        /// <summary>
        /// Setts correct local mark transform and svg color
        /// </summary>
        /// <param name="mark">Mark's transform reference</param>
        /// <param name="svg">Mark's svg component</param>
        private void SetMark(RectTransform mark, Graphic svg) {
            mark.localPosition = new Vector3(0, markDistance, 0);
            mark.localRotation = Quaternion.Euler(0, 0, markRotation);
            mark.localScale = Vector2.one * markSize;
            mark.sizeDelta = Vector2.one;
            svg.color = accent;
        }

        /// <summary>
        /// Populates <see cref="marks"/> with valid references to children, does not create or modify child
        /// game objects or transforms.
        /// </summary>
        public void PopulateMarksList() {
            marks = new List<Mark>(markersContainer.childCount);

            foreach (Transform markContainer in markersContainer) {
                var mark = markContainer.GetChild(0);
                marks.Add(new Mark {
                    container = markContainer.GetComponent<RectTransform>(),
                    mark = mark.GetComponent<RectTransform>(),
                    svg = mark.GetComponent<SVGImage>()
                });
            }
        }

        public ComboButtonSettings GetCurrentSettings() {
            var newSettings = ScriptableObject.CreateInstance<ComboButtonSettings>();

            newSettings.size = size;
            newSettings.markDistance = markDistance;
            newSettings.markRotation = markRotation;
            newSettings.offsetAngle = offsetAngle;
            newSettings.markUniformScale = markSize;

            newSettings.marksCount = marks.Count;

            if (marks.Count > 0) newSettings.markSprite = marks[0].svg.sprite;

            return newSettings;
        }
        
        /// <inheritdoc/>
        public override void ItemHit(float accuracy = 1f) {
            base.ItemHit(accuracy);
            animationDetail.Hit();
        }

        /// <inheritdoc/>
        public override void ItemMissed() {
            base.ItemMissed();
            animationDetail.Miss();
        }
    }
}