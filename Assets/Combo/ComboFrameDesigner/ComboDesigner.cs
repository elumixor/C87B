using System.Collections.Generic;
using System.Linq;
using Combo.ComboItems;
using Combo.ComboItems.ComboButton;
using Combo.ComboItems.ComboSlider;
using Combo.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace Combo.ComboFrameDesigner {
    /// <summary>
    /// This class is focused on helping design <see cref="ComboFrame"/> and save it as <see cref="ScriptableObject"/>.
    /// It allows previewing combo frame execution in live mode.
    /// </summary>
    [SelectionBase]
    public class ComboDesigner : MonoBehaviour {
        public Image background;
        public List<ComboItem.Data> items;
        public Domain.ComboFrame.ComboFrameType frameType;
        public Domain.ComboFrame frameSettings;

        public ComboSlider sliderPrefab;
        public ComboButton buttonPrefab;

        [Range(0f, 2f)] public float executionTime;

        /// <summary>
        /// Checks if specific item can be displayed on this frame.
        /// </summary>
        /// <param name="item">Combo Item to be checked</param>
        public bool IsDisplayed(ComboItem.Data item) {
            return frameType == Domain.ComboFrame.ComboFrameType.Unordered
                   || frameType == Domain.ComboFrame.ComboFrameType.Ordered
                   || frameType == Domain.ComboFrame.ComboFrameType.SimultaneousSlider && item is ComboSlider.Data
                   || frameType == Domain.ComboFrame.ComboFrameType.SimultaneousButton && item is ComboButton.Data;
        }

        /// <summary>
        /// List of only displayed items
        /// </summary>
        public List<ComboItem.Data> DisplayedItems => items.Where(IsDisplayed).ToList();

        public void CreateFrame() {
            ComboFrame.ComboFrame.Create(buttonPrefab, sliderPrefab, GetCurrentSettings(), transform);
        }

        public Domain.ComboFrame GetCurrentSettings() {
            var comboFrame = ScriptableObject.CreateInstance<Domain.ComboFrame>();
            comboFrame.items = DisplayedItems.ToArray();
            comboFrame.frameType = frameType;
            comboFrame.executionTime = executionTime;

            return comboFrame;
        }

        public void AssignFromSettings(Domain.ComboFrame comboFrame) {
            items = new List<ComboItem.Data>(comboFrame.items);
            frameType = comboFrame.frameType;
            executionTime = comboFrame.executionTime;
        }
    }
}