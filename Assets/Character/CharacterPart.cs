using System;
using Character.Trait;
using JetBrains.Annotations;
using Meta;
using Shared;
using Shared.EditorScripts;
using Shared.PropertyDrawers;
using UnityEditor;
using UnityEngine;

namespace Character {
    /// <summary>
    /// <para>
    ///     Base for character parts
    /// </para>
    /// <para>
    ///     CharacterPart is a component, that provides some functionality to a <see cref="Character"/>
    /// </para>
    /// <para>
    ///     Functionality is added as publicly available fields and properties, that are called traits.
    ///     Traits are added to character automatically on <see cref="Awake"/> or <see cref="Reset"/> and retrieved 
    /// </para>
    /// </summary>
    [CreateAssetMenu(fileName = "Character Part", menuName = "Character Parts/Character Part", order = 1)]
    [CustomShortcut(Mode = CustomShortcutAttribute.ShortcutMode.Generate)]
    public class CharacterPart : ScriptableObject {
        /// <summary>
        /// Character part name
        /// </summary>
        [PublicAPI]
        public virtual string Name => GetType().Name.ToSentenceCase();

        /// <summary>
        /// Character part can provide abilities
        /// </summary>
        [Trait, SerializeField] protected Ability.Ability[] abilities;

//        /// <summary>
//        /// Each part has durability. When it reaches zero, the part should be destroyed
//        /// </summary>
//        [SerializeField] protected float durability;
//        /// <summary>
//        /// Adds self to <see cref="Character.parts"/> is is not already added and registers self as trait provider
//        /// </summary>
//        protected virtual void Start() {
//            character.AddPart(this);
//        }
//
//        /// <summary>
//        /// Remove part from <see cref="Character.parts"/>
//        /// </summary>
//        protected void OnDestroy() {
//            character.RemovePart(this);
//        }
//
//        /// <summary>
//        /// This is used to give good default values in inspector, we cannot rely on it as in-game initializer 
//        /// </summary>
//        protected virtual void Reset() {
//            character = GetComponent<Character>();
//            character.AddPart(this);
//        }


    }
//
//    /// <summary>
//    /// <para>
//    ///     Base for character parts with generic data container
//    /// </para>
//    /// <para>
//    ///     CharacterPart is a component, that provides some functionality to a <see cref="Character"/>
//    /// </para>
//    /// <para>
//    ///     Functionality is added as publicly available fields and properties, that are called traits.
//    ///     Traits are added to character automatically on <see cref="Awake"/> or <see cref="Reset"/> and retrieved 
//    /// </para>
//    /// </summary>
//    /// <seealso cref="CharacterPart"/>
//    /// <typeparam name="TData">Data of character part, must derive from <see cref="ScriptableObject"/></typeparam>
//    public abstract class CharacterPart<TData> : CharacterPart where TData : ScriptableObject {
////        protected override void Reset() {
////            base.Reset();
////            if (data != null) AssignFromData(data);
////        }
////
////        protected override void Awake() {
////            base.Awake();
////            if (data != null) AssignFromData(data);
////        }
////
////        protected virtual void OnValidate() {
////            if (data != null) AssignFromData(data);
////        }
//
//        /// <summary>
//        /// Data container for this part
//        /// </summary>
//        [SerializeField, CanBeNull, Required] private TData data;
//
//        /// <summary>
//        /// Assigns fields from <see cref="data"/>. Must be implemented in child classes
//        /// </summary>
//        /// <param name="data"></param>
//        protected abstract void AssignFromData([NotNull] TData data);
//    }
}