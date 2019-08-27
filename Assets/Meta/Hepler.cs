using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Meta {
    public static class Helper {
        [NotNull] public static string[] Scenes => Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
    }
}