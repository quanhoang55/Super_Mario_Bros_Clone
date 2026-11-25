using System;

namespace SuperMario.Core.Contracts
{
    /// <summary>
    /// Engine-agnostic input contract. The Unity layer implements it with an adapter
    /// (see <c>UnityInputAdapter</c> in SuperMario.App) so the Core never references the engine.
    /// </summary>
    public interface IInputProvider : IDisposable
    {
        /// <summary>Horizontal movement axis in the range [-1, 1].</summary>
        float MoveAxisX { get; }

        /// <summary>True on the frame the jump button was pressed.</summary>
        bool IsJumpPressed { get; }

        /// <summary>True while the jump button is being held.</summary>
        bool IsJumpHeld { get; }

        /// <summary>True on the frame the shoot button was pressed.</summary>
        bool IsShootPressed { get; }

        /// <summary>True on the frame the pause button was pressed.</summary>
        bool IsPausePressed { get; }
    }
}
