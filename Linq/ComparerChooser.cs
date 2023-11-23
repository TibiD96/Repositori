using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    internal class ComparerChooser<TSource, TKey> : IComparer<TSource>
    {
        readonly Func<TSource, TKey> keySelector;
        readonly IComparer<TKey> comparer;

        public ComparerChooser(Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
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