using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.Windows.WebCam;

namespace Meta {
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomShortcutAttribute : Attribute {
        public string Hotkey { get; set; }
        
        public static class ShortcutMode {
            public const string General = "general";
            public const string Unclassified = "unclassified";
        }
        
        public static (string name, string hotkey, Action method, string mode)[] ShortcutActions { get; private set; }

        [DidReloadScripts]
        private static void CacheActions() {
            ShortcutActions = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .SelectMany(type => type
                    .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(method =>
                        IsDefined(method, typeof(CustomShortcutAttribute), true)
                        && IsDefined(method, typeof(MenuItem), true)
                        && method.ReturnType == typeof(void)
                        && method.GetParameters().Length == 0)
                    .Select(method => {
                        var menuItem = ((MenuItem) GetCustomAttribute(method, typeof(MenuItem))).menuItem;
                        var customShortcutAttribute = (CustomShortcutAttribute) GetCustomAttribute(method, typeof(CustomShortcutAttribute));
                        var split = menuItem.Split('/');
                        
                        return (name: menuItem.Substring(0,
                                menuItem.IndexOf(
                                    menuItem.First(c =>
                                        c == '_'
                                        || c == '&'
                                        || c == '%'
                                        || c == '#'))),
                            keys: customShortcutAttribute.Hotkey,
                            method: (Action) method.CreateDelegate(typeof(Action)),
                            mode: split.Length >= 3 ? split[2].ToLowerInvariant() : "unclassified");
                    }))
                .ToArray();
        }
    }
}