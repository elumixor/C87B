using UnityEngine;

namespace Shared.AnimationBehaviours {
    public class DestroyOnExit : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            foreach (var exitHandler in animator.GetComponents<IDestroyOnExitHandler>()) {
                exitHandler.OnDestroyedOnExit();
            }
            Destroy(animator.gameObject, stateInfo.length);
        }
    }
}