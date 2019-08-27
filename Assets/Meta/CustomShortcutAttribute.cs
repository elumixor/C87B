using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Shared;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Meta {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CustomShortcutAttribute : Attribute {
        public string Hotkey { get; set; }
        public ShortcutMode Mode { get; set; } = ShortcutMode.Unclassified;

        public enum ShortcutMode {
            Generate,
            Unclassified,
        }

        public static (string name, string hotkey, Action method, ShortcutMode mode)[] ShortcutActions { get; private set; }

        [DidReloadScripts]
        [InitializeOnLoadMethod]
        private static void CacheActions() {
            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes()).ToArray();

            ShortcutActions = types.Where(type => IsDefined(type, typeof(CustomShortcutAttribute), true)
                                                  && IsDefined(type, typeof(CreateAssetMenuAttribute), true))
                .Select(type => {
                    var createAsset = (CreateAssetMenuAttribute) GetCustomAttribute(type, typeof(CreateAssetMenuAttribute));
                    var customShortcutAttribute = (CustomShortcutAttribute) GetCustomAttribute(type, typeof(CustomShortcutAttribute));
                    return (name: createAsset.fileName,
                        keys: customShortcutAttribute.Hotkey,
                        method: new Action(() => {
                            var asset = ScriptableObject.CreateInstance(type);

                            AssetDatabase.CreateAsset(asset,
                                Path.Combine(GeneralExtensions.GetSelectedPathOrFallback(), $"{createAsset.fileName}.asset"));
                            AssetDatabase.SaveAssets();

                            EditorUtility.FocusProjectWindow();

                            Selection.activeObject = asset;
                        }),
                        mode: customShortcutAttribute.Mode);
                }).Union(types.SelectMany(type => type
                    .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(method =>
                        IsDefined(method, typeof(CustomShortcutAttribute), true)
                        && IsDefined(method, typeof(MenuItem), true)
                        && method.ReturnType == typeof(void)
                        && method.GetParameters().Length == 0)
                    .Select(method => {
                        var customShortcutAttribute =
                            (CustomShortcutAttribute) GetCustomAttribute(method, typeof(CustomShortcutAttribute));
                        var itemName = ((MenuItem) GetCustomAttribute(method, typeof(MenuItem))).menuItem;
                        var index = itemName.IndexOfAny(new[] {'_', '&', '%', '#'});

                        return (name: index == -1 ? itemName : itemName.Substring(0, index),
                            keys: customShortcutAttribute.Hotkey,
                            method: (Action) method.CreateDelegate(typeof(Action)),
                            mode: customShortcutAttribute.Mode);
                    })))
                .ToArray();
        }
    }
}