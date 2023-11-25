using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    internal class ComparerChooser<TSource> : IComparer<TSource>
    {
        readonly Func<TSource, TSource, int> comparer;

        public ComparerChooser(Func<TSource, TSource, int> comparer)
        {
            this.comparer = comparer;
        }

        public int Compare(TSource first, TSource second)
        {
            TSource firstKey = first;
            TSource secondKey = second;

            return comparer(firstKey, secondKey);
        }
    }
}