using System;
using System.Linq;
using Shared.EditorScripts;
using UnityEditor;
using UnityEngine;

namespace Meta {
    public class GenerateWindow : Popup {
        private static (string name, Action method)[] shortcuts;

        private AutocompleteSearchField autocompleteSearchField;

        [MenuItem("Window/Generate %n")]
        [CustomShortcut(Hotkey = "Ctrl + N")]
        private static void Init() {
            shortcuts = CustomShortcutAttribute.ShortcutActions
                .Where(s => s.mode == CustomShortcutAttribute.ShortcutMode.Generate)
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
            autocompleteSearchField.ClearResults();
            if (!string.IsNullOrEmpty(searchString))
                foreach (var (shortcutName, _) in shortcuts) {
                    var result = shortcutName.Split('/').Last();
                    if (result != autocompleteSearchField.searchString)
                        autocompleteSearchField.AddResult(result);
                }
        }

        private void OnConfirm(string result) {
            foreach (var (shortcutName, method) in shortcuts)
                if (shortcutName.Split('/').Last() == result) {
                    method();
                    Close();
                    return;
                }
        }


        protected override void OnGUI() {
            autocompleteSearchField.OnGUI();

            if (autocompleteSearchField.results.Count > 0 && autocompleteSearchField.selectedIndex == -1)
                autocompleteSearchField.selectedIndex = 0;
            if (Code == KeyCode.Return) {
                if (autocompleteSearchField.selectedIndex != -1)
                    OnConfirm(autocompleteSearchField.results[autocompleteSearchField.selectedIndex]);
            }

            base.OnGUI();
        }
    }
}