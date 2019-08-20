using UnityEditor;
using UnityEngine;

namespace Shared.EditorScripts {
    public class ManagerWindow : EditorWindow {
        public Camera mainCamera;
        public Mesh mesh;
        public Material material;

        private const string WindowName = "Manager Window";

        [MenuItem("Window/" + WindowName)]
        private static void Init() {
            var window = (ManagerWindow) GetWindow(typeof(ManagerWindow), false, WindowName);
            window.Show();
        }

        private void OnGUI() {
            mainCamera = EditorGUILayout.ObjectField(mainCamera, typeof(Camera), true) as Camera;

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();

            mesh = EditorGUILayout.ObjectField(mesh, typeof(Mesh), true) as Mesh;
            material = EditorGUILayout.ObjectField(material, typeof(Material), true) as Material;

            GUILayout.EndVertical();

            if (GUILayout.Button("Instantiate")) {
                var instance = new GameObject("Test Object");
                instance.AddComponent<MeshRenderer>().material = material;
                instance.AddComponent<MeshFilter>().mesh = mesh;
            }

            GUILayout.EndHorizontal();


            if (GUILayout.Button("View to Main Camera")) {
                if (mainCamera != null) SceneView.lastActiveSceneView.AlignViewToObject(mainCamera.transform);
            }
        }
    }
}