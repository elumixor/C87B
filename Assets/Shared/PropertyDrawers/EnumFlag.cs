using UnityEditor;
using UnityEngine;

namespace Shared.PropertyDrawers {
    public class EnumFlagAttribute : PropertyAttribute {
        public readonly bool required;
        public EnumFlagAttribute(bool required = false) {
            this.required = required;
        }
    }
    
    [CustomPropertyDrawer(typeof(EnumFlagAttribute))]
    public class EnumFlagAttributeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var buttonsIntValue = 0;
            var enumLength = property.enumNames.Length;
            var buttonPressed = new bool[enumLength];
            var buttonWidth = (position.width - EditorGUIUtility.labelWidth) / enumLength;

            EditorGUI.LabelField(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height), label);

            var required = ((EnumFlagAttribute) attribute).required;

            EditorGUI.BeginChangeCheck();

            for (var i = 0; i < enumLength; i++) {
                if ((property.intValue & (1 << i)) == 1 << i) buttonPressed[i] = true;

                var buttonPos = new Rect(position.x + EditorGUIUtility.labelWidth + buttonWidth * i,
                    position.y, buttonWidth, position.height);

                buttonPressed[i] = GUI.Toggle(buttonPos, buttonPressed[i], property.enumNames[i], "Button");

                if (buttonPressed[i]) buttonsIntValue += 1 << i;
            }

            if (EditorGUI.EndChangeCheck()) property.intValue = required && buttonsIntValue == 0 ? 1 : buttonsIntValue;
        }
    }
}