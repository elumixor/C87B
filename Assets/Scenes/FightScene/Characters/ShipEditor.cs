using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Character;
using Character.Trait;
using Character.Trait.ReflectionILHelper;
using UnityEditor;
using UnityEngine;

namespace Scenes.FightScene.Characters {
    [CustomEditor(typeof(Ship))]
    public class ShipEditor : Editor {
        private Ship ship;
        private const string FieldName = "parts";
        private FieldInfo parts;

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic;

        private void OnEnable() {
            ship = (Ship) target;
            parts = typeof(Character.Character).GetField(FieldName, BindingFlags);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var componentParts = (List<CharacterPart>) parts.GetValue(ship);

            var newPart = (CharacterPart) EditorGUILayout.ObjectField("Add part", null, typeof(CharacterPart), false);
            if (newPart != null) componentParts.Add(newPart);

            EditorGUILayout.Foldout(true, $"Parts {componentParts.Count}", true);

            foreach (var part in componentParts) {
                GUILayout.Label(part.Name);
                var members = part.GetType().GetMembers(BindingFlags).Where(mi => Attribute.IsDefined(mi, typeof(TraitAttribute)));
                var getters = members.Select(DelegateCreator.CreateDelegate);

                using (new EditorGUI.IndentLevelScope()) {
                    foreach (var getterAbstract in getters) {
                        GUILayout.Label(getterAbstract(part).ToString());
                    }
                }
            }
        }
    }
}