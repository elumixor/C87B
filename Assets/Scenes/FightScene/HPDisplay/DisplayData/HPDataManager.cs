using System;
using Character.HP;
using Shared;
using UnityEngine;

namespace Scenes.FightScene.HPDisplay.DisplayData {
    /// <summary>
    /// Order and color data for health types
    /// </summary>
    [CreateAssetMenu(fileName = "HPDataManager", menuName = "HP Data Manager")]
    public class HPDataManager : ScriptableObject {
        [SerializeField] private HPDisplayData[] values;
        public HPDisplayData this[HPType hpType] => hpType.ArrayValueIn(values);
    }
}