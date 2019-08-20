using UnityEngine;

namespace Shared.GameObjectManagement {
    public static class ComponentManagement {
        public static TComponent AddIfNotPresent<TComponent>(GameObject go) where TComponent : Component {
            var component = go.GetComponent<TComponent>();
            return !component ? go.AddComponent<TComponent>() : component;
        }
    }
}
