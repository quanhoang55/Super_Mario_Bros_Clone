using System;
using System.Collections.Generic;

namespace SuperMario.Core.Save
{
    /// <summary>
    /// Root object stored in highscores.json.
    /// </summary>
    [Serializable]
    public class HighScoreTable
    {
        public List<HighScoreEntry> Entries = new List<HighScoreEntry>();
    }
}
