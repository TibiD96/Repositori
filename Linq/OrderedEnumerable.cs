using System;
using System.Collections;
using System.Collections.Generic;

namespace Linq
{
    public class OrderedEnumerable<TSource, TKey> : IOrderedEnumerable<TSource>
    {
        private readonly IEnumerable<TSource> source;
        private readonly Func<TSource, TKey> keySelector;
        private readonly IComparer<TKey> comparer;

        public OrderedEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            this.source = source;
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            var newComparer = new ComparerChooser<TKey>((IComparer<TKey>)this.comparer, comparer);
            return new OrderedEnumerable<TSource, TKey>(source, keySelector, newComparer);
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var list = source.ToList();

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    var itemOne = keySelector(list[j]);
                    var itemTwo = keySelector(list[j + 1]);

                    if (comparer.Compare(itemOne, itemTwo) > 0)
                    {
                        var temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }

            foreach (var element in list)
            {
                yield return element;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}