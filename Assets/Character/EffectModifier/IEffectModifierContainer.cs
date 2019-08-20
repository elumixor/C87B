using System.Collections.Generic;

namespace Character.EffectModifier {
    public interface IEffectModifierContainer {
        IEnumerable<IEffectModifier> Modifiers { get; }
    }
}