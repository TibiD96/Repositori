using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    internal class ComparerChooser<TSource, TKey> : IComparer<TSource>
    {
        readonly Func<TSource, TKey> keySelector;
        readonly IComparer<TKey> firstComparer;
        readonly IComparer<TKey> secondComparer;

        public ComparerChooser(Func<TSource, TKey> keySelector, IComparer<TKey> firstComparer, IComparer<TKey> secondComparer)
        {
            this.keySelector = keySelector;
            this.firstComparer = firstComparer;
            this.secondComparer = secondComparer;
        }

        public int Compare(TSource first, TSource second)
        {
            var firstComparerResult = firstComparer.Compare(keySelector(first), keySelector(second));

            if (firstComparerResult == 0)
            {
                return firstComparerResult;
            }

            return secondComparer.Compare(keySelector(first), keySelector(second));
        }
    }
}