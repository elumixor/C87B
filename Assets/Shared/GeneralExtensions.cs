using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Shared.Behaviours;
using Shared.EditorScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Shared {
    public static class GeneralExtensions {
        /// <summary>
        ///     Gets <see cref="FieldInfo.FieldType"/> for <see cref="FieldInfo"/>,
        ///     <see cref="PropertyInfo.PropertyType"/> for <see cref="PropertyInfo"/>,
        ///     and <see cref="MethodInfo.ReturnType"/> for <see cref="MethodInfo"/>
        /// </summary>
        /// <param name="memberInfo">Member info</param>
        /// <returns>Underlying type of member</returns>
        /// <exception cref="ArgumentException">
        ///     Throws ArgumentException if memberInfo argument is neither <see cref="FieldInfo"/>, <see cref="PropertyInfo"/> nor
        ///     <see cref="MethodInfo"/> or if given <see cref="MethodInfo"/> had either void return type or more than zero parameters
        /// </exception>
        [Pure, NotNull]
        public static Type GetUnderlyingType(this MemberInfo memberInfo) {
            if ((memberInfo.MemberType & MemberTypes.Field) != 0) return ((FieldInfo) memberInfo).FieldType;
            if ((memberInfo.MemberType & MemberTypes.Property) != 0) return ((PropertyInfo) memberInfo).PropertyType;
            if ((memberInfo.MemberType & MemberTypes.Method) != 0) return ((MethodInfo) memberInfo).ReturnType;

            throw new ArgumentException("Member should be a field, property or method");
        }

        /// <summary>
        /// Helper function to check <see cref="Type.IsAssignableFrom"/>, but works on generic types
        /// <example>
        /// <code>
        ///    class A&lt;T&gt; { ... }
        /// 
        ///
        ///    class B : A &lt;string&gt; { ... }
        /// 
        ///     typeof(B).IsAssignableFrom(typeof(A&lt;&gt;))   // false
        ///     typeof(B).IsGenericChild(typeof(A&lt;&gt;))     // true
        /// </code>
        /// </example>
        /// </summary>
        public static bool IsGenericChild(this Type baseType, Type childType) {
            while (childType != null && childType != typeof(object)) {
                var cur = childType.IsGenericType ? childType.GetGenericTypeDefinition() : childType;
                if (baseType == cur) return true;

                childType = childType.BaseType;
            }

            return false;
        }

        public static string ToSentenceCase(this string str) =>
            Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );

        public static TResult ArrayValueIn<TEnum, TResult>(this TEnum enumValue, IList<TResult> values) where TEnum : Enum {
            var types = (TEnum[]) Enum.GetValues(typeof(TEnum));

            for (var i = 0; i < types.Length; i++)
                if (Equals(types[i], enumValue))
                    return values[i];

            return default;
        }

        public static string GetAssetPath(this UnityEngine.Object @object) => AssetDatabase.GetAssetPath(@object);
        public static bool IsSavedFile(this UnityEngine.Object @object) => !string.IsNullOrEmpty(@object.GetAssetPath());
        public static string SaveNew(this ScriptableObject scriptableObject, string directoryName, string fileName) {
            var name = System.IO.Path.Combine(directoryName, $"{fileName}.asset");
            var i = 1;
            while (File.Exists(fileName)) name = System.IO.Path.Combine(directoryName, $"{fileName} {i++}.asset");

            AssetDatabase.CreateAsset(scriptableObject, name);
            return name;
        }


        public static SaveAssetWindow SaveDialog(this ScriptableObject scriptableObject, string directoryName, string fileName) =>
            SaveAssetWindow.Init(scriptableObject, directoryName, fileName);

        /// <summary>
        /// Retrieves selected folder on Project view.
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedPathOrFallback() {
            var path = "Assets";

            foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
                path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) continue;

                path = System.IO.Path.GetDirectoryName(path);
                break;
            }

            return path;
        }

        [Pure, NotNull]
        public static string ToNamespace([NotNull] this string filePath) =>
            string.Join(".", filePath.Split('\\').Skip(2));

        [Pure, NotNull]
        public static string Cased([NotNull] this string objectName) => $"{char.ToUpper(objectName[0])}{objectName.Substring(1)}";

        public static Color SetAlpha(this Color color, float alpha) {
            color.a = alpha;
            return color;
        }

        public static void SetColorAlpha(this Image image, float alpha) => image.color = image.color.SetAlpha(alpha);

        public static void UpdateUsages<TSettings>(this TSettings scriptableObject) where TSettings : ScriptableObject {
            foreach (var handler in Object.FindObjectsOfType<Component>().OfType<ISettingsChangeHandler<TSettings>>())
                handler.OnSettingsChanged();
        }

        public static void EnableCanvasOffset(ref RectTransform canvas, out Vector2 offset) {
            var c = Object.FindObjectOfType<Canvas>();
            if (c != null) {
                canvas = c.GetComponent<RectTransform>();
                var rect = canvas.rect;
                offset = new Vector2(rect.width * .5f, rect.height * .5f);
            } else offset = Vector2.zero;
        }
        public static void OnInspectorCanvasOffset(ref RectTransform canvas, ref Vector2 offset) {
            var newCanvas = (RectTransform) EditorGUILayout.ObjectField("Canvas", canvas, typeof(RectTransform), true);
            if (newCanvas != canvas) {
                canvas = newCanvas;
                if (canvas != null) {
                    var rect = canvas.rect;
                    offset = new Vector2(rect.width * .5f, rect.height * .5f);
                } else offset = Vector2.zero;
            }
        }
    }
}