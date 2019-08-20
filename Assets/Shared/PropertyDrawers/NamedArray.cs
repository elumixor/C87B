using UnityEditor;
using UnityEngine;

namespace Shared.PropertyDrawers {
    public class NamedArrayAttribute : PropertyAttribute {
        public readonly string prefix;

        public NamedArrayAttribute(string prefix) {
            this.prefix = prefix;
        }
    }

    [CustomPropertyDrawer(typeof(NamedArrayAttribute))]
    public class NamedArrayDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            var pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            EditorGUI.PropertyField(rect, property,
                new GUIContent($"{((NamedArrayAttribute) attribute).prefix} {pos}"), true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}