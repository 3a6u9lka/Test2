using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DictionaryWithTwoKey
{
    public class MyConcurrentDictionaryV3<TKey1, TKey2, TValue>
    {
        private readonly Dictionary<TKey1, Dictionary<TKey2, TValue>> _keys1;

        private readonly Dictionary<TKey2, Dictionary<TKey1, TValue>> _keys2;

        public MyConcurrentDictionaryV3()
        {
            _keys1 = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
            _keys2 = new Dictionary<TKey2, Dictionary<TKey1, TValue>>();
        }

        public void AddOrUpdate(TKey1 key1, TKey2 key2, TValue value)
        {
            lock (this)
            {
                if (!_keys1.ContainsKey(key1))
                    _keys1[key1] = new Dictionary<TKey2, TValue>();
                _keys1[key1][key2] = value;

                if (!_keys2.ContainsKey(key2))
                    _keys2[key2] = new Dictionary<TKey1, TValue>();
                _keys2[key2][key1] = value;
            }
        }

        public void Remove(TKey1 key1, TKey2 key2)
        {
            lock (this)
            {
                if (_keys1.ContainsKey(key1))
                    _keys1[key1].Remove(key2);

                if (_keys2.ContainsKey(key2))
                    _keys2[key2].Remove(key1);
            }

        }

        public TValue this[TKey1 key1, TKey2 key2]
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

        public bool ContainsKey(TKey1 key1) => _keys1.ContainsKey(key1);

        public bool ContainsKey(TKey2 key2) => _keys2.ContainsKey(key2);

        public IEnumerable<TValue> GetValues(TKey1 key1)
        {
            lock (_keys1)
            {
                return _keys1[key1].Values;
            }
        }

        public IEnumerable<TValue> GetValues(TKey2 key2)
        {
            lock (_keys2)
            {
                return _keys2[key2].Values;
            }
        }

        public IEnumerable<TValue> GetValues()
        {
            lock (this)
            {
                foreach (var keys2 in _keys1.Values)
                {
                    foreach (var value in keys2.Values)
                    {
                        yield return value;
                    }
                }
            }
            
        }

        public void Clear()
        {
            lock (this)
            {
                _keys1.Clear();
                _keys2.Clear();
            }
        }
    }
}
