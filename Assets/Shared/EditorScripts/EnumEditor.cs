using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Shared.EditorScripts {
    public abstract class EnumScriptableObject<TEnum, TResult> : ScriptableObject where TEnum : Enum {
        [SerializeField] protected TResult[] values;
        public TResult this[TEnum targetType] => targetType.ArrayValueIn(values);
    }

    public abstract class EnumMonoBehaviour<TEnum, TResult> : MonoBehaviour where TEnum : Enum {
        [SerializeField] protected TResult[] values;
        public TResult this[TEnum targetType] => targetType.ArrayValueIn(values);
    }

    public abstract class EnumEditor<TEnum, TResult> : Editor where TEnum : Enum {
        protected virtual string FieldName { get; } = "values";
        private Type targetType;
        private static TEnum[] EnumValues => (TEnum[]) Enum.GetValues(typeof(TEnum));
        private SerializedProperty property;

        protected virtual void OnEnable() {
            targetType = target.GetType();
            
            var field = targetType.GetField(FieldName,
                BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (field == null) throw new ArgumentNullException(FieldName);
            
            var arr = (TResult[]) field.GetValue(target);
            if (arr == null || arr.Length == 0) field.SetValue(target, new TResult[EnumValues.Length]);

            property = serializedObject.FindProperty(FieldName); 
        }

        public override void OnInspectorGUI() {
            var ev = EnumValues;
            for (var i = 0; i < ev.Length; i++)
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(ev[i].ToString()), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}