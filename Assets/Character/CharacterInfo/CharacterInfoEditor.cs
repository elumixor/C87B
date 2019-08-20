using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Character.HP;
using Character.Trait;
using Character.Trait.ReflectionILHelper;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Character.CharacterInfo {
    [CustomEditor(typeof(CharacterInfo))]
    public class CharacterInfoEditor : Editor {
        private delegate List<CharacterPart> Getter();

        private CharacterInfo info;
        private Character character;
        private const string fieldName = "parts";
        private FieldInfo parts;
        
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic;

        private void OnEnable() {
            info = (CharacterInfo) target;
            character = info.character;
            parts = typeof(Character).GetField(fieldName, BindingFlags);
        }

        public override void OnInspectorGUI() {
            var componentParts = character.GetComponents<CharacterPart>();

            if (parts == null) EditorGUILayout.HelpBox("'parts' property was not found", MessageType.Warning);
            else if (GUILayout.Button("Assign parts")) {
                var characterParts = new List<CharacterPart>((List<CharacterPart>) parts.GetValue(character));
                foreach (var part in characterParts) character.RemovePart(part);

                foreach (var part in componentParts) character.AddPart(part);
            }

            EditorGUILayout.Foldout(true, $"Parts {componentParts.Length}", true);

            foreach (var part in componentParts) {
                GUILayout.Label(part.Name);
                var members = part.GetType().GetMembers(BindingFlags).Where(mi => Attribute.IsDefined(mi, typeof(TraitAttribute)));
                var getters = members.Select(DelegateCreator.CreateDelegate);
//                var providedTypes = TypeCacher.type2ProvidedTypes[part.GetType()];

                using (new EditorGUI.IndentLevelScope()) {
                    foreach (var getterAbstract in getters) {
                        GUILayout.Label(getterAbstract(part).ToString());
                    }

//                    foreach (var (type, getter) in providedTypes) {
//                        GUILayout.Label(type.Name + " " + getter(part));
//                    }
                }
            }
        }
    }
}