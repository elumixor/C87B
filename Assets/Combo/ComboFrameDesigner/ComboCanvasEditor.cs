using System;
using System.Collections.Generic;
using System.Linq;
using Combo.ComboItems;
using Combo.ComboItems.ComboButton;
using Combo.ComboItems.ComboSlider;
using Shared.EditorScripts.CustomScopes;
using Shared.Path;
using UnityEditor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Combo.ComboFrameDesigner {
    [CustomEditor(typeof(ComboDesigner))]
    public class ComboCanvasEditor : Editor {
        private ComboDesigner designer;
        private Animator animator;

        private float fadeInTime;
        private AnimationClip clip;

        private const float SliderPointRadius = 10f;

        private List<Tuple<Editor, bool>> editors;

        private Color orderedColor = Color.green;
        private Color unorderedColor = Color.yellow;
        private Color simultaneousColor = Color.red;

        private Color ColorByType(Domain.ComboFrame.ComboFrameType type) {
            switch (type) {
                case Domain.ComboFrame.ComboFrameType.Ordered:
                    return orderedColor;
                case Domain.ComboFrame.ComboFrameType.Unordered:
                    return unorderedColor;
                default:
                    return simultaneousColor;
            }
        }

        private void OnEnable() {
            designer = (ComboDesigner) target;
            if (designer.frameSettings) designer.AssignFromSettings(designer.frameSettings);
            editors = designer.items
                .Select(item => {
                    if (item is ComboButton.Data button) return CreateEditor(button);
                    if (item is ComboSlider.Data slider)
                        return (ComboSliderEditor) CreateEditor(slider, typeof(ComboSliderEditor));

                    return null;
                })
                .Where(editor => editor != null)
                .Select(editor => new Tuple<Editor, bool>(editor, true))
                .ToList();
        }

        private void OnDisable() {
            designer.frameSettings = designer.GetCurrentSettings();
        }

        // Scene GUI
        private void OnSceneGUI() {
            Handles.color = ColorByType(designer.frameType);
            
            var displayedItems = designer.DisplayedItems;

            foreach (var item in displayedItems) {
                switch (item) {
                    case ComboSlider.Data slider: {
                        DisplaySliderHandle(slider);
                        break;
                    }

                    case ComboButton.Data button:
                        DisplayButtonHandle(button);
                        break;
                }

                var newPos = (Vector2) Handles.FreeMoveHandle(item.Position, Quaternion.identity, SliderPointRadius,
                    Vector3.one,
                    Handles.CircleHandleCap);
                if (newPos != item.Position) {
                    Undo.RecordObject(item, "Change Item position");
                    item.Position = newPos;
                }

                if (designer.frameType == Domain.ComboFrame.ComboFrameType.Ordered)
                    Handles.color *= new Color(1, 1, 1, 1f - 1f / designer.items.Count);
            }

            if (designer.frameType == Domain.ComboFrame.ComboFrameType.Ordered) {
                Handles.DrawAAPolyLine(5, displayedItems.Select(x => (Vector3) x.Position).ToArray());

                var start = displayedItems[0];
                Handles.DrawSolidArc(start.Position,
                    Vector3.forward, Vector3.right, 360, 10);
            }
        }

        /// <summary>
        /// Scene GUI for Slider
        /// </summary>
        private static void DisplaySliderHandle(ComboSlider.Data slider) {
            for (var i = 0; i < Path2D.Length; i++) {
                var newPos = (Vector2) Handles.FreeMoveHandle(slider.path[i], Quaternion.identity, SliderPointRadius,
                    Vector3.one,
                    Handles.CircleHandleCap);

                if (newPos != slider.path[i]) {
                    Undo.RecordObject(slider, "Change Slider control point");
                    slider.path[i] = newPos;
                }

                var path = slider.path;
                Handles.DrawBezier(path.StartPosition, path.EndPosition, path.StartTangent, path.EndTangent,
                    Color.green, null, 2f);
                Handles.DrawLine(path.StartPosition, path.StartTangent);
                Handles.DrawLine(path.EndPosition, path.EndTangent);
            }

            var newSize = Handles.RadiusHandle(Quaternion.identity, slider.path.StartPosition, slider.size);
            if (newSize != slider.size) {
                Undo.RecordObject(slider, "Change Slider arrow size");
                slider.size = newSize;
            }
        }

        /// <summary>
        /// Scene GUI for button
        /// </summary>
        private static void DisplayButtonHandle(ComboItem.Data button) {
            var newSize = Handles.RadiusHandle(Quaternion.identity, button.Position, button.size);
            if (newSize != button.size) {
                Undo.RecordObject(button, "Change Button size");
                button.size = newSize;
            }
        }

        // Inspector
        public override void OnInspectorGUI() {
            LoadSettingsSection();
            GUILayout.Space(10);

            BackgroundSection();
            GUILayout.Space(10);

            FadeInSection();
            GUILayout.Space(10);

            ColorSection();
            GUILayout.Space(10);

            FrameTypeSection();
            GUILayout.Space(10);

            ItemsSection();
            GUILayout.Space(10);

            ItemsAddSection();
            GUILayout.Space(10);

            SaveSettingsSection();
            GUILayout.Space(10);

            CreateFrameSection();
        }

        private void CreateFrameSection() {
            var newButtonPrefab = (ComboButton) EditorGUILayout.ObjectField("Button Prefab",
                designer.buttonPrefab, typeof(ComboButton), false);
            if (designer.buttonPrefab != newButtonPrefab) {
                Undo.RecordObject(designer, "Change Button Prefab");
                designer.buttonPrefab = newButtonPrefab;
            }

            var newSliderPrefab = (ComboSlider) EditorGUILayout.ObjectField("Slider Prefab",
                designer.sliderPrefab, typeof(ComboSlider), false);
            if (designer.sliderPrefab != newSliderPrefab) {
                Undo.RecordObject(designer, "Change Slider Prefab");
                designer.sliderPrefab = newSliderPrefab;
            }

            var canCreateFrame = designer.buttonPrefab != null && designer.sliderPrefab != null;

            if (!canCreateFrame)
                EditorGUILayout.HelpBox("Assign Combo Button and Combo Slider prefabs to create Combo Frame",
                    MessageType.Warning);

            GUI.enabled = canCreateFrame;
            if (GUILayout.Button("Create Frame")) designer.CreateFrame();

            GUI.enabled = true;
        }

        private void LoadSettingsSection() {
            var newSettings =
                (Domain.ComboFrame) EditorGUILayout.ObjectField("Settings", designer.frameSettings, typeof(Domain.ComboFrame), true);
            if (newSettings == designer.frameSettings) return;

            Undo.RecordObject(designer, "Assign settings");
            designer.frameSettings = newSettings;
            designer.AssignFromSettings(newSettings);
        }

        private void BackgroundSection() {
            var newBackground = (Image) EditorGUILayout.ObjectField(designer.background, typeof(Image), true);
            if (designer.background != newBackground) {
                Undo.RecordObject(designer, "Change background");
                designer.background = newBackground;
            }

            GUILayout.Space(10);
            var newColor = EditorGUILayout.ColorField("Background Color", designer.background.color);
            if (designer.background.color != newColor) {
                Undo.RecordObject(designer.background, "Change background color");
                designer.background.color = newColor;
            }
        }

        private void FadeInSection() {
            var newExecution = EditorGUILayout.Slider("Execution time", designer.executionTime, 0, 1);
            if (designer.executionTime != newExecution) {
                Undo.RecordObject(designer, "Change execution time");
                designer.executionTime = newExecution;
            }
        }

        private void FrameTypeSection() {
            var newType = (Domain.ComboFrame.ComboFrameType) EditorGUILayout.EnumPopup("Frame type", designer.frameType);
            if (designer.frameType != newType) {
                Undo.RecordObject(designer, "Change Frame type");
                designer.frameType = newType;
            }
        }

        private void ColorSection() {
            GUILayout.Label("Combo Colors", new GUIStyle(EditorStyles.boldLabel));

            using (new EditorGUI.IndentLevelScope()) {
                unorderedColor = EditorGUILayout.ColorField("Unordered", unorderedColor);
                orderedColor = EditorGUILayout.ColorField("Ordered", orderedColor);
                simultaneousColor = EditorGUILayout.ColorField("Simultaneous", simultaneousColor);
                var colors =
                    (ComboColors.ComboColors) EditorGUILayout.ObjectField("Set from settings", null,
                        typeof(ComboColors.ComboColors), true);
                if (colors != null) {
                    unorderedColor = colors.unordered;
                    orderedColor = colors.ordered;
                    simultaneousColor = colors.simultaneous;
                }
            }
        }

        /// <summary>
        /// Displays list of ComboItems
        /// </summary>
        private void ItemsSection() {
            if (editors == null) return;

            int? editorToRemove = null;

            var index = 0;
            var shownList = editors.Select(e => e.Item2).ToList();

            foreach (var (editor, shown) in editors) {
                if (!designer.IsDisplayed((ComboItem.Data) editor.target)) continue;

                var isSlider = editor.target is ComboSlider;

                var isShown = EditorGUILayout.Foldout(shown,
                    $"{(isSlider ? "Slider" : "Button")} {index}",
                    true, new GUIStyle(EditorStyles.foldout) {fontStyle = FontStyle.Bold});

                if (isShown) {
                    using (new EditorGUI.IndentLevelScope()) {
                        editor.OnInspectorGUI();
                    }
                }

                if (GUILayout.Button("Remove Item")) {
                    Undo.RecordObject(designer, "Remove Item");
                    designer.items.RemoveAt(index);
                    editorToRemove = index;
                }

                shownList[index] = isShown;

                index++;
            }

            for (var i = 0; i < shownList.Count; i++) {
                editors[i] = new Tuple<Editor, bool>(editors[i].Item1, shownList[i]);
            }

            if (editorToRemove != null) editors.RemoveAt(editorToRemove.Value);
        }

        private void ItemsAddSection() {
            using (new HorizontalScope()) {
                GUI.enabled = designer.frameType != Domain.ComboFrame.ComboFrameType.SimultaneousSlider;

                if (GUILayout.Button("Add Button")) {
                    var button = CreateInstance<ComboButton.Data>();
                    var found = designer.items.FindLast(i => i is ComboButton.Data);
                    if (found != null) {
                        button.Position = found.Position + new Vector2(15f, -15f);
                        button.size = found.size;
                    }

                    Undo.RecordObjects(new UnityEngine.Object[] {designer, this}, "Add button");

                    designer.items.Add(button);
                    editors.Add(new Tuple<Editor, bool>(CreateEditor(button), true));
                }

                GUI.enabled = designer.frameType != Domain.ComboFrame.ComboFrameType.SimultaneousButton;

                if (GUILayout.Button("Add Slider")) {
                    var slider = CreateInstance<ComboSlider.Data>();
                    var found = designer.items.FindLast(i => i is ComboSlider.Data) as ComboSlider.Data;
                    if (found != null) {
                        slider.path = new Path2D(found.path);
                        slider.Position = found.Position + new Vector2(15f, -15f);
                    }

                    Undo.RecordObjects(new UnityEngine.Object[] {designer, this}, "Add slider");

                    designer.items.Add(slider);
                    editors.Add(new Tuple<Editor, bool>(CreateEditor(slider), true));
                }

                GUI.enabled = true;
            }
        }

        private void SaveSettingsSection() {
            if (GUILayout.Button("Save as new"))
                ProjectWindowUtil.CreateAsset(designer.GetCurrentSettings(),
                    "Assets/Scripts/Domain/Combo/ComboFrames/New combo frame.asset");
        }
    }
}