using System;

namespace SuperMario.Core.Exceptions
{
    /// <summary>
    /// Base type for every domain exception raised by the game.
    /// </summary>
    [Serializable]
    public class MarioGameException : Exception
    {
        public MarioGameException()
        {
        }

        public MarioGameException(string message)
            : base(message)
        {
        }

        public MarioGameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
