using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shared.GameObjectManagement {
    public static class ChildrenManagement {
        public static void DestroyAllImmediate(Transform parent) {
            IterateChildren(t => Object.DestroyImmediate(t.gameObject), parent);
        }

        public static void DestroyAll(Transform parent) {
            IterateChildren(t => Object.Destroy(t.gameObject), parent);
        }

        public static void IterateChildren(Action<Transform> function, Transform parent) {
            var childCount = parent.childCount;
            var children = new Transform[childCount];
            for (var i = 0; i < childCount; i++) children[i] = parent.GetChild(i);
            foreach (var child in children) function(child);
        }

        public static GameObject FindOrCreate(string gameObjectName, Transform parent) {
            var found = parent.Find(gameObjectName);
            return found ? found.gameObject : CreateChild(gameObjectName, parent);
        }

        public static GameObject CreateChild(string gameObjectName, Transform parent) {
            var created = new GameObject(gameObjectName);
            created.transform.SetParent(parent);
            return created;
        }
    }
}