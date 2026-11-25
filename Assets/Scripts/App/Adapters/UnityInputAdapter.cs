using SuperMario.Core.Contracts;
using UnityEngine;

namespace SuperMario.App.Adapters
{
    /// <summary>
    /// Unity adapter that implements the engine-agnostic <see cref="IInputProvider"/>
    /// on top of the generated Input System actions (SuperMario.Input assembly).
    /// </summary>
    public sealed class UnityInputAdapter : IInputProvider
    {
        private readonly PlayerInputActions _actions;
        private bool _disposed;

        public UnityInputAdapter()
        {
            _actions = new PlayerInputActions();
            _actions.Enable();
        }

        public float MoveAxisX
        {
            get { return _actions.Player.Move.ReadValue<Vector2>().x; }
        }

        public bool IsJumpPressed
        {
            get { return _actions.Player.Jump.WasPressedThisFrame(); }
        }

        public bool IsJumpHeld
        {
            get { return _actions.Player.Jump.IsPressed(); }
        }

        public bool IsShootPressed
        {
            get { return _actions.Player.Attack.WasPressedThisFrame(); }
        }

        public bool IsPausePressed
        {
            get { return _actions.GamePlay.Pause.WasPressedThisFrame(); }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _actions.Disable();
            _actions.Dispose();
        }
    }
}
