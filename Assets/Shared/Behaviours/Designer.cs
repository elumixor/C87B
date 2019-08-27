using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Shared.Behaviours {
    public class Designer<T> : MonoBehaviour where T : ScriptableObject {
        public T itemData;
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
            designer.itemData = itemData = (T) EditorGUILayout.ObjectField("Data", designer.itemData, typeof(T), true);
            if (!itemData.IsSavedFile()) {
                EditorGUILayout.HelpBox("Asset file does not exist", MessageType.Warning);
                if (GUILayout.Button("Save to file")) itemData.SaveDialog(DirectoryName, FileName);
            } else if (GUILayout.Button("Save as new"))
                Instantiate(itemData).SaveDialog(DirectoryName, itemData.GetAssetPath().Split('/').LastOrDefault());

            itemDataEditor.OnInspectorGUI();
        }

        protected virtual void OnSceneGUI() {
            onSceneGUI();
            Repaint();
        }
        
        protected virtual string DirectoryName { get; } = "Assets/Data/";
        protected virtual string FileName { get; } = "Data Item";
    }
}