﻿using System;
using System.Linq;

namespace Linq
{
    public static class LinqMethodes
    {
        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(predicate, nameof(predicate));

            foreach (var element in source)
            {
                if (!predicate(element))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(predicate, nameof(predicate));

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return true;
                }
            }

            return false;
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(predicate, nameof(predicate));

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            throw new InvalidOperationException("No element respect condition");
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(selector, nameof(selector));

            foreach (var element in source)
            {
                yield return selector(element);
            }
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(selector, nameof(selector));

            foreach (var element in source)
            {
                foreach (var item in selector(element))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(predicate, nameof(predicate));

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    yield return element;
                }
            }
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(keySelector, nameof(keySelector));
            CheckIfNull(elementSelector, nameof(elementSelector));

            var dictionary = new Dictionary<TKey, TElement>();

            foreach (var element in source)
            {
                dictionary.Add(keySelector(element), elementSelector(element));
            }

            return dictionary;
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            CheckIfNull(first, nameof(first));
            CheckIfNull(second, nameof(second));
            CheckIfNull(resultSelector, nameof(resultSelector));

            var firstList = first.GetEnumerator();
            var secondList = second.GetEnumerator();

            while (firstList.MoveNext() && secondList.MoveNext())
            {
                yield return resultSelector(firstList.Current, secondList.Current);
            }
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(seed, nameof(seed));
            CheckIfNull(func, nameof(func));

            TAccumulate agregate = seed;

            foreach (var element in source)
            {
                agregate = func(agregate, element);
            }

            return agregate;
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            CheckIfNull(outer, nameof(outer));
            CheckIfNull(inner, nameof(inner));
            CheckIfNull(outerKeySelector, nameof(outerKeySelector));
            CheckIfNull(innerKeySelector, nameof(innerKeySelector));
            CheckIfNull(resultSelector, nameof(resultSelector));

            foreach (var element in outer)
            {
                foreach (var item in inner)
                {
                    if (outerKeySelector(element).Equals(innerKeySelector(item)))
                    {
                        yield return resultSelector(element, item);
                    }
                }
            }
        }

        public static IEnumerable<TSource> Distinct<TSource>(
            this IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(comparer, nameof(comparer));

            return new HashSet<TSource>(source, comparer);
        }

        public static IEnumerable<TSource> Union<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            CheckIfNull(first, nameof(first));
            CheckIfNull(second, nameof(second));
            CheckIfNull(comparer, nameof(comparer));

            var result = new HashSet<TSource>(first, comparer);
            result.UnionWith(new HashSet<TSource>(second, comparer));
            return result;
        }

        public static IEnumerable<TSource> Intersect<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            CheckIfNull(first, nameof(first));
            CheckIfNull(second, nameof(second));
            CheckIfNull(comparer, nameof(comparer));

            var result = new HashSet<TSource>(first, comparer);
            result.IntersectWith(new HashSet<TSource>(second, comparer));
            return result;
        }

        public static IEnumerable<TSource> Expect<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            CheckIfNull(first, nameof(first));
            CheckIfNull(second, nameof(second));
            CheckIfNull(comparer, nameof(comparer));

            var result = new HashSet<TSource>(first, comparer);
            result.ExceptWith(new HashSet<TSource>(second, comparer));
            return result;
        }

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(keySelector, nameof(keySelector));
            CheckIfNull(elementSelector, nameof(elementSelector));
            CheckIfNull(resultSelector, nameof(resultSelector));
            CheckIfNull(comparer, nameof(comparer));

            var dataBase = new Dictionary<TKey, List<TElement>>();
            foreach (var element in source)
            {
                var key = keySelector(element);

                if (!dataBase.ContainsKey(key))
                {
                    dataBase.Add(key, new List<TElement>());
                }

                dataBase[key].Add(elementSelector(element));
            }

            foreach (var data in dataBase)
            {
                yield return resultSelector(data.Key, data.Value);
            }
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(keySelector, nameof(keySelector));
            CheckIfNull(comparer, nameof(comparer));

            Func<TSource, TSource, int> functionForComparer = (first, second) =>
            {
                TKey firstKey = keySelector(first);
                TKey secondKey = keySelector(second);

                return comparer.Compare(firstKey, secondKey);
            };

            return new OrderedEnumerable<TSource>(source, new ComparerChooser<TSource>(functionForComparer));
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
             this IOrderedEnumerable<TSource> source,
             Func<TSource, TKey> keySelector,
             IComparer<TKey> comparer)
        {
            CheckIfNull(source, nameof(source));
            CheckIfNull(keySelector, nameof(keySelector));
            CheckIfNull(comparer, nameof(comparer));

            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        static void CheckIfNull<T>(T input, string nullReturn)
        {
            if (input != null)
            {
                return;
            }

            throw new ArgumentNullException(nullReturn);
        }
    }
}