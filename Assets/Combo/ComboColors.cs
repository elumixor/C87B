using Meta;
using Shared;
using Shared.EditorScripts;
using UnityEngine;

namespace Combo {
    [CreateAssetMenu(fileName = "Combo Colors", menuName = "ComboColors")]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class ComboColors : EnumScriptableObject<ComboFrameType, Color> { }

    public class ComboColorsEditor : EnumEditor<ComboFrameType, Color> { }
}