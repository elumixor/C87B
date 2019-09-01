using System;
using Combo.DataContainers;
using JetBrains.Annotations;
using Shared.AnimationBehaviours;
using Shared.Behaviours;
using Shared.Behaviours.Animable;
using Shared.PropertyDrawers;
using UnityEngine;

namespace Combo.Items {
    /// <summary>
    /// <para>
    ///     ComboItem is a single abstract part of <see cref="Frame.ComboFrame"/> that inherits from <see cref="HitMissItem"/>
    /// </para>
    /// <para>
    ///     ComboItem encapsulates visualization process of hitting or missing and item.
    ///     When <see cref="OnHit"/> or <see cref="OnMissed"/> is executed, an animation is triggered.
    ///     When animation has finished the item is destroyed and <see cref="Destroyed"/> event is raised.
    /// </para>
    /// </summary>
    /// <seealso cref="IDestroyOnExitHandler"/>
    [RequireComponent(typeof(Animator))]
    public abstract class ComboItem : HitMissItem, IDestroyOnExitHandler, IAnimable {
        /// <summary>
        /// Destroyed event, that is raised after animation has finished
        /// </summary>
        public event EventHandler Destroyed;

        /// <summary>
        /// Reference to animator component
        /// </summary>
        [SerializeField, Required]
        private Animator animator;

        /// <summary>
        /// Implementation of <see cref="IAnimable"/>
        /// </summary>
        public Animator Animator => animator;

        /// <summary>
        /// Assigns <see cref="animator"/> component
        /// </summary>
        protected virtual void Reset() => animator = GetComponent<Animator>();

        /// <summary>
        /// <para>
        ///    On item hit
        /// </para>
        /// <para>
        ///    Starts hit animation and destroys component after is has finished
        /// </para>
        /// </summary>
        public override bool OnHit(float accuracy = 1f) {
            if (base.OnHit(accuracy)) {
                this.AnimateHit();
                return true;
            }

            return false;
        }

        /// <summary>
        /// <para>
        ///    On item missed
        /// </para>
        /// <para>
        ///    Starts miss animation and destroys component after is has finished
        /// </para>
        /// </summary>>
        public override bool OnMissed() {
            if (base.OnMissed()) {
                this.AnimateMiss();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Raises <see cref="Destroyed"/> event just before it is destroyed
        /// </summary>
        public void OnDestroyedOnExit() => Destroyed?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc cref="ComboItem"/>
    /// <remarks>
    ///     This is the base class for <see cref="Button.ComboButton"/> and <see cref="Slider.ComboSlider"/>
    /// </remarks>
    public abstract class ComboItem<TItemData> : ComboItem, ISettingsChangeHandler<TItemData> where TItemData : ComboItemData {
        /// <summary>
        /// <see cref="ComboItemData"/> reference
        /// </summary>
        [SerializeField, HideInInspector]
        private TItemData settings;

        /// <summary>
        /// Protected getter for <see cref="settings"/> to access in <see cref="OnSettingsChanged"/>
        /// </summary>
        public TItemData Settings => settings;

        /// <summary>
        /// Adjust parameters when <see cref="settings"/> has changed
        /// </summary>
        public abstract void OnSettingsChanged();

        protected override void Reset() {
            base.Reset();
            if (settings == null) settings = ScriptableObject.CreateInstance<TItemData>();
            OnSettingsChanged();
        }
    }
}