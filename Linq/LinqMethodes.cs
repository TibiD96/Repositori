using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

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

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,Func<TSource, TElement> elementSelector)
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

            foreach (var element in source)
            {
                seed = func(seed, element);
            }

            return seed;
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

            foreach (var element in source)
            {
                foreach (var item in source)
                {
                    if (comparer.Equals(element, item))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }

        public static IEnumerable<TSource> Union<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            CheckIfNull(first, nameof(first));
            CheckIfNull(second, nameof(second));
            CheckIfNull(comparer, nameof(comparer));

            foreach (var element in first)
            {
                foreach (var item in second)
                {
                    if (comparer.Equals(element, item))
                    {
                        break;
                    }

                    yield return element;
                }
            }
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
