using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SuperMario.Core.Exceptions;
using SuperMario.Core.Levels;
using SuperMario.Data.Serialization;

namespace SuperMario.Data.Levels
{
    /// <summary>
    /// Loads a <see cref="LevelDefinition"/> from a JSON file (Phase 1 – task 1.5).
    /// The class is engine-agnostic: it only depends on System.IO and Newtonsoft.Json.
    /// </summary>
    public sealed class JsonLevelLoader
    {
        private readonly JsonSerializerSettings _settings;

        public JsonLevelLoader()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new FieldsOnlyCamelCaseContractResolver()
            };
        }

        /// <summary>
        /// Reads and validates a level file.
        /// </summary>
        /// <exception cref="InvalidLevelDataException">The file is missing, unreadable or malformed.</exception>
        public LevelDefinition Load(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new InvalidLevelDataException("Level file path is empty.");

            if (!File.Exists(filePath))
                throw new InvalidLevelDataException("Level file not found: " + filePath);

            string json;
            try
            {
                json = File.ReadAllText(filePath);
            }
            catch (IOException exception)
            {
                throw new InvalidLevelDataException("Cannot read level file: " + filePath, exception);
            }
            catch (UnauthorizedAccessException exception)
            {
                throw new InvalidLevelDataException("Access denied when reading level file: " + filePath, exception);
            }

            LevelDefinition level;
            try
            {
                level = JsonConvert.DeserializeObject<LevelDefinition>(json, _settings);
            }
            catch (JsonException exception)
            {
                throw new InvalidLevelDataException("Malformed level JSON: " + filePath, exception);
            }

            if (level == null)
                throw new InvalidLevelDataException("Level JSON produced no data: " + filePath);

            Validate(level);
            return level;
        }

        private static void Validate(LevelDefinition level)
        {
            if (string.IsNullOrWhiteSpace(level.Id))
                throw new InvalidLevelDataException("Level 'id' is required.");

            if (level.TimeLimit <= 0)
                throw new InvalidLevelDataException("Level 'timeLimit' must be greater than zero.");

            if (level.Entities == null)
                level.Entities = new List<EntitySpawn>();
        }
    }
}
