using UnityEditor;
using UnityEngine;

namespace Shared.EditorScripts {
    public class Popup : EditorWindow {
        protected virtual void OnLostFocus() {
            Close();
        }

        protected KeyCode Code { get; private set; } 
        
        protected virtual void OnGUI() {
            var evt = Event.current;
            Code = evt.keyCode;
            if (Code == KeyCode.Escape) Close();
        }
    }
}