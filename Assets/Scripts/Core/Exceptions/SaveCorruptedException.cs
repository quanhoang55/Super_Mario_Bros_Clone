using System;

namespace SuperMario.Core.Exceptions
{
    /// <summary>
    /// Raised when a save file cannot be read, parsed or written.
    /// </summary>
    [Serializable]
    public sealed class SaveCorruptedException : MarioGameException
    {
        public SaveCorruptedException(string message)
            : base(message)
        {
        }

        public SaveCorruptedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
