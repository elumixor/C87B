using System;
using Shared.EditorScripts.CustomScopes;
using Shared.GameObjectManagement;
using UnityEditor;
using UnityEngine;

namespace Combo.ComboItems.ComboButton {
    [CustomEditor(typeof(ComboButton))]
    public class ComboButtonEditor : Editor {
        private ComboButton comboButton;
        private Editor buttonDataEditor;
        private int marksCount;
        private Sprite sprite;

        private void OnEnable() {
            comboButton = (ComboButton) target;
            marksCount = comboButton.MarksCount;
            sprite = comboButton.MarkersContainer == null
                ? null
                : comboButton.MarkersContainer.GetChild(0).GetChild(0).GetComponent<SVGImage>().sprite;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var newMarksCount = EditorGUILayout.IntSlider(marksCount, 0, 10);
            if (newMarksCount != marksCount) {
                marksCount = newMarksCount;
                ResetMarks();
                comboButton.PopulateMarksList();
            }
        
            var newSprite = (Sprite) EditorGUILayout.ObjectField(sprite, typeof(Sprite), true);
            if (newSprite != sprite) {
                sprite = newSprite;
                UpdateSprite();
            }

            using (new HorizontalScope("Marks")) {
                if (GUILayout.Button("Populate List")) comboButton.PopulateMarksList();
//                if (GUILayout.Button("Reset Marks")) comboButton.ResetMarks();
            }

            using (new HorizontalScope("Settings")) {
                if (GUILayout.Button("Update")) {
                    /*comboButton.settings = comboButton.GetCurrentSettings();*/
                }

                if (GUILayout.Button("Save new")) {
                    ProjectWindowUtil.CreateAsset(comboButton.GetCurrentSettings(), "New combo button");
                }
            }
        }
        /// <summary>
        /// Destroys all marks and creates new
        /// </summary>
        private void ResetMarks() {
            if (comboButton.MarkersContainer == null) return;

            ChildrenManagement.DestroyAllImmediate(comboButton.MarkersContainer);

            for (var i = 0; i < marksCount; i++) {
                var markContainer = CreateMarkContainer($"Mark {i}");
                CreateMark(markContainer);
            }
        }
        /// <summary>
        /// Instantiates mark container to correctly rotate mark
        /// </summary>
        /// <param name="gameObjectName">Name of created <see cref="GameObject"/></param>
        /// <returns><see cref="Transform"/> of instantiated container</returns>
        private RectTransform CreateMarkContainer(string gameObjectName = "Mark Container") {
            var instance = new GameObject(gameObjectName, typeof(RectTransform)).GetComponent<RectTransform>();
            instance.SetParent(comboButton.MarkersContainer);
            return instance;
        }
        /// <summary>
        /// Instantiates mark sprite
        /// </summary>
        /// <param name="container">Container of current mark that is rotated</param>
        /// <param name="gameObjectName">Name of created <see cref="GameObject"/></param>
        /// <returns><see cref="Transform"/> of instantiated mark and reference to it's <see cref="SVGImage"/> component
        /// </returns>
        private void CreateMark(Transform container, string gameObjectName = "Mark") {
            var instance = new GameObject(gameObjectName, typeof(RectTransform));

            var rectTransform = instance.GetComponent<RectTransform>();
            rectTransform.SetParent(container);

            var svg = instance.AddComponent<SVGImage>();
            svg.sprite = sprite;
        }

        private void UpdateSprite() {
            throw new NotImplementedException();
        }
    }
}