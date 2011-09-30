using System;
using System.Collections.Generic;

namespace GeniusCode.Components.Support.Collections
{
    public class SelfKeyingDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public SelfKeyingDictionary(Func<TValue, TKey> keyFunc)
        {
            KeyFunc = keyFunc;
        }

        public Func<TValue, TKey> KeyFunc { get; private set; }

        public void Add(TValue value)
        {
            var key = KeyFunc(value);
            Add(key, value);
        }

        public void Remove(TValue value)
        {
            var key = KeyFunc(value);
            Remove(key);
        }


    }
}
