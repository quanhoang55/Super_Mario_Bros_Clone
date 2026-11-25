using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SuperMario.Core.Exceptions;
using SuperMario.Core.Save;
using SuperMario.Data.Serialization;

namespace SuperMario.Data.HighScores
{
    /// <summary>
    /// Stores and retrieves the Top 10 high scores (Phase 1 – task 1.5).
    /// </summary>
    public sealed class HighScoreRepository
    {
        public const int MaxEntries = 10;

        private readonly string _filePath;
        private readonly JsonSerializerSettings _settings;

        public HighScoreRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("High score file path is required.", "filePath");

            _filePath = filePath;
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new FieldsOnlyCamelCaseContractResolver()
            };
        }

        public string FilePath { get { return _filePath; } }

        /// <summary>Returns the current Top 10, ordered from highest to lowest score.</summary>
        public IReadOnlyList<HighScoreEntry> GetTopScores()
        {
            HighScoreTable table = LoadTable();
            return table.Entries
                .OrderByDescending(entry => entry.Score)
                .Take(MaxEntries)
                .ToList();
        }

        /// <summary>
        /// Adds a score, keeps only the Top 10 and writes the file back.
        /// Returns true when the score made it into the table and outputs its 1-based rank.
        /// </summary>
        public bool TrySubmit(string playerName, int score, out int rank)
        {
            if (string.IsNullOrWhiteSpace(playerName))
                playerName = "PLAYER";

            HighScoreTable table = LoadTable();
            HighScoreEntry newEntry = new HighScoreEntry
            {
                PlayerName = playerName.Trim(),
                Score = score,
                AchievedAtUtc = DateTime.UtcNow.ToString("o")
            };

            table.Entries.Add(newEntry);
            table.Entries = table.Entries
                .OrderByDescending(entry => entry.Score)
                .Take(MaxEntries)
                .ToList();

            SaveTable(table);

            int index = table.Entries.IndexOf(newEntry);
            rank = index >= 0 ? index + 1 : 0;
            return index >= 0;
        }

        private HighScoreTable LoadTable()
        {
            if (!File.Exists(_filePath))
                return new HighScoreTable();

            string json;
            try
            {
                json = File.ReadAllText(_filePath);
            }
            catch (IOException exception)
            {
                throw new SaveCorruptedException("Cannot read high score file: " + _filePath, exception);
            }

            try
            {
                HighScoreTable table = JsonConvert.DeserializeObject<HighScoreTable>(json, _settings);
                if (table == null)
                    throw new SaveCorruptedException("High score file contains no data: " + _filePath);

                if (table.Entries == null)
                    table.Entries = new List<HighScoreEntry>();

                return table;
            }
            catch (JsonException exception)
            {
                throw new SaveCorruptedException("High score file is corrupted: " + _filePath, exception);
            }
        }

        private void SaveTable(HighScoreTable table)
        {
            try
            {
                string directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string json = JsonConvert.SerializeObject(table, _settings);
                File.WriteAllText(_filePath, json);
            }
            catch (IOException exception)
            {
                throw new SaveCorruptedException("Cannot write high score file: " + _filePath, exception);
            }
            catch (UnauthorizedAccessException exception)
            {
                throw new SaveCorruptedException("Access denied when writing high score file: " + _filePath, exception);
            }
        }
    }
}
