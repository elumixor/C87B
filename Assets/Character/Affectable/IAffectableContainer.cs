using System.Collections.Generic;

namespace Character.Affectable {
    public interface IAffectableContainer {
        IEnumerable<IAffectable> Affectables { get; }
    }
}