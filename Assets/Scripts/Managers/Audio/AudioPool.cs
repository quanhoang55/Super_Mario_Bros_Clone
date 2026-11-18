using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoSingleton<AudioPool>
{
    [SerializeField] private int initialSize = 10;

    protected Stack<PoolableAudio> Available = new Stack<PoolableAudio>();
    protected int _activeCount;

    private void Awake()
    {
        if (initialSize < 1)
            initialSize = 10;

        AddItemsToPool();
    }

    private void AddItemsToPool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            PoolableAudio pooledAudio = new GameObject("PoolableAudio").AddComponent<PoolableAudio>();
            pooledAudio.gameObject.SetActive(false);
            Available.Push(pooledAudio);
        }
    }

    public PoolableAudio Get()
    {
        if (Available.Count == 0)
            AddItemsToPool();

        PoolableAudio pooledAudio = Available.Pop();
        pooledAudio.gameObject.SetActive(true);
        pooledAudio.Reset();
        _activeCount++;

        return pooledAudio;
    }

    public void Return(PoolableAudio source)
    {
        source.gameObject.SetActive(false);
        Available.Push(source);
        _activeCount--;
    }

    public int CountActive => _activeCount;
}
