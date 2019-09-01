using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Shared.Behaviours {
    public class Designer<T> : MonoBehaviour where T : ScriptableObject {
        public T itemData;

        protected void Reset() {
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.position = Vector3.zero;
            var rectTransformRect = rectTransform.rect;
            rectTransformRect.width = 0;
            rectTransformRect.height = 0;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.pivot = Vector2.zero;
            rectTransform.rotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
        }
    }

    public class DesignerEditor<T> : Editor where T : ScriptableObject {
        protected Designer<T> designer;
        protected T itemData;
        protected Action onSceneGUI;
        private Editor itemDataEditor;

        protected virtual void OnEnable() {
            designer = (Designer<T>) target;
            if (designer.itemData == null) designer.itemData = CreateInstance<T>();
            itemData = designer.itemData;
            itemDataEditor = CreateEditor(itemData);
            var methodInfo = itemDataEditor.GetType()
                .GetMethod("OnSceneGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            onSceneGUI = methodInfo != null
                ? (Action) Delegate.CreateDelegate(typeof(Action), itemDataEditor, methodInfo)
                : () => { };
        }

        public override void OnInspectorGUI() {
            var newData = (T) EditorGUILayout.ObjectField("Data", designer.itemData, typeof(T), true);
            if (newData != itemData) {
                designer.itemData = itemData = newData;
            }

            if (!itemData.IsSavedFile()) {
                EditorGUILayout.HelpBox("Asset file does not exist", MessageType.Warning);
                if (GUILayout.Button("Save to file")) itemData.SaveDialog(DirectoryName, FileName);
            } else if (GUILayout.Button("Save as new")) {
                var dialog = Instantiate(itemData).SaveDialog(DirectoryName, itemData.GetAssetPath().Split('/').LastOrDefault());
                dialog.OnSaved += asset => designer.itemData = itemData = (T) asset;
            }

            itemDataEditor.OnInspectorGUI();
            itemData.UpdateUsages();
            Repaint();
        }

        protected virtual void OnSceneGUI() {
            onSceneGUI();
            itemData.UpdateUsages();
            Repaint();
        }

        protected virtual string DirectoryName { get; } = "Assets/Data/";
        protected virtual string FileName { get; } = "Data Item";
    }
}