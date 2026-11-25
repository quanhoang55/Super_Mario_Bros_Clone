using System;
using System.IO;
using Newtonsoft.Json;
using SuperMario.Core.Contracts;
using SuperMario.Core.Exceptions;
using SuperMario.Core.Save;
using SuperMario.Data.Serialization;

namespace SuperMario.Data.Save
{
    /// <summary>
    /// Persists a full <see cref="GameSnapshot"/> as JSON (Phase 1 – task 1.5).
    /// The target path is injected by the Unity layer (persistentDataPath, StreamingAssets...).
    /// </summary>
    public sealed class JsonSaveStorage : ISaveStorage
    {
        private readonly string _filePath;
        private readonly JsonSerializerSettings _settings;

        public JsonSaveStorage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Save file path is required.", "filePath");

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

        public bool Exists { get { return File.Exists(_filePath); } }

        /// <exception cref="SaveCorruptedException">The snapshot cannot be written.</exception>
        public void Save(GameSnapshot snapshot)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");

            try
            {
                string directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string json = JsonConvert.SerializeObject(snapshot, _settings);
                File.WriteAllText(_filePath, json);
            }
            catch (IOException exception)
            {
                throw new SaveCorruptedException("Cannot write save file: " + _filePath, exception);
            }
            catch (UnauthorizedAccessException exception)
            {
                throw new SaveCorruptedException("Access denied when writing save file: " + _filePath, exception);
            }
        }

        /// <exception cref="SaveCorruptedException">The file is missing or corrupted.</exception>
        public GameSnapshot Load()
        {
            if (!Exists)
                throw new SaveCorruptedException("Save file not found: " + _filePath);

            string json;
            try
            {
                json = File.ReadAllText(_filePath);
            }
            catch (IOException exception)
            {
                throw new SaveCorruptedException("Cannot read save file: " + _filePath, exception);
            }

            try
            {
                GameSnapshot snapshot = JsonConvert.DeserializeObject<GameSnapshot>(json, _settings);
                if (snapshot == null)
                    throw new SaveCorruptedException("Save file contains no data: " + _filePath);

                return snapshot;
            }
            catch (JsonException exception)
            {
                throw new SaveCorruptedException("Save file is corrupted: " + _filePath, exception);
            }
        }

        public void Delete()
        {
            if (Exists)
                File.Delete(_filePath);
        }
    }
}
