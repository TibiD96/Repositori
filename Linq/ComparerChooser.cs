using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    internal class ComparerChooser<TSource> : IComparer<TSource>
    {
        readonly IComparer<TSource> firstComparer;
        readonly IComparer<TSource> secondComparer;

        public ComparerChooser(IComparer<TSource> firstComparer, IComparer<TSource> secondComparer)
        {
            this.firstComparer = firstComparer;
            this.secondComparer = secondComparer;
        }

        public int Compare(TSource first, TSource second)
        {
            var firstComparerResult = firstComparer.Compare(first, second);

            if (firstComparerResult == 0)
            {
                return firstComparerResult;
            }

            return secondComparer.Compare(first, second);
        }
    }
}