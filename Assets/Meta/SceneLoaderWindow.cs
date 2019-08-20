using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Meta {
    public class SceneLoaderWindow : EditorWindow {
        [MenuItem("Custom/Windows/Scene Loader")]
        private static void CreateWindow() => GetWindow<SceneLoaderWindow>(false, "Scene Loader").Show();

        private string[] scenes;

        private void OnEnable() {
            UpdateScenes();
        }

        private void OnGUI() {
            foreach (var scene in scenes)
                if (GUILayout.Button(Path.GetFileNameWithoutExtension(scene)))
                    EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);

            GUILayout.Space(15);
            
            if (GUILayout.Button("Update Scenes")) UpdateScenes();
        }

        private void UpdateScenes() => scenes = Helper.Scenes;
    }
}