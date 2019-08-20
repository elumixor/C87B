using System;
using System.Reflection;
using Character.HP;
using UnityEditor;
using UnityEngine;

namespace Scenes.FightScene.HPDisplay.DisplayData {
    [CustomEditor(typeof(HPDataManager))]
    public class HPDataManagerEditor : Editor {
        private HPDataManager manager;
        private const string FieldName = "values";
        private static HPType[] enumValues => (HPType[]) Enum.GetValues(typeof(HPType));


        private void OnEnable() {
            manager = (HPDataManager) target;
            var field = typeof(HPDataManager).GetField(FieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            var arr = (HPDisplayData[]) field.GetValue(manager);
            if (arr == null || arr.Length == 0) field.SetValue(manager, new HPDisplayData[enumValues.Length]);
        }

        public override void OnInspectorGUI() {
            var values = serializedObject.FindProperty("values");
            var ev = enumValues;
            for (var i = 0; i < ev.Length; i++)
                EditorGUILayout.PropertyField(values.GetArrayElementAtIndex(i), new GUIContent(ev[i].ToString()), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}