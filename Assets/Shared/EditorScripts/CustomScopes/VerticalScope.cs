using UnityEngine;

namespace Shared.EditorScripts.CustomScopes {
    public class VerticalScope : CustomEditorScope {
        public VerticalScope() : base(() => GUILayout.BeginVertical(), GUILayout.EndVertical) { }
        public VerticalScope(string scopeName = null) : base(() => {
            if (scopeName != null) GUILayout.Label(scopeName);
            GUILayout.BeginVertical();
        }, GUILayout.EndVertical) { }
    }
}