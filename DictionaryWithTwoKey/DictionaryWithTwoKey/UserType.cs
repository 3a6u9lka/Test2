using System;
using System.Collections.Generic;

namespace DictionaryWithTwoKey
{
    public class UserType : IEquatable<UserType>
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public override int GetHashCode()
        {
            var result = 17;

                result = 31 * result + EqualityComparer<int>.Default.GetHashCode(Age);
                result = 31 * result + EqualityComparer<string>.Default.GetHashCode(Name);

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserType);
        }

        public bool Equals(UserType other)
        {
            if(other == null)
                return false;

            return other.Name == Name && other.Age == Age;
        }

        public override string ToString()
        {
            return $"Name = {Name}, age = {Age}";
        }
    }
}
