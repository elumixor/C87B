using JetBrains.Annotations;

namespace Meta {
    public static class FileTemplates {
        [Pure, NotNull]
        public static string MonoBehaviour([NotNull] string namespaceName, [NotNull] string className) =>
            $@"using UnityEngine;

namespace {namespaceName} {{
    public class {className} : MonoBehaviour {{
    
    }}
}}
";

        [Pure, NotNull]
        public static string Editor([NotNull] string namespaceName, [NotNull] string targetTypeName,
            [CanBeNull] string editorClassName = null) =>
            $@"using UnityEditor;

namespace {namespaceName} {{
    [CustomEditor(typeof({editorClassName ?? $"{targetTypeName}Editor"})]
    class {editorClassName} : Editor {{
        
    }}
}}
";
    }
}