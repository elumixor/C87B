using System.Linq;
using Shared.EditorScripts.CustomScopes;
using UnityEditor;
using UnityEngine;

namespace Meta {
    public class EditorShortcutsWindow : EditorWindow {
        [MenuItem("Custom/Windows/Shortcuts")]
        private static void CreateWindow() => GetWindow<EditorShortcutsWindow>(false, "Shortcuts").Show();

        private void OnGUI() {
            using (new HorizontalScope()) {
                using (new VerticalScope()) {
                    foreach (var (shortcutName, _, method, _) in CustomShortcutAttribute.ShortcutActions) {
                        if (GUILayout.Button(shortcutName.Split('/').Last())) method();
                    }
                }

                using (new VerticalScope()) {
                    foreach (var (_, hotkey, _, _) in CustomShortcutAttribute.ShortcutActions) {
                        GUILayout.Label(hotkey ?? "");
                    }
                }
            }
        }
    }
}