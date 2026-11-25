using System;
using SuperMario.Core.Domain;

namespace SuperMario.Core.Save
{
    /// <summary>
    /// Persisted state of a single dynamic entity (enemy, item, block...).
    /// </summary>
    [Serializable]
    public class EntityState
    {
        public string Id;
        public string Type;
        public Vector2D Position;
        public bool IsAlive;
        public int Health;
    }
}
