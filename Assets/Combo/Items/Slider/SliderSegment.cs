using System;
using Shared.AnimationBehaviours;
using Shared.Behaviours.Animable;
using UnityEngine;

namespace Combo.Items.Slider {
    [RequireComponent(typeof(RectTransform), typeof(SVGImage), typeof(Animator))]
    public class SliderSegment : MonoBehaviour, IAnimable, IDestroyOnExitHandler {
        [SerializeField] private Animator animator;
        public SVGImage image;
        public Animator Animator => animator;


        private void Reset() {
            animator = GetComponent<Animator>();
            image = GetComponent<SVGImage>();
        }

        public event EventHandler Destroyed;
        public void OnDestroyedOnExit() => Destroyed?.Invoke(this, EventArgs.Empty);
    }
}