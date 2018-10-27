using System;
using System.Collections.Generic;

namespace DictionaryWithTwoKey
{
    /// <summary>
    /// в данной реализации существует два хранища значений, это может повлечь увеличении используемой памяти в зависимости от того что мы храним.
    /// </summary>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MyDictionaryV3<TKey1, TKey2, TValue> : MyDictionary<TKey1, TKey2, TValue>
    {
        private readonly Dictionary<TKey1, Dictionary<TKey2, TValue>> _keys1;

        private readonly Dictionary<TKey2, Dictionary<TKey1, TValue>> _keys2;

        public MyDictionaryV3()
        {
            _keys1 = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
            _keys2 = new Dictionary<TKey2, Dictionary<TKey1, TValue>>();
        }

        public override void AddOrUpdate(TKey1 key1, TKey2 key2, TValue value)
        {
            if(!_keys1.ContainsKey(key1))
                _keys1[key1] = new Dictionary<TKey2, TValue>();
            _keys1[key1][key2] = value;

            if (!_keys2.ContainsKey(key2))
                _keys2[key2] = new Dictionary<TKey1, TValue>();
            _keys2[key2][key1] = value;
        }

        public override void Remove(TKey1 key1, TKey2 key2)
        {
            if (_keys1.ContainsKey(key1))
                _keys1[key1].Remove(key2);

            if (_keys2.ContainsKey(key2))
                _keys2[key2].Remove(key1);
        }

        public override TValue this [TKey1 key1, TKey2 key2]
        {
            get => _keys1[key1][key2];

            set => AddOrUpdate(key1, key2, value);
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            if (!_keys1.ContainsKey(key1))
                return false;

            if (!_keys1[key1].ContainsKey(key2))
                return false;

            return true;
        }

        public bool ContainsKey(TKey2 key2) => _keys2.ContainsKey(key2);

        public bool ContainsKey(TKey1 key1) => _keys1.ContainsKey(key1);

        public override IEnumerable<TValue> GetValues(TKey1 key1)
        {
            return _keys1[key1].Values;
        }

        public override IEnumerable<TValue> GetValues(TKey2 key2)
        {
            return _keys2[key2].Values;
        }

        public override IEnumerable<TValue> GetValues()
        {
            foreach (var keys2 in _keys1.Values)
            {
                foreach (var value in keys2.Values)
                {
                    yield return value;
                }
            }
        }

        public override void Printe()
        {
            foreach (var key1 in _keys1)
            {
                foreach (var key2 in key1.Value)
                {
                    Console.WriteLine($"[{key1.Key}, {key2.Key}] = {key2.Value}");
                }
            }
        }

        public override void Clear()
        {
            _keys1.Clear();
            _keys2.Clear();
        }
    }
}
