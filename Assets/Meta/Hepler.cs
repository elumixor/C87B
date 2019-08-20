using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Meta {
    public static class Helper {
        /// <summary>
        /// Retrieves selected folder on Project view.
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedPathOrFallback() {
            var path = "Assets";

            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets)) {
                path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) continue;

                path = Path.GetDirectoryName(path);
                break;
            }

            return path;
        }

        [Pure, NotNull]
        public static string ToNamespace([NotNull] this string filePath) =>
            string.Join(".", filePath.Split('\\').Skip(2));

        [Pure, NotNull]
        public static string Cased([NotNull] this string objectName) => $"{char.ToUpper(objectName[0])}{objectName.Substring(1)}";

        [NotNull] public static string[] Scenes => Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
    }
}