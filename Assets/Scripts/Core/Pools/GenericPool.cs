using System;
using System.Collections.Generic;

namespace SuperMario.Core.Pools
{
    /// <summary>
    /// Optional lifecycle hooks for objects managed by <see cref="GenericPool{T}"/>.
    /// </summary>
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }

    /// <summary>
    /// Engine-agnostic object pool (Phase 1 – task 1.6).
    /// Reuses bullets/enemies without allocating garbage inside Update().
    /// </summary>
    public sealed class GenericPool<T> where T : class
    {
        private readonly Func<T> _factory;
        private readonly int _maxSize;
        private readonly Stack<T> _available;
        private readonly HashSet<T> _inUse;

        public GenericPool(Func<T> factory, int initialCapacity = 8, int maxSize = 128)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException("initialCapacity");
            if (maxSize < 1)
                throw new ArgumentOutOfRangeException("maxSize");

            _factory = factory;
            _maxSize = maxSize;
            _available = new Stack<T>(initialCapacity);
            _inUse = new HashSet<T>();

            for (int i = 0; i < initialCapacity; i++)
            {
                _available.Push(CreateInstance());
            }
        }

        public int AvailableCount { get { return _available.Count; } }

        public int ActiveCount { get { return _inUse.Count; } }

        /// <summary>Takes an instance from the pool (creating one when the pool is empty).</summary>
        public T Get()
        {
            T item = _available.Count > 0 ? _available.Pop() : CreateInstance();
            _inUse.Add(item);

            IPoolable poolable = item as IPoolable;
            if (poolable != null)
                poolable.OnSpawn();

            return item;
        }

        /// <summary>Returns an instance to the pool. Throws when the item does not belong to this pool.</summary>
        public void Return(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (!_inUse.Remove(item))
                throw new InvalidOperationException("Item is not part of this pool.");

            IPoolable poolable = item as IPoolable;
            if (poolable != null)
                poolable.OnDespawn();

            if (_available.Count < _maxSize)
                _available.Push(item);
        }

        /// <summary>Drops every instance (used by the Replay flow).</summary>
        public void Clear()
        {
            _available.Clear();
            _inUse.Clear();
        }

        private T CreateInstance()
        {
            T instance = _factory();
            if (instance == null)
                throw new InvalidOperationException("Pool factory returned null.");

            return instance;
        }
    }
}
