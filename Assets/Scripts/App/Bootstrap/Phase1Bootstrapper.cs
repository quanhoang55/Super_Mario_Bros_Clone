using System;
using System.IO;
using SuperMario.Core.Domain;
using SuperMario.Core.Levels;
using SuperMario.Core.Save;
using SuperMario.Data.Levels;
using SuperMario.Data.Save;
using UnityEngine;

namespace SuperMario.App.Bootstrap
{
    /// <summary>
    /// Phase 1 smoke test. Attach this component to any GameObject in a scene and press Play:
    /// 1) loads StreamingAssets/Levels/level_01.json through <see cref="JsonLevelLoader"/> and logs every entity;
    /// 2) performs a Save → Load round-trip through <see cref="JsonSaveStorage"/> and logs PASSED/FAILED.
    /// </summary>
    public sealed class Phase1Bootstrapper : MonoBehaviour
    {
        [Header("Level file")]
        [SerializeField] private string levelsFolder = "Levels";
        [SerializeField] private string levelFileName = "level_01.json";

        [Header("Save/Load smoke test")]
        [SerializeField] private bool runSaveLoadSmokeTest = true;

        private void Start()
        {
            LoadLevel();

            if (runSaveLoadSmokeTest)
                RunSaveLoadSmokeTest();
        }

        private void LoadLevel()
        {
            string path = Path.Combine(Application.streamingAssetsPath, levelsFolder, levelFileName);
            JsonLevelLoader loader = new JsonLevelLoader();
            LevelDefinition level = loader.Load(path);

            Debug.Log("[Phase1] Loaded '" + level.Name + "' (World " + level.World + "-" + level.Level +
                      ") with " + level.Entities.Count + " entities from " + path);

            for (int i = 0; i < level.Entities.Count; i++)
            {
                EntitySpawn entity = level.Entities[i];
                string variant = string.IsNullOrEmpty(entity.Variant) ? string.Empty : " [" + entity.Variant + "]";
                Debug.Log("  - " + entity.Type + variant + " @ " + entity.Position + " size " + entity.Size);
            }
        }

        private void RunSaveLoadSmokeTest()
        {
            string path = Path.Combine(Application.persistentDataPath, "save_slot_1.json");
            JsonSaveStorage storage = new JsonSaveStorage(path);

            GameSnapshot snapshot = new GameSnapshot
            {
                LevelId = "level_01",
                World = 1,
                Level = 1,
                PlayerPosition = new Vector2D(-8f, -1f),
                Lives = 3,
                Score = 1234,
                Coins = 7,
                TimeRemaining = 400,
                SavedAtUtc = DateTime.UtcNow.ToString("o")
            };

            storage.Save(snapshot);
            GameSnapshot loaded = storage.Load();

            bool passed = loaded != null &&
                          loaded.Score == snapshot.Score &&
                          loaded.Lives == snapshot.Lives &&
                          loaded.PlayerPosition == snapshot.PlayerPosition;

            Debug.Log("[Phase1] Save/Load round-trip " + (passed ? "PASSED" : "FAILED") + " -> " + path);
        }
    }
}
