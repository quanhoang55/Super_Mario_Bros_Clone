using System;

namespace SuperMario.Core.Save
{
    /// <summary>
    /// One record of the Top 10 high score table (highscores.json).
    /// </summary>
    [Serializable]
    public class HighScoreEntry
    {
        public string PlayerName;
        public int Score;
        public string AchievedAtUtc;
    }
}
