using System;
using Character.HP;
using UnityEngine;

namespace Scenes.FightScene.HPDisplay.DisplayData {
    /// <summary>
    /// Order and color data for health types
    /// </summary>
    [CreateAssetMenu(fileName = "HPDataManager", menuName = "HP Data Manager")]
    public class HPDataManager : ScriptableObject {
        [SerializeField] private HPDisplayData[] values;
        
        public HPDisplayData this[HPType hpType] {
            get {
                var types = (HPType[]) Enum.GetValues(typeof(HPType));
                for (var i = 0; i < types.Length; i++) {
                    if (types[i] == hpType) return values[i];
                }

                return default;
            }
        }
    }
}