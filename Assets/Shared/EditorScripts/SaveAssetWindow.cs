using System;
using UnityEditor;
using UnityEngine;

namespace Shared.EditorScripts {
    public class SaveAssetWindow : EditorWindow {
        private string dir;
        private new string name;
        private ScriptableObject scriptableObject;

        private void OnGUI() {
            GUILayout.Label(dir);
            
            GUI.SetNextControlName("textField");
            name = GUILayout.TextField(name);
            GUI.FocusControl("textField");
            
            if (GUILayout.Button("Save")) scriptableObject.SaveNew(dir, name);

            var evt = Event.current;
            if (evt.keyCode == KeyCode.Escape) Close();
            if (evt.keyCode == KeyCode.Return) {
                scriptableObject.SaveNew(dir, name);
                Close();
            }
        }
        
        private void OnLostFocus() {
            Close();
        }

        public static void Init(ScriptableObject @object, string directory, string suggestedName) {
            var window = CreateInstance<SaveAssetWindow>();
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 250, 100);
            window.dir = directory;
            window.name = suggestedName;
            window.scriptableObject = @object;
            window.ShowPopup();
            window.Focus();
        }
    }
}