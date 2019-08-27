using System;
using Character.HP;
using Shared.EditorScripts;
using UnityEditor;

namespace Scenes.FightScene.HPDisplay.DisplayData {
    [CustomEditor(typeof(HPDataManager))]
    public class HPDataManagerEditor : EnumEditor<HPType, HPDisplayData> {
        protected override string FieldName => "values"; 
    }
}