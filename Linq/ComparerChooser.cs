using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    internal class ComparerChooser<TSource, TKey> : IComparer<TSource>
    {
        readonly Func<TSource, TKey> keySelector;

        public ComparerChooser(Func<TSource, TKey> keySelector)
        {
            this.keySelector = keySelector;
        }

        public int Compare(TSource first, TSource second)
        {
            TKey firstKey = keySelector(first);
            TKey secondKey = keySelector(second);

            return ((IComparable<TKey>)firstKey).CompareTo(secondKey);
        }
    }
}