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
            var newComparer = new ComparerChooser<TSource>((first, second) =>
            {
                var finalcomp = this.comparer.Compare(first, second);

                return finalcomp == 0 ? comparer.Compare(keySelector(first), keySelector(second)) : finalcomp;
            });

            return new OrderedEnumerable<TSource>(source, newComparer);
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var list = source.ToList();

            NonRecursiveQuickSort(list, 0, list.Count - 1);

            foreach (var element in list)
            {
                yield return element;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void QuickSort(List<TSource> list, int left, int right)
        {
            if (left >= right)
            {
                return;
            }

            int listSplitingIndex = Partitioning(list, left, right);
            QuickSort(list, left, listSplitingIndex - 1);
            QuickSort(list, listSplitingIndex + 1, right);
        }

        private void NonRecursiveQuickSort(List<TSource> list, int left, int right)
        {
            int[] lifoList = new int[list.Count];
            int elementFromTop = -1;

            lifoList[++elementFromTop] = left;
            lifoList[++elementFromTop] = right;

            while (elementFromTop >= 0)
            {
                right = lifoList[elementFromTop--];
                left = lifoList[elementFromTop--];

                int listSplitingIndex = Partitioning(list, left, right);

                if (listSplitingIndex - 1 > left)
                {
                    lifoList[++elementFromTop] = left;
                    lifoList[++elementFromTop] = listSplitingIndex - 1;
                }

                if (listSplitingIndex + 1 < right)
                {
                    lifoList[++elementFromTop] = listSplitingIndex + 1;
                    lifoList[++elementFromTop] = right;
                }
            }
        }

        private int Partitioning(List<TSource> list, int left, int right)
        {
            int pivot = right;

            int indexSmallestElem = left - 1;

            for (int i = left; i <= right; i++)
            {
                if (comparer.Compare(list[i], list[pivot]) < 0)
                {
                    indexSmallestElem++;
                    Swap(list, indexSmallestElem, i);
                }
            }

            Swap(list, indexSmallestElem + 1, right);
            return indexSmallestElem + 1;
        }

        private void Swap(List<TSource> list, int elemToBeSwap, int elemToSwapWith)
        {
            (list[elemToSwapWith], list[elemToBeSwap]) = (list[elemToBeSwap], list[elemToSwapWith]);
        }
    }
}