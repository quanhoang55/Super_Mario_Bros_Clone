using System;
using System.Globalization;

namespace SuperMario.Core.Domain
{
    /// <summary>
    /// Engine-agnostic 2D vector used by the Core layer.
    /// Fields are public so every JSON backend (Newtonsoft.Json, System.Text.Json, JsonUtility)
    /// can serialize and deserialize it without extra attributes.
    /// </summary>
    [Serializable]
    public struct Vector2D : IEquatable<Vector2D>
    {
        public float X;
        public float Y;

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D Zero { get { return new Vector2D(0f, 0f); } }
        public static Vector2D One { get { return new Vector2D(1f, 1f); } }
        public static Vector2D Up { get { return new Vector2D(0f, 1f); } }
        public static Vector2D Down { get { return new Vector2D(0f, -1f); } }
        public static Vector2D Left { get { return new Vector2D(-1f, 0f); } }
        public static Vector2D Right { get { return new Vector2D(1f, 0f); } }

        public float SqrMagnitude { get { return (X * X) + (Y * Y); } }

        public float Magnitude { get { return (float)Math.Sqrt(SqrMagnitude); } }

        public Vector2D Normalized
        {
            get
            {
                float magnitude = Magnitude;
                return magnitude > 0.0001f ? new Vector2D(X / magnitude, Y / magnitude) : Zero;
            }
        }

        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2D operator -(Vector2D value)
        {
            return new Vector2D(-value.X, -value.Y);
        }

        public static Vector2D operator *(Vector2D value, float scalar)
        {
            return new Vector2D(value.X * scalar, value.Y * scalar);
        }

        public static Vector2D operator *(float scalar, Vector2D value)
        {
            return value * scalar;
        }

        public static Vector2D operator /(Vector2D value, float scalar)
        {
            return new Vector2D(value.X / scalar, value.Y / scalar);
        }

        public static bool operator ==(Vector2D left, Vector2D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2D left, Vector2D right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Vector2D other)
        {
            return Math.Abs(X - other.X) < 0.0001f && Math.Abs(Y - other.Y) < 0.0001f;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2D && Equals((Vector2D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return "(" + X.ToString("0.###", CultureInfo.InvariantCulture) + ", " + Y.ToString("0.###", CultureInfo.InvariantCulture) + ")";
        }
    }
}
