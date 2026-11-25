using System;

namespace SuperMario.Core.Exceptions
{
    /// <summary>
    /// Raised when an entity moves outside the playable bounds of the level.
    /// </summary>
    [Serializable]
    public sealed class EntityOutOfBoundsException : MarioGameException
    {
        public EntityOutOfBoundsException(string message)
            : base(message)
        {
        }

        public EntityOutOfBoundsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
