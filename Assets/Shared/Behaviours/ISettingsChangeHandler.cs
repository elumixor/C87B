using JetBrains.Annotations;
using UnityEngine;

namespace Shared.Behaviours {
    /// <summary>
    /// Allows reacting to settings (<see cref="UnityEngine.ScriptableObject"/>) update
    /// </summary>
    public interface ISettingsChangeHandler<out TSettings> where TSettings : ScriptableObject {
        /// <summary>
        /// Called when settings
        /// </summary>
        void OnSettingsChanged();

        /// <summary>
        /// Getter for settings
        /// </summary>
        [CanBeNull]
        TSettings Settings { get; }
    }
}