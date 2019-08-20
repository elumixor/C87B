using UnityEngine;

namespace Shared.EditorScripts.CustomScopes {
    public class HorizontalScope : CustomEditorScope {
        public HorizontalScope(string scopeName = null) : base(() => {
            if (scopeName != null) GUILayout.Label(scopeName);
            GUILayout.BeginHorizontal();
        }, GUILayout.EndHorizontal) { }
    }
}