using System.Reflection;
using UnityEditor;

namespace Character.EditorScripts {
    [CustomEditor(typeof(Character), true, isFallback = true)]
    public class CharacterEditor : Editor {
        private Character character;
        private bool displayTraits = true;
        private bool displayParts = true;
        private PropertyInfo characterPartsProperty;

        private void OnEnable() {
            character = (Character) target;
            characterPartsProperty = typeof(Character).GetProperty("parts", BindingFlags.Instance | BindingFlags.NonPublic);
        }


        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
//            var traits = character.ProvidedTraits.ToArray();
//            var traitsValues = character.GetTraits<object>().ToArray();
//
//            var parts = (CharacterPart[]) characterPartsProperty.GetValue(character);
//
//            displayParts = EditorGUILayout.Foldout(displayParts && parts.Length > 0, $"Parts ({parts.Length})");
//
//            if (displayParts) {
//                foreach (var part in parts) {
//                    GUILayout.Label($"{part.GetType().Name} ({part.Name})");
//                }
//            }
//
//            displayTraits = EditorGUILayout.Foldout(displayTraits && traits.Length > 0, $"Traits ({traits.Length})", true);
//
//            if (displayTraits) {
//                for (var index = 0; index < traits.Length; index++) {
//                    var trait = traits[index];
//                    GUILayout.Label($"{trait.Name} ({traitsValues[index]})");
//                }
//            }
        }
    }
}