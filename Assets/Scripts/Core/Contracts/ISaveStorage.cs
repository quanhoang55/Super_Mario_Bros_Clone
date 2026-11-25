using SuperMario.Core.Save;

namespace SuperMario.Core.Contracts
{
    /// <summary>
    /// Contract for persisting and restoring a full game snapshot.
    /// Implemented by <c>JsonSaveStorage</c> in SuperMario.Data.
    /// </summary>
    public interface ISaveStorage
    {
        /// <summary>True when a save file is available.</summary>
        bool Exists { get; }

        /// <summary>Writes the snapshot to the underlying storage.</summary>
        void Save(GameSnapshot snapshot);

        /// <summary>Reads the last snapshot from the underlying storage.</summary>
        GameSnapshot Load();

        /// <summary>Deletes the stored snapshot, if any.</summary>
        void Delete();
    }
}
