using System;
using System.Collections;
using System.Collections.Generic;

namespace kirypto.AdventOfCode.Common.Collections {
    public class QueueSet<T> : ICollection, IReadOnlyCollection<T>, ISet<T> {
        private readonly Queue<T> _queue = new();
        private readonly HashSet<T> _set = [];

        public int Count => _queue.Count;
        public bool IsSynchronized => false;
        public bool IsReadOnly => false;
        public object SyncRoot => throw new NotImplementedException();

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_queue).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void Add(T item) => AddInternal(item);
        bool ISet<T>.Add(T item) => AddInternal(item);
        public bool Contains(T item) => _set.Contains(item);

        public bool RemoveFirst(out T item) {
            if (_queue.Count == 0) {
                item = default;
                return false;
            }
            item = _queue.Dequeue();
            return _set.Remove(item);
        }

        public bool Remove(T item) {
            if (_queue.Peek().Equals(item)) {
                return RemoveFirst(out _);
            }

            throw new NotImplementedException($"Removing a non-first item from {nameof(QueueSet<T>)} is not implemented.");
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) => throw NotYet(nameof(IsProperSubsetOf));
        public bool IsProperSupersetOf(IEnumerable<T> other) => throw NotYet(nameof(IsProperSupersetOf));
        public bool IsSubsetOf(IEnumerable<T> other) => throw NotYet(nameof(IsSubsetOf));
        public bool IsSupersetOf(IEnumerable<T> other) => throw NotYet(nameof(IsSupersetOf));
        public bool Overlaps(IEnumerable<T> other) => throw NotYet(nameof(Overlaps));
        public bool SetEquals(IEnumerable<T> other) => throw NotYet(nameof(SetEquals));
        public void ExceptWith(IEnumerable<T> other) => throw NotYet(nameof(ExceptWith));
        public void IntersectWith(IEnumerable<T> other) => throw NotYet(nameof(IntersectWith));
        public void SymmetricExceptWith(IEnumerable<T> other) => throw NotYet(nameof(SymmetricExceptWith));
        public void UnionWith(IEnumerable<T> other) => throw NotYet(nameof(UnionWith));
        public void CopyTo(T[] array, int arrayIndex) => throw NotYet(nameof(CopyTo));
        public void Clear() => throw NotYet(nameof(Clear));
        public void CopyTo(Array array, int index) => throw NotYet(nameof(CopyTo));

        private static NotImplementedException NotYet(string methodName) =>
                new NotImplementedException($"{nameof(QueueSet<T>)}.{methodName} is not implemented.");

        private bool AddInternal(T item) {
            bool added = _set.Add(item);
            if (added) {
                _queue.Enqueue(item);
            }
            return added;
        }
    }
}
