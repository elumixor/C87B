using System;
using System.Linq;
using Shared.EditorScripts;
using UnityEditor;
using UnityEngine;

namespace Meta {
    public class GenerateWindow : Popup {
        private static (string name, Action method)[] shortcuts;

        private AutocompleteSearchField autocompleteSearchField;

        [MenuItem("Custom/Generate %n")]
        [CustomShortcut(Hotkey = "Ctrl + N")]
        private static void Init() {
            shortcuts = CustomShortcutAttribute.ShortcutActions
                .Where(s => s.mode == CustomShortcutAttribute.ShortcutMode.General)
                .Select(s => (s.name, s.method))
                .ToArray();
            var window = CreateInstance<GenerateWindow>();
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 250, 500);
            window.ShowPopup();
            window.Focus();
        }

        private void OnEnable() {
            autocompleteSearchField = new AutocompleteSearchField();
            autocompleteSearchField.onInputChanged += OnInputChanged;
            autocompleteSearchField.onConfirm += OnConfirm;
            autocompleteSearchField.Focus();
        }

        private void OnInputChanged(string searchString) {
            Debug.Log(searchString);
            Debug.Log(autocompleteSearchField.searchString);

            autocompleteSearchField.ClearResults();
            if (!string.IsNullOrEmpty(searchString))
                foreach (var (shortcutName, _) in shortcuts) {
                    var result = shortcutName.Split('/').Last();
                    if (result != autocompleteSearchField.searchString)
                        autocompleteSearchField.AddResult(result);
                }
        }

        private void OnConfirm(string result) {
            Debug.Log(result);
            Debug.Log(autocompleteSearchField.searchString);

            foreach (var (shortcutName, method) in shortcuts) {
                if (shortcutName.Split('/').Last() == result) method();
                Close();
                return;
            }
        }


        protected override void OnGUI() {
            base.OnGUI();
            GUILayout.Label(shortcuts.Length.ToString());
//            base.OnGUI();
//            GUI.SetNextControlName("textField");
//            input = GUILayout.TextField(input);
//            GUI.FocusControl("textField");
//
            autocompleteSearchField.OnGUI();
//
//            using (var scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition, GUILayout.Width(250), GUILayout.Height(100))) {
//                scrollPosition = scrollViewScope.scrollPosition;
//                
//                GUILayout.Label("1");
//                GUILayout.Label("2");
//                GUILayout.Label("3");
//                GUILayout.Label("4");
//                GUILayout.Label("5");
//                GUILayout.Label("6");
//                GUILayout.Label("7");
//                GUILayout.Label("8");
//                GUILayout.Label("9");
//                GUILayout.Label("10");
//            }
            if (autocompleteSearchField.results.Count > 0 && autocompleteSearchField.selectedIndex == -1)
                autocompleteSearchField.selectedIndex = 0;
            if (code == KeyCode.Return) {
                if (autocompleteSearchField.selectedIndex != -1)
                    OnConfirm(autocompleteSearchField.results[autocompleteSearchField.selectedIndex]);
            }
        }
    }
}