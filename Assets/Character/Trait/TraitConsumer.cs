using System;
using System.Collections;
using System.Linq;
using Character.Trait.ReflectionILHelper;
using JetBrains.Annotations;
using UnityEngine;

namespace Character.Trait {
    /// <summary>
    ///<para>
    ///     Base class for all trait consumers.
    /// </para>
    /// <para>
    ///     Trait consumer is a <see cref="MonoBehaviour"/>, that
    ///     requires some <see cref="Trait"/> to be available from <see cref="Character"/>'s <see cref="Character.parts"/>
    /// </para>
    /// </summary>
    /// <remarks>
    ///    Use with <see cref="RequireTraitAttribute"/> to specify data about required traits
    /// </remarks>
    /// <example>
    /// <code>
    ///     [RequireTrait(typeof(Bar), count = 3)]
    ///     class Foo : TraitConsumer { ...  }
    /// 
    ///
    ///     class Engine : CharacterPart {
    ///         [Trait]
    ///         private Bar bar;
    ///
    ///         ...
    ///     }
    /// </code>
    /// </example>
    /// <seealso cref="TraitAttribute"/>
    /// <seealso cref="RequireTraitAttribute"/>
    [RequireComponent(typeof(Character))]
    public abstract class TraitConsumer : MonoBehaviour {
        /// <summary>
        /// Character component reference
        /// </summary>
        [SerializeField] protected Character character;

        /// <summary>
        /// <para>
        ///     Method, that is called every time provider components update via <see cref="IUpdatable{T}.Update"/> event,
        ///     or when providers are destroyed or added
        /// </para>
        /// <para>
        ///     This method should typically update values using <see cref="GetTraits{TTrait}"/>
        /// </para>
        /// <para>
        ///     Abstract modifier to ensure correct conceptual inheritance. Without handling provider's update
        ///     deriving from <see cref="TraitConsumer"/> would be pointless 
        /// </para>
        /// </summary>
        protected abstract void OnProvidersUpdate();

        /// <summary>
        /// Get reference to <see cref="character"/> component 
        /// </summary>
        protected virtual void Awake() {
            character = GetComponent<Character>();
        }

        /// <summary>
        /// Add self as consumer to a character and register callback to update
        /// </summary>
        protected virtual void Start() {
            if (!character.AddTraitConsumer(this)) DestroyDelayed();

            var requiredTraits = TypeCacher.consumerType2RequiredTypes[GetType()];

            // Call OnProvidersUpdate when character's update is affectable, and is one of provided types 
            character.Update += affectable => {
                if (requiredTraits.Contains(affectable.GetType()))
                    OnProvidersUpdate();
            };
        }

        /// <summary>
        /// Try to add this trait consumer to character. If could not be added, remove component 
        /// </summary>
        protected virtual void Reset() {
            character = GetComponent<Character>();
            if (!character.AddTraitConsumer(this)) DestroyDelayed(true);
        }

        /// <summary>
        /// Destroys component at the end of frame via coroutine and <see cref="WaitForEndOfFrame"/>
        /// </summary>
        private void DestroyDelayed(bool immediate = false) {
            IEnumerator DestroyCoroutine() {
                yield return new WaitForEndOfFrame();
                if (immediate) DestroyImmediate(this);
                else Destroy(this);
            }

            StartCoroutine(DestroyCoroutine());
        }

        /// <summary>
        /// Remove self from trait consumers of character
        /// </summary>
        protected virtual void OnDestroy() => character.RemoveTraitConsumer(this);
        
        /// <summary>
        /// Alias for <see cref="Character.GetTraits{TTrait}"/>
        /// </summary>
        /// <typeparam name="TTrait">Required type</typeparam>
        /// <returns>Array of trait values</returns>
        [PublicAPI]
        protected TTrait[] GetTraits<TTrait>() => character.GetTraits<TTrait>();

        /// <summary>
        /// Get first trait value
        /// </summary>
        /// <typeparam name="TTrait"></typeparam>
        /// <returns></returns>
        [PublicAPI]
        protected TTrait GetTrait<TTrait>() => GetTraits<TTrait>().First();

        /// <summary>
        /// Runs 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OnProvidersEmpty() => DestroyDelayed();
    }
}