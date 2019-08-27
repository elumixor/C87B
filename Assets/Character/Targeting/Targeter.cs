using Shared.EditorScripts;
using UnityEditor;

namespace Character.Targeting {
    /// <summary>
    /// Targeter is a behaviour script, that defines targets for <see cref="Ability"/> effects
    /// </summary>
    public sealed class Targeter : EnumMonoBehaviour<TargetType, Targetable> {}
    
    [CustomEditor(typeof(Targeter))]
    public class TargeterEditor : EnumEditor<TargetType, Targetable> {}
}