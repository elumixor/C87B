using System;
using System.Reflection;
using Combo.DataContainers;
using Combo.Items.Designers.Editors;
using Combo.Items.Editors;
using JetBrains.Annotations;
using Shared.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Combo.Items {
    public class ComboItemEditor<T> : Editor where T : ComboItemData {
        private ComboItem<T> comboItem;
        private ComboItemDataEditor<T> dataEditor;
        private FieldInfo field;
        private const string fieldName = "settings";

        protected virtual void OnEnable() {
            comboItem = (ComboItem<T>) target;
            field = typeof(ComboItem<T>).GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            dataEditor = field != null
                ? (ComboItemDataEditor<T>) CreateEditor((T) field.GetValue(target))
                : null;
            comboItem.OnSettingsChanged();
            Tools.hidden = true;
        }

        private void OnDisable() {
            Tools.hidden = false;
        }

        public override void OnInspectorGUI() {
            var oldValue = (T) field.GetValue(target);
            var newValue = EditorGUILayout.ObjectField(oldValue, typeof(T), false);

            if (oldValue != newValue) {
                field.SetValue(target, newValue);
                dataEditor = (ComboItemDataEditor<T>) CreateEditor((T) newValue);
                comboItem.OnSettingsChanged();
                Repaint();
            }

            if (dataEditor != null) {
                using (var changeScope = new EditorGUI.ChangeCheckScope()) {
                    dataEditor.OnInspectorGUI();
                    if (changeScope.changed) {
                        comboItem.OnSettingsChanged();
                        Repaint();
                    }
                }
            }

            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnSceneGUI() {
            if (dataEditor == null) return;

            using (var changeScope = new EditorGUI.ChangeCheckScope()) {
                dataEditor.OnSceneGUI();
                if (changeScope.changed) {
                    comboItem.OnSettingsChanged();
                    Repaint();
                }
            }
        }
    }
}