using System;
using System.Collections.Generic;
using System.Linq;
using GeniusCode.Components.Delegates;
using GeniusCode.Components.Support.Comparison;

namespace GeniusCode.Components.Extensions
{
    public static class CollectionExtensions
    {

        public static void AddManyParams<T>(this List<T> list, params T[] args)
        {
            list.AddRange(args);
        }

        public static IEnumerable<T> CustomDistinct<T>(this IEnumerable<T> input, EqualityComparerDelegate<T> comparer, GetHashcodeDelegate<T> hashcode)
        {
            var c = new GcGenericEqualityComparer<T>(comparer, hashcode);
            return input.Distinct(c);
        }
        public static IEnumerable<T> CustomDistinctOn<T, TKey>(this IEnumerable<T> input, Func<T, TKey> keyAquirer)
            where T : class
        {
            return CustomDistinctOn(input, keyAquirer, null, null);
        }
        public static IEnumerable<T> CustomDistinctOn<T, TKey>(this IEnumerable<T> input, Func<T, TKey> keyAquirer, EqualityComparerDelegate<TKey> keyComparer, GetHashcodeDelegate<TKey> hashcode)
            where T : class
        {
            var c = new gcGenericEqualityComparer<T, TKey>(keyAquirer, keyComparer, hashcode);
            return input.Distinct(c);
        }

        public static void ForAll<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T obj in sequence)
                action(obj);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> input)
        {
            return input == null || input.Count() <= 0;
        }


        public static void ForAll<T>(this IEnumerable<T> sequnce, Action<T> action, Func<T, bool> predicate)
        {
            foreach (var obj in sequnce.Where(predicate))
                action(obj);
        }


        public static int AddIfUnique<TInput>(this HashSet<TInput> hashset, IEnumerable<TInput> input)
        {
            var count = 0;
            input.ForAll(o =>
            {
                if (hashset.AddIfUnique(o))
                    count++;
            });

            return count;
        }

        public static bool AddIfUnique<TInput>(this HashSet<TInput> hashset, TInput o)
        {
            if (!hashset.Contains(o))
            {
                hashset.Add(o);
                return true;
            }
            return false;
        }


        public static IEnumerable<Tuple<T1, T2>> FullOuterJoin<T1, T2>(this IEnumerable<T1> one, IEnumerable<T2> two, Func<T1, T2, bool> match)
        {
            var oneList = one.ToList();
            var twoList = two.ToList();

            var left = from a in oneList
                       from b in twoList.Where(b => match(a, b)).DefaultIfEmpty()
                       select new Tuple<T1, T2>(a, b);

            var right = from b in twoList
                        from a in oneList.Where(a => match(a, b)).DefaultIfEmpty()
                        select new Tuple<T1, T2>(a, b);

            return left.Union(right);
        }

    }
}
