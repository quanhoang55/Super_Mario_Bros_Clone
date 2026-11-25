namespace SuperMario.Core.Contracts
{
    /// <summary>
    /// Contract used by the Core to push HUD state to any presentation layer
    /// (Unity UI, console, tests...).
    /// </summary>
    public interface IHudObserver
    {
        void OnScoreChanged(int score);
        void OnCoinsChanged(int coins);
        void OnLivesChanged(int lives);
        void OnTimeChanged(int seconds);
        void OnWorldChanged(int world, int level);
    }
}
