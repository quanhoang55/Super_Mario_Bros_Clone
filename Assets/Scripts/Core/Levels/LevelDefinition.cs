using System;
using System.Collections.Generic;
using SuperMario.Core.Domain;

namespace SuperMario.Core.Levels
{
    /// <summary>
    /// JSON schema describing one playable level (loaded by <c>JsonLevelLoader</c>).
    /// </summary>
    [Serializable]
    public class LevelDefinition
    {
        public string Id;
        public string Name;
        public int World;
        public int Level;
        public int TimeLimit;
        public Vector2D PlayerSpawn;
        public Vector2D Goal;
        public List<EntitySpawn> Entities = new List<EntitySpawn>();
    }
}
