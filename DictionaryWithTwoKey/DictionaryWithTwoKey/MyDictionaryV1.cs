using System;
using System.Collections.Generic;

namespace DictionaryWithTwoKey
{
    /// <summary>
    /// По тестам показал, что hash не уникальный и появляется коллизия, в данной реализации это не учтено из-за чего происходит перезатерание значений
    /// </summary>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MyDictionaryV1<TKey1, TKey2, TValue> : MyDictionary<TKey1, TKey2, TValue>
    {
        private readonly Dictionary<int, TValue> _values;

        private readonly Dictionary<TKey1, List<TKey2>> _keys1;

        private readonly Dictionary<TKey2, List<TKey1>> _keys2;

        public MyDictionaryV1()
        {
            _keys1 = new Dictionary<TKey1, List<TKey2>>();
            _keys2 = new Dictionary<TKey2, List<TKey1>>();
            _values = new Dictionary<int, TValue>();
        }

        public override void AddOrUpdate(TKey1 key1, TKey2 key2, TValue value)
        {
            var hash = GetHash(key1, key2);
            if(!_values.ContainsKey(hash))
                AddToKeys(key1, key2);

            _values[hash] = value;
        }

        private int GetHash(TKey1 key1, TKey2 key2)
        {
            var result = 17;

            result = 31 * result + EqualityComparer<TKey1>.Default.GetHashCode(key1);
            result = 31 * result + EqualityComparer<TKey2>.Default.GetHashCode(key2);

            return result;
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

        public override void Remove(TKey1 key1, TKey2 key2)
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

        public override TValue this [TKey1 key1, TKey2 key2]
        {
            get => _values[GetHash(key1, key2)];

            set => AddOrUpdate(key1, key2, value);
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2) => _values.ContainsKey(GetHash(key1, key2));

        public bool ContainsKey(TKey2 key2) => _keys2.ContainsKey(key2);

        public bool ContainsKey(TKey1 key1) => _keys1.ContainsKey(key1);

        public override IEnumerable<TValue> GetValues(TKey1 key1)
        {
            if(!ContainsKey(key1))
                yield break;

            foreach (var key2 in _keys1[key1])
            {
                yield return this[key1, key2];
            }
        }

        public override IEnumerable<TValue> GetValues(TKey2 key2)
        {
            if (!ContainsKey(key2))
                yield break;

            foreach (var key1 in _keys2[key2])
            {
                yield return this[key1, key2];
            }
        }

        public override IEnumerable<TValue> GetValues()
        {
            foreach (var value in _values.Values)
            {
                yield return value;
            }
        }

        public override void Printe()
        {
            foreach (var key1 in _keys1)
            {
                foreach (var key2 in key1.Value)
                {
                    Console.WriteLine($"[{key1.Key}, {key2}] = {_values[GetHash(key1.Key, key2)]}");
                }
            }
        }

        public override void Clear()
        {
            _keys1.Clear();
            _keys2.Clear();
            _values.Clear();
        }
    }
}
