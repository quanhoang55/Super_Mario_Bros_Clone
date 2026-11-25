using System;
using System.Globalization;

namespace SuperMario.Core.Domain
{
    /// <summary>
    /// Axis-aligned bounding box (AABB) used for engine-agnostic collision checks.
    /// </summary>
    [Serializable]
    public struct BoundingBox
    {
        public Vector2D Center;
        public Vector2D Size;

        public BoundingBox(Vector2D center, Vector2D size)
        {
            Center = center;
            Size = size;
        }

        public static BoundingBox FromMinMax(Vector2D min, Vector2D max)
        {
            Vector2D size = max - min;
            return new BoundingBox(min + (size * 0.5f), size);
        }

        public float MinX { get { return Center.X - (Size.X * 0.5f); } }
        public float MaxX { get { return Center.X + (Size.X * 0.5f); } }
        public float MinY { get { return Center.Y - (Size.Y * 0.5f); } }
        public float MaxY { get { return Center.Y + (Size.Y * 0.5f); } }

        /// <summary>
        /// Returns true when the two AABBs overlap.
        /// </summary>
        public bool Intersects(BoundingBox other)
        {
            return MinX < other.MaxX &&
                   MaxX > other.MinX &&
                   MinY < other.MaxY &&
                   MaxY > other.MinY;
        }

        public bool Contains(Vector2D point)
        {
            return point.X >= MinX &&
                   point.X <= MaxX &&
                   point.Y >= MinY &&
                   point.Y <= MaxY;
        }

        public override string ToString()
        {
            return "AABB[min=" + MinX.ToString("0.###", CultureInfo.InvariantCulture) + "," + MinY.ToString("0.###", CultureInfo.InvariantCulture) +
                   " max=" + MaxX.ToString("0.###", CultureInfo.InvariantCulture) + "," + MaxY.ToString("0.###", CultureInfo.InvariantCulture) + "]";
        }
    }
}
