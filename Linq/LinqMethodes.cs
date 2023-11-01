using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
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

            var zip = new List<TResult>();
            using (var firstList = first.GetEnumerator())
            using (var secondList = second.GetEnumerator())
            {
                while (firstList.MoveNext() && secondList.MoveNext())
                {
                    zip.Add(resultSelector(firstList.Current, secondList.Current));
                }
            }

            return zip;
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
