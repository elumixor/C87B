using System.IO;
using Combo.DataContainers;
using UnityEditor;
using UnityEngine;

namespace Combo {
    /// <summary>
    /// This class is focused on helping design <see cref="ComboFrameData"/> and save it as <see cref="ScriptableObject"/>.
    /// It allows previewing combo frame execution in live mode.
    /// </summary>
    public static class Helper {
        public static string savePath = "Assets/Data/";

        private const string ComboFolderName = "Combo";
        private const string ComboFrameFolderName = "Combo Frames";
        private const string ComboItemsFolderName = "Combo Items";
        private const string ComboButtonsFolderName = "Combo Buttons";
        private const string ComboSlidersFolderName = "Combo Sliders";

        /// <summary>
        /// Checks if specific item can be displayed on this frame.
        /// </summary>
        /// <param name="frameType">Combo frame type</param>
        /// <param name="item">Combo Item to be checked</param>
        public static bool IsDisplayed(ComboItemData item, ComboFrameType frameType) =>
            frameType == ComboFrameType.Unordered
            || frameType == ComboFrameType.Ordered
            || frameType == ComboFrameType.SimultaneousSlider && item is ComboSliderData
            || frameType == ComboFrameType.SimultaneousButton && item is ComboButtonData;
        
        public static (string combo, string comboFrame, string comboItems, string comboButtons, string comboSliders) Directories {
            get {
                string GetDirectory(params string[] paths) {
                    var combined = Path.Combine(paths);
                    Directory.CreateDirectory(combined);
                    return combined;
                }

                var combo = GetDirectory(savePath, ComboFolderName);
                var comboFrame = GetDirectory(savePath, ComboFolderName, ComboFrameFolderName);
                var comboItems = GetDirectory(savePath, ComboFolderName, ComboItemsFolderName);
                var comboButtons = GetDirectory(savePath, ComboFolderName, ComboItemsFolderName, ComboButtonsFolderName);
                var comboSliders = GetDirectory(savePath, ComboFolderName, ComboItemsFolderName, ComboSlidersFolderName);

                AssetDatabase.Refresh();

                return (combo, comboFrame, comboItems, comboButtons, comboSliders);
            }
        }
    }
}