using System;
using System.Collections.Generic;
using GeniusCode.Components.Delegates;

namespace GeniusCode.Components.Support.Comparison
{
    public class gcGenericEqualityComparer<T, TValue> : IEqualityComparer<T>
        //where T : class  (could also be a struct)
    {
        readonly Func<T, TValue> _getKey;
        readonly EqualityComparerDelegate<TValue> _keyComparer;
        readonly GetHashcodeDelegate<TValue> _hashcode;

        public gcGenericEqualityComparer(Func<T, TValue> getKey)
            : this(getKey, null, null)
        {
        }

        public gcGenericEqualityComparer(Func<T, TValue> getKey, EqualityComparerDelegate<TValue> keyComparer, GetHashcodeDelegate<TValue> hashcode)
        {
            _keyComparer = keyComparer ?? ((x, y) => EqualityComparer<TValue>.Default.Equals(x, y));
            _getKey = getKey;
            _hashcode = hashcode ?? (a => a.GetHashCode());

        }

        public bool Equals(T x, T y)
        {
            var leftKey = _getKey(x);
            var rightKey = _getKey(y);
            return _keyComparer(leftKey, rightKey);
        }

        public int GetHashCode(T obj)
        {
            var toHash = _getKey(obj);
            return _hashcode(toHash);
        }
    }



    public class GcGenericEqualityComparer<T> : IEqualityComparer<T>
    {

        public GcGenericEqualityComparer()
            : this(null, null)
        {
        }
        public GcGenericEqualityComparer(EqualityComparerDelegate<T> equals, GetHashcodeDelegate<T> hashCode)
        {
            _equals = equals ?? ((a, b) => EqualityComparer<T>.Default.Equals(a, b));
            _hashcode = hashCode ?? ((a) => a.GetHashCode());
        }


        private readonly GetHashcodeDelegate<T> _hashcode;
        private readonly EqualityComparerDelegate<T> _equals;


        public bool Equals(T x, T y)
        {
            return _equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hashcode(obj);
        }
    }

}