using System;
using System.Collections.Generic;

namespace DictionaryWithTwoKey
{
    public class MyDictionary<TKey1, TKey2, TValue>
    {

        private readonly Dictionary<int, TValue> _values;

        private readonly Dictionary<TKey1, List<TKey2>> _keys1;

        private readonly Dictionary<TKey2, List<TKey1>> _keys2;

        public MyDictionary()
        {
            _keys1 = new Dictionary<TKey1, List<TKey2>>();
            _keys2 = new Dictionary<TKey2, List<TKey1>>();
            _values = new Dictionary<int, TValue>();
        }

        public void AddOrUpdate(TKey1 key1, TKey2 key2, TValue value)
        {
            var hash = GetHash(key1, key2);
            if(!_values.ContainsKey(hash))
                AddToKeys(key1, key2);

            _values[hash] = value;
        }

        private int GetHash(TKey1 key1, TKey2 key2)
        {
            return new Tuple<TKey1, TKey2>(key1, key2).GetHashCode();
        }

        private void AddToKeys(TKey1 key1, TKey2 key2)
        {
            if (!_keys1.ContainsKey(key1))
                _keys1[key1] = new List<TKey2>();

            _keys1[key1].Add(key2);

            if (!_keys2.ContainsKey(key2))
                _keys2[key2] = new List<TKey1>();

            _keys2[key2].Add(key1);
        }

        public void Remove(TKey1 key1, TKey2 key2)
        {
            var hash = GetHash(key1, key2);

            if(_values.Remove(hash))
                RemoveKeys(key1, key2);
        }

        private void RemoveKeys(TKey1 key1, TKey2 key2)
        {
            _keys1[key1].Remove(key2);
            _keys2[key2].Remove(key1);
        }

        public TValue this [TKey1 key1, TKey2 key2]
        {
            get => _values[GetHash(key1, key2)];

            set => AddOrUpdate(key1, key2, value);
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2) => _values.ContainsKey(GetHash(key1, key2));

        public bool ContainsKey(TKey2 key2) => _keys2.ContainsKey(key2);

        public bool ContainsKey(TKey1 key1) => _keys1.ContainsKey(key1);

        public IEnumerable<TValue> GetValues(TKey1 key1)
        {
            if(!ContainsKey(key1))
                yield break;

            foreach (var key2 in _keys1[key1])
            {
                yield return this[key1, key2];
            }
        }

        public IEnumerable<TValue> GetValues(TKey2 key2)
        {
            if (!ContainsKey(key2))
                yield break;

            foreach (var key1 in _keys2[key2])
            {
                yield return this[key1, key2];
            }
        }

        public IEnumerable<TValue> GetValues()
        {
            foreach (var value in _values.Values)
            {
                yield return value;
            }
        }

        public void Printe()
        {
            foreach (var key1 in _keys1)
            {
                foreach (var key2 in key1.Value)
                {
                    Console.WriteLine($"[{key1.Key}, {key2}] = {_values[GetHash(key1.Key, key2)]}");
                }
            }
        }
    }
}
