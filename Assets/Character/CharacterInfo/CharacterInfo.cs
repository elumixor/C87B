using System;
using UnityEngine;

namespace Character.CharacterInfo {
    /// <summary>
    /// Class to display information about <see cref="Character"/>
    /// </summary>
    [RequireComponent(typeof(Character))]
    public class CharacterInfo : MonoBehaviour {
        public Character character;

        
        public bool showParts;
        
        private void Reset() {
            character = GetComponent<Character>();
        }
    }
}