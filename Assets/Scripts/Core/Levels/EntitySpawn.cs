using System;
using SuperMario.Core.Domain;

namespace SuperMario.Core.Levels
{
    /// <summary>
    /// One entity entry inside a level file (ground, block, enemy, item, goal...).
    /// </summary>
    [Serializable]
    public class EntitySpawn
    {
        public string Id;
        public string Type;
        public Vector2D Position;
        public Vector2D Size;
        public string Variant;
        public float PatrolRange;
        public int Health;
    }
}
