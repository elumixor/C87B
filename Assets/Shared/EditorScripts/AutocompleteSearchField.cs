using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Shared.EditorScripts {
    [Serializable]
    public class AutocompleteSearchField {
        private static class Styles {
            public const float ResultHeight = 20f;
            public const float ResultsBorderWidth = 2f;
            public const float ResultsMargin = 15f;
            public const float ResultsLabelOffset = 2f;

            public static readonly GUIStyle EntryEven = new GUIStyle("CN EntryBackEven");
            public static readonly GUIStyle EntryOdd = new GUIStyle("CN EntryBackOdd");

            public static readonly GUIStyle LabelStyle = new GUIStyle(EditorStyles.label) {
                alignment = TextAnchor.MiddleLeft,
                richText = true
            };

            public static readonly GUIStyle ResultsBorderStyle = new GUIStyle("hostview");
        }

        public Action<string> onInputChanged;
        public Action<string> onConfirm;
        public string searchString;
        public int maxResults = 15;

        [SerializeField] public List<string> results = new List<string>();

        [SerializeField] public int selectedIndex = -1;

        private SearchField searchField;

        private Vector2 previousMousePosition;
        private bool selectedIndexByMouse;

        private bool showResults;

        [PublicAPI]
        public void AddResult(string result) => results.Add(result);

        [PublicAPI]
        public void ClearResults() => results.Clear();

        [PublicAPI]
        public void OnToolbarGui() => Draw(true);

        [PublicAPI]
        public void OnGUI() => Draw(false);

        private void Draw(bool asToolbar) {
            var rect = GUILayoutUtility.GetRect(1, 1, 18, 18, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal();
            DoSearchField(rect, asToolbar);
            GUILayout.EndHorizontal();
            rect.y += 18;
            DoResults(rect);
        }

        public AutocompleteSearchField() {
                searchField = new SearchField();
                searchField.downOrUpArrowKeyPressed += OnDownOrUpArrowKeyPressed;
        }

        private void DoSearchField(Rect rect, bool asToolbar) {
            var result = asToolbar
                ? searchField.OnToolbarGUI(rect, searchString)
                : searchField.OnGUI(rect, searchString);

            if (result != searchString && onInputChanged != null) {
                onInputChanged(result);
                selectedIndex = -1;
                showResults = true;
            }

            searchString = result;

            if (HasSearchBarFocused()) {
                RepaintFocusedWindow();
            }
        }

        private void OnDownOrUpArrowKeyPressed() {
            var current = Event.current;

            if (current.keyCode == KeyCode.UpArrow) {
                current.Use();
                selectedIndex--;
                selectedIndexByMouse = false;
            } else {
                current.Use();
                selectedIndex++;
                selectedIndexByMouse = false;
            }

            if (selectedIndex >= results.Count) selectedIndex = results.Count - 1;
            else if (selectedIndex < 0) selectedIndex = -1;
        }

        private void DoResults(Rect rect) {
            if (results.Count <= 0 || !showResults) return;

            var current = Event.current;
            rect.height = Styles.ResultHeight * Mathf.Min(maxResults, results.Count);
            rect.x = Styles.ResultsMargin;
            rect.width -= Styles.ResultsMargin * 2;

            var elementRect = rect;

            rect.height += Styles.ResultsBorderWidth;
            GUI.Label(rect, "", Styles.ResultsBorderStyle);

            var mouseIsInResultsRect = rect.Contains(current.mousePosition);

            if (mouseIsInResultsRect) {
                RepaintFocusedWindow();
            }

            var movedMouseInRect = previousMousePosition != current.mousePosition;

            elementRect.x += Styles.ResultsBorderWidth;
            elementRect.width -= Styles.ResultsBorderWidth * 2;
            elementRect.height = Styles.ResultHeight;

            var didJustSelectIndex = false;

            for (var i = 0; i < results.Count && i < maxResults; i++) {
                if (current.type == EventType.Repaint) {
                    var style = i % 2 == 0 ? Styles.EntryOdd : Styles.EntryEven;

                    style.Draw(elementRect, false, false, i == selectedIndex, false);

                    var labelRect = elementRect;
                    labelRect.x += Styles.ResultsLabelOffset;
                    GUI.Label(labelRect, results[i], Styles.LabelStyle);
                }

                if (elementRect.Contains(current.mousePosition)) {
                    if (movedMouseInRect) {
                        selectedIndex = i;
                        selectedIndexByMouse = true;
                        didJustSelectIndex = true;
                    }

                    if (current.type == EventType.MouseDown) OnConfirm(results[i]);
                }

                elementRect.y += Styles.ResultHeight;
            }

            if (current.type == EventType.Repaint && !didJustSelectIndex && !mouseIsInResultsRect && selectedIndexByMouse)
                selectedIndex = -1;

            if (GUIUtility.hotControl != searchField.searchFieldControlID && GUIUtility.hotControl > 0
                || current.rawType == EventType.MouseDown && !mouseIsInResultsRect) showResults = false;

            if (current.type == EventType.KeyUp && current.keyCode == KeyCode.Return && selectedIndex >= 0)
                OnConfirm(results[selectedIndex]);

            if (current.type == EventType.Repaint) previousMousePosition = current.mousePosition;
        }

        private void OnConfirm(string result) {
            searchString = result;
            onConfirm?.Invoke(result);
            onInputChanged?.Invoke(result);
            RepaintFocusedWindow();
            GUIUtility.keyboardControl = 0; // To avoid Unity sometimes not updating the search field text
        }

        private bool HasSearchBarFocused() {
            return GUIUtility.keyboardControl == searchField.searchFieldControlID;
        }

        private static void RepaintFocusedWindow() {
            if (EditorWindow.focusedWindow != null) EditorWindow.focusedWindow.Repaint();
        }

        public void Focus() {
            searchField.SetFocus();
        }
    }
}