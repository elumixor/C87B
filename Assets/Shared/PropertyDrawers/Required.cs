using System;
using UnityEditor;
using UnityEngine;

namespace Shared.PropertyDrawers {
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(RequiredAttribute), true)]
    public class RequiredAttributePropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label, true);
            var propertyHeight = EditorGUI.GetPropertyHeight(property);
            var spacing =  EditorGUIUtility.standardVerticalSpacing;

            if (property.objectReferenceValue == null)
                EditorGUI.HelpBox(
                    new Rect(position.x, position.y + propertyHeight + spacing, position.width,
                        position.height - propertyHeight - 2 * spacing),
                    $"{property.displayName} should be assigned", MessageType.Error);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUI.GetPropertyHeight(property) * (property.objectReferenceValue == null ? 3f : 1f);
    }
}