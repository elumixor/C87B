using Shared.EditorScripts;
using UnityEditor;

namespace Meta.Generate {
    public class CharacterPart : Popup {
        [MenuItem("Custom/Generate/Character Part")]
        [CustomShortcut]
        private static void Init() { }
    }
}