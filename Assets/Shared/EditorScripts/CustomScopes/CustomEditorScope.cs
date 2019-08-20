using System;

namespace Shared.EditorScripts.CustomScopes {
    public abstract class CustomEditorScope : IDisposable {
        private readonly Action endScope;

        protected CustomEditorScope(Action beginScope, Action endScope) {
            beginScope();
            this.endScope = endScope;
        }

        public void Dispose() {
            endScope();
        }
    }
}