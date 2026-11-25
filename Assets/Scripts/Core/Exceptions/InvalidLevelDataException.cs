using System;

namespace SuperMario.Core.Exceptions
{
    /// <summary>
    /// Raised when a level definition is missing required data or is malformed.
    /// </summary>
    [Serializable]
    public sealed class InvalidLevelDataException : MarioGameException
    {
        public InvalidLevelDataException(string message)
            : base(message)
        {
        }

        public InvalidLevelDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
