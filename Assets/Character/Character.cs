using System;
using System.Collections.Generic;
using System.Linq;
using Character.Affectable;
using Character.EffectModifier;
using Character.Targeting;
using Character.Trait;
using Character.Trait.ReflectionILHelper;
using JetBrains.Annotations;
using UnityEngine;

namespace Character {
    /// <summary>
    /// Character is an abstract behaviour component, that consists of various parts (affectables and trait providers)
    /// These parts are added separately, each as separate component
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Character : Targetable, IUpdatable<IAffectable> {
        /// <summary>
        /// Collections of character parts
        /// </summary>
        [SerializeField] protected List<CharacterPart> parts = new List<CharacterPart>();

        #region Trait management

        /// <summary>
        /// Helper class to manage trait types, providers and consumers
        /// </summary>
        private class TraitsManager {
            /// <summary>
            /// Helper class to manage providers and consumers on specific type
            /// </summary>
            public class Trait {
                /// <summary>
                /// Underlying container for providers as parts and getter functions
                /// </summary>
                private readonly Dictionary<CharacterPart, Getter> providers = new Dictionary<CharacterPart, Getter>();

                /// <summary>
                /// Underlying container for trait consumers
                /// </summary>
                private readonly HashSet<TraitConsumer> consumers = new HashSet<TraitConsumer>();

                /// <summary>
                /// Adds trait provider component
                /// </summary>
                /// <param name="part"></param>
                /// <param name="provider"></param>
                public void AddProvider([NotNull] CharacterPart part, [NotNull] Getter provider) {
                    if (providers.ContainsKey(part)) return;
                    providers.Add(part, provider);
                }

                /// <summary>
                /// Removes provider also destroying consumer components, if no providers left
                /// </summary>
                /// <param name="part"></param>
                public void RemoveProvider([NotNull] CharacterPart part) {
                    if (part == null) throw new ArgumentNullException(nameof(part));
                    if (providers.ContainsKey(part)) {
                        providers.Remove(part);
                        if (providers.Count == 0) {
                            foreach (var traitConsumer in consumers) {
                                consumers.Remove(traitConsumer);
                                traitConsumer.OnProvidersEmpty();
                            }
                        }
                    }
                }

                /// <summary>
                /// Adds consumer to consumers set if <see cref="providers"/> contains at least one provider 
                /// </summary>
                /// <param name="consumer">Consumer component</param>
                /// <returns>
                /// Providers: dictionary of part and function to retrieve trait
                /// or null if no providers exist and consumer was not added
                /// </returns>
                public bool AddConsumer([NotNull] TraitConsumer consumer) {
                    var providersExist = providers.Count > 0;
                    if (providersExist) {
                        consumers.Add(consumer);
                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Gets all provided values
                /// </summary>
                public IEnumerable<object> Values => providers.Values.Select(v => v());

                /// <summary>
                /// Removes consumer from set without deleting component
                /// </summary>
                /// <param name="consumer">Consumer component</param>
                public void RemoveConsumer([NotNull] TraitConsumer consumer) {
                    consumers.Remove(consumer);
                }
            }

            /// <summary>
            /// Gets all provided values of type
            /// </summary>
            /// <typeparam name="T">Retrieved type</typeparam>
            /// <returns></returns>
            [Pure, NotNull]
            public T[] GetValues<T>() => traits.ContainsKey(typeof(T)) ? traits[typeof(T)].Values.OfType<T>().ToArray() : new T[0];

            /// <summary>
            /// Underlying container of Trait types to providers and consumers
            /// </summary>
            private readonly Dictionary<Type, Trait> traits = new Dictionary<Type, Trait>();

            /// <summary>
            /// Returns providers and consumers for given trait
            /// </summary>
            /// <param name="traitType">Trait type</param>
            [NotNull]
            public Trait this[Type traitType] => traits.ContainsKey(traitType) ? traits[traitType] : traits[traitType] = new Trait();

            public TraitsManager() { }

            public TraitsManager([NotNull] IEnumerable<CharacterPart> parts) {
                foreach (var part in parts) {
                    foreach (var (type, provider) in TypeCacher.type2ProvidedTypes[part.GetType()]) {
                        if (!traits.ContainsKey(type)) traits[type] = new Trait();
                        traits[type].AddProvider(part, provider.Substitute(part));
                    }
                }
            }
        }

        /// <summary>
        /// Instance of <see cref="TraitsManager"/>
        /// </summary>
        private TraitsManager traitManager = new TraitsManager();

        #endregion

        /// <summary>
        /// Affectables
        /// </summary>
        public override IEnumerable<IAffectable> Affectables => parts.OfType<IAffectable>();

        /// <summary>
        /// Character traits
        /// </summary>
        public override IEnumerable<IEffectModifier> Modifiers => parts.OfType<IEffectModifier>();

        #region Parts public methods

        /// <summary>
        /// Adds part to <see cref="parts"/> and registers it as provider in <see cref="traitManager"/>
        /// </summary>
        /// <param name="part">Character part instance to be added</param>
        [PublicAPI]
        internal void AddPart([NotNull] CharacterPart part) {
            if (parts.Contains(part)) return;

            parts.Add(part);
            foreach (var (type, provider) in TypeCacher.type2ProvidedTypes[part.GetType()])
                traitManager[type].AddProvider(part, provider.Substitute(part));
        }

        /// <summary>
        /// Removes part from parts and updates provided traits
        /// </summary>
        /// <param name="part">Character part instance to be removed</param>
        [PublicAPI]
        internal void RemovePart([NotNull] CharacterPart part) {
            if (parts.Remove(part))
                foreach (var (type, _) in TypeCacher.type2ProvidedTypes[part.GetType()])
                    traitManager[type].RemoveProvider(part);
        }

        #endregion

        #region Trait Consumers public methods

        /// <summary>
        /// Adds trait consumer in case providers of that trait exist
        /// </summary>
        /// <param name="traitConsumer">Trait consumer</param>
        /// <returns>True if consumer added, false otherwise</returns>
        internal bool AddTraitConsumer([NotNull] TraitConsumer traitConsumer) {
            var requiredTraits = TypeCacher.consumerType2RequiredTypes[traitConsumer.GetType()];

            if (requiredTraits.All(type => traitManager[type].AddConsumer(traitConsumer))) return true;

            foreach (var requiredTrait in requiredTraits) traitManager[requiredTrait].RemoveConsumer(traitConsumer);
            return false;
        }

        /// <summary>
        /// Removes trait consumer
        /// </summary>
        /// <param name="traitConsumer">Trait consumer</param>
        internal void RemoveTraitConsumer([NotNull] TraitConsumer traitConsumer) {
            var requiredTraits = TypeCacher.consumerType2RequiredTypes[traitConsumer.GetType()];
            foreach (var traitType in requiredTraits) traitManager[traitType].RemoveConsumer(traitConsumer);
        }

        /// <summary>
        /// Get array of trait values
        /// </summary>
        /// <typeparam name="TTrait">Required type</typeparam>
        /// <returns>Array of trait values</returns>
        [Pure]
        public TTrait[] GetTraits<TTrait>() => traitManager.GetValues<TTrait>();

        #endregion

        /// <summary>
        /// Propagates update event from Updateable
        /// </summary>
        public event UpdateHandler<IAffectable> Update;

        /// <summary>
        /// Fill <see cref="traitManager"/> with traits from <see cref="parts"/> and
        /// propagate <see cref="IUpdatable{T}.Update"/> event to <see cref="Update"/>
        /// </summary>
        private void Awake() {
            foreach (var updatable in Affectables.OfType<IUpdatable<IAffectable>>())
                updatable.Update += updatedAffectable => Update?.Invoke(updatedAffectable);

            traitManager = new TraitsManager(parts);
        }
    }
}