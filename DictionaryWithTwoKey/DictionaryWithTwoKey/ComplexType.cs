using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryWithTwoKey
{

    public class ComplexType<TKey1, TKey2> : IEquatable<ComplexType<TKey1, TKey2>>
    {
        private readonly TKey1 _key1;
        private readonly TKey2 _key2;

        public ComplexType(TKey1 key1, TKey2 key2)
        {
            _key2 = key2;
            _key1 = key1;
        }

        public bool Equals(ComplexType<TKey1, TKey2> other)
        {
            if (other == null)
                return false;

            return other._key1.Equals(_key1) && other._key2.Equals(_key2); ;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ComplexType<TKey1, TKey2>);
        }

        public override int GetHashCode()
        {
            var result = 17;

                result = 31 * result + _key1.GetHashCode();
                result = 31 * result + _key2.GetHashCode();

            return result;
        }
    }
}
