using System;
using System.Collections.Generic;

namespace Linq
{
    internal class BaseComparer<TSource, TKey> : IComparer<TSource>
    {
        readonly Func<TSource, TKey> keySelector;
        readonly IComparer<TKey> comparer;

        public BaseComparer(Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        public int Compare(TSource first, TSource second)
        {
            return comparer.Compare(keySelector(first), keySelector(second));
        }
    }
}
