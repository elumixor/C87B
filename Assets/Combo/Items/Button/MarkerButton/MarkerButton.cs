using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        private Transform markersContainer;

        /// <summary>
        /// Distance of mark from center
        /// </summary>
        [Header("Marks settings")]
        [SerializeField, Range(0, 5)]
        private float markDistance;

        /// <summary>
        /// Rotation of all marks with origin in object's center (rotation of containers)
        /// </summary>
        [SerializeField, Range(0, 360)]
        public float offsetAngle;

        /// <summary>
        /// Individual marks rotation
        /// </summary>
        [Header("Individual mark settings")]
        [SerializeField, Range(-180, 180)]
        private float markRotation;

        /// <summary>
        /// Size of mark
        /// </summary>
        [SerializeField, Range(0f, 1f)]
        private float markSize;

        /// <summary>
        /// List of references to marks
        /// </summary>
        [HideInInspector, SerializeField]
        private List<Mark> marks;

        /// <summary>
        /// Marks count
        /// </summary>
        public int MarksCount => marks?.Count ?? 0;

        private float size;

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
    }
}