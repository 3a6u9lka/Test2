using System;
using System.Collections.Generic;

namespace DictionaryWithTwoKey
{
    public abstract class MyDictionary<TKey1, TKey2, TValue>
    {
        public abstract void AddOrUpdate(TKey1 key1, TKey2 key2, TValue value);

        public abstract void Remove(TKey1 key1, TKey2 key2);

        public abstract IEnumerable<TValue> GetValues(TKey1 key1);

        public abstract IEnumerable<TValue> GetValues(TKey2 key2);

        public abstract IEnumerable<TValue> GetValues();

        public abstract void Printe();

        public abstract TValue this[TKey1 key1, TKey2 key2] { get; set; }

        public abstract void Clear();

    }
}
