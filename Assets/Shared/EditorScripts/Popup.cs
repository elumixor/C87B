using UnityEditor;
using UnityEngine;

namespace Shared.EditorScripts {
    public class Popup : EditorWindow {
        protected virtual void OnLostFocus() {
            Close();
        }

        protected KeyCode code { get; private set; } 
        
        protected virtual void OnGUI() {
            var evt = Event.current;
            code = evt.keyCode;
            if (code == KeyCode.Escape) Close();
        }
    }
}