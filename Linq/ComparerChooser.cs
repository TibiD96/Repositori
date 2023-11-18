using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    internal class ComparerChooser<TKey> : IComparer<TKey>
    {
        readonly IComparer<TKey> firstComparer;
        readonly IComparer<TKey> secondComparer;

        public ComparerChooser(IComparer<TKey> firstComparer, IComparer<TKey> secondComparer)
        {
            this.firstComparer = firstComparer;
            this.secondComparer = secondComparer;
        }

        public int Compare(TKey first, TKey second)
        {
            var firstComparerResult = firstComparer.Compare(first, second);

            if (firstComparerResult != 0)
            {
                return firstComparerResult;
            }

            return secondComparer.Compare(first, second);
        }
    }
}