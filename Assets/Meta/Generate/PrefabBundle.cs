using System.IO;
using Shared.EditorScripts;
using Shared.EditorScripts.CustomScopes;
using UnityEditor;
using UnityEngine;

namespace Meta.Generate {
    public class PrefabBundle : Popup {
        [MenuItem("Assets/Create/Bundle #&%g")]
        [CustomShortcut(Hotkey = "Ctrl + Alt + Shift + G")]
        private static void Init() {
            var window = CreateInstance<PrefabBundle>();
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 250, 150);
            window.ShowPopup();
            window.Focus();
        }

        private string bundleName;
        private bool isRect;

        protected override void OnGUI() {
            base.OnGUI();
            GUI.SetNextControlName("textField");
            bundleName = GUILayout.TextField(bundleName);
            GUI.FocusControl("textField");

            using (new HorizontalScope()) {
                if (GUILayout.Button("Transform")) isRect = false;
                if (GUILayout.Button("Rect Transform")) isRect = true;
            }

            GUILayout.Label(isRect ? "RectTransform" : "Transform");

            var enabled = !string.IsNullOrEmpty(bundleName);

            using (new HorizontalScope()) {
                if (GUILayout.Button("Close")) Close();
                GUI.enabled = enabled;
                if (GUILayout.Button("Create")) Create();
            }

            if (code == KeyCode.Return && enabled) Create();
        }

        private void Create() {
            Close();

            var path = Helper.GetSelectedPathOrFallback();

            var prefabDirectoryPath = Path.Combine(path, bundleName);
            if (!Directory.Exists(prefabDirectoryPath)) AssetDatabase.CreateFolder(path, bundleName);

            var instance = isRect ? new GameObject(bundleName, typeof(RectTransform)) : new GameObject(bundleName);

            var namespacePath = prefabDirectoryPath.ToNamespace();

            var casedName = bundleName.Cased();

            var scriptFilePath = Path.Combine(prefabDirectoryPath, $"{casedName}.cs");

            using (var outfile = new StreamWriter(scriptFilePath)) {
                outfile.WriteLine(FileTemplates.MonoBehaviour(namespacePath, casedName));
            }

            AssetDatabase.ImportAsset(scriptFilePath, ImportAssetOptions.ForceUpdate);

            scriptFilePath = Path.Combine(prefabDirectoryPath, $"{casedName}Editor.cs");

            using (var outfile = new StreamWriter(scriptFilePath)) {
                outfile.WriteLine(FileTemplates.Editor(namespacePath, casedName));
            }

            AssetDatabase.ImportAsset(scriptFilePath, ImportAssetOptions.ForceUpdate);

            var prefabPath = Path.Combine(prefabDirectoryPath, $"{bundleName}.prefab");
            PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);

            DestroyImmediate(instance);

            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(prefabPath);
        }
    }
}