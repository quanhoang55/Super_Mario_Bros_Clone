using System;
using System.Collections.Generic;
using SuperMario.Core.Domain;

namespace SuperMario.Core.Save
{
    /// <summary>
    /// Complete, serializable snapshot of a running game (save_slot_1.json).
    /// </summary>
    [Serializable]
    public class GameSnapshot
    {
        public string LevelId;
        public int World;
        public int Level;
        public Vector2D PlayerPosition;
        public int Lives;
        public int Score;
        public int Coins;
        public int TimeRemaining;
        public string SavedAtUtc;
        public List<EntityState> Entities = new List<EntityState>();
    }
}
