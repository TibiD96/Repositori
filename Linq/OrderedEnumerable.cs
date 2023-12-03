using System;
using System.Collections;

namespace Linq
{
    public class OrderedEnumerable<TSource> : IOrderedEnumerable<TSource>
    {
        private readonly IEnumerable<TSource> source;
        private readonly IComparer<TSource> comparer;

        public OrderedEnumerable(IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            this.source = source;
            this.comparer = comparer;
        }

        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            Func<TSource, TSource, int> functionForComparer = (first, second) =>
            {
                var finalcomp = this.comparer.Compare(first, second);
                TKey firstKey = keySelector(first);
                TKey secondKey = keySelector(second);

                if (finalcomp == 0)
                {
                    return comparer.Compare(firstKey, secondKey);
                }

                return finalcomp;
            };

            var newComparer = new ComparerChooser<TSource>(functionForComparer);
            return new OrderedEnumerable<TSource>(source, newComparer);
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var list = source.ToList();

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (comparer.Compare(list[j], list[j + 1]) > 0)
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