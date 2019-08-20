using System;
using UnityEngine;

namespace Shared.EditorScripts {
    public static class Requirements {
        public static void Require(bool condition, string message = null) {
            if (condition) return;

            Debug.LogError($"Condition was not met{(string.IsNullOrEmpty(message) ? "." : $": {message}")}");
            Application.Quit(1);
        }

        public static void Require(Func<bool> condition, string message = null) {
            Require(condition(), message);
        }
    }
}