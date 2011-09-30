using System.Collections.Generic;
using GeniusCode.Components.Delegates;

namespace GeniusCode.Components.Support.Comparison
{
    public class gcGenericComparer<T> : IComparer<T>
    {
        public gcGenericComparer(ComparerDelegate<T> comparer)
        {
            _comparer = comparer;
        }

        private readonly ComparerDelegate<T> _comparer;

        public int Compare(T x, T y)
        {
            return _comparer(x, y);
        }
    }
}
