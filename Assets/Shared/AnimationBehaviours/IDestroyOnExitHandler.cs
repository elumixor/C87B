using System;

namespace Shared.AnimationBehaviours {
    public interface IDestroyOnExitHandler {
        void OnDestroyedOnExit();
        event EventHandler Destroyed;
    }
}