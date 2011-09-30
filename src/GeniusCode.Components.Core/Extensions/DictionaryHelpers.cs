using System;
using System.Collections.Generic;
using System.Linq;
using GeniusCode.Components.Extensions;
using GeniusCode.Components.Support.Collections;

namespace GeniusCode.Framework.Extensions
{
    public static class DictionaryHelpers
    {



        public static bool DoIfKeyExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Action<TValue> action)
        {
            TValue output;
            if (dictionary.TryGetValue(key, out output))
            {
                action(output);
                return true;
            }

            return false;
        }

        public static TValue GetIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue elseValueToReturn)
        {
            TValue output;

            if (dictionary.TryGetValue(key, out output))
                return output;
            else
                return elseValueToReturn;
        }


        public static TValue CreateOrGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createDelegate)
        {
            bool wasCreated;
            return CreateOrGetValue<TKey, TValue>(dictionary, key, createDelegate, out wasCreated);
        }
        public static TValue CreateOrGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createDelegate, out bool wasCreated)
        {
            TValue output;
            if (dictionary.TryGetValue(key, out output))
            {
                wasCreated = false;
                return output;
            }
            else
            {
                TValue result = createDelegate();
                dictionary.Add(key, result);
                wasCreated = true;
                return result;
            }
        }

        /// <summary>
        /// Adds if unique to Dictionary.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="input">The input.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>Count of unique items added</returns>
        public static int AddIfUniqueMany<TInput, TKey>(this Dictionary<TKey, TInput> dictionary, IEnumerable<TInput> input, Func<TInput, TKey> keySelector)
        {
            int count = 0;
            input.ForAll(o =>
                {
                    if (AddIfUnique<TInput, TKey>(dictionary, o, keySelector))
                        count++;
                });

            return count;
        }


        /// <summary>
        /// Adds if unique to Dictonary.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="input">The input.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>whether or not item was added</returns>
        public static bool AddIfUnique<TInput, TKey>(this Dictionary<TKey, TInput> dictionary, TInput input, Func<TInput, TKey> keySelector)
        {
            TKey key = keySelector(input);
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, input);
                return true;
            }
            return false;
        }


        public static void Add<TInput, TKey>(this Dictionary<TKey, TInput> dictionary, TInput input, Func<TInput, TKey> keySelector)
        {
            TKey key = keySelector(input);
            dictionary.Add(key, input);
        }



        public static int AddIfUniqueOrReplace<TInput, TKey>(this Dictionary<TKey, TInput> dictionary, IEnumerable<TInput> input, Func<TInput, TKey> keySelector)
        {
            Func<TInput, TInput, bool> replaceFunc = (a, b) => true;
            return AddIfUniqueOrReplaceIf<TInput, TKey>(dictionary, input, keySelector, replaceFunc);
        }
        public static int AddIfUniqueOrReplaceIf<TInput, TKey>(this Dictionary<TKey, TInput> dictionary, IEnumerable<TInput> input, Func<TInput, TKey> keySelector, Func<TInput, TInput, bool> replaceFunc)
        {
            int count = 0;
            input.ForAll(o =>
            {
                if (AddIfUniqueOrReplaceIf<TInput, TKey>(dictionary, o, keySelector, replaceFunc))
                    count++;
            });

            return count;
        }



        public static bool AddIfUniqueOrReplace<TInput, TKey>(this Dictionary<TKey, TInput> dictionary, TInput input, Func<TInput, TKey> keySelector)
        {
            Func<TInput, TInput, bool> replaceFunc = (a, b) => true;
            return AddIfUniqueOrReplaceIf<TInput, TKey>(dictionary, input, keySelector, replaceFunc);
        }
        public static bool AddIfUniqueOrReplaceIf<TInput, TKey>(this Dictionary<TKey, TInput> dictionary, TInput input, Func<TInput, TKey> keySelector, Func<TInput, TInput, bool> replaceFunc)
        {
            if (AddIfUnique(dictionary, input, keySelector))
                return true;
            else
            {
                TKey key = keySelector(input);
                TInput existingValue = dictionary[key];

                if (replaceFunc(existingValue, input))
                {
                    dictionary[key] = input;
                    return true;
                }
                else
                    return false;
            }
        }


        public static SelfKeyingDictionary<K, V> ToSelfKeyingDictionary<K, V>(this IEnumerable<V> input, Func<V, K> keySelector)
        {
            var output = new SelfKeyingDictionary<K, V>(keySelector);
            input.ToList().ForEach(output.Add);
            return output;
        }


        /// <summary>
        /// Selects the intersection between one dictonary and aother based on keys they both have in common.
        /// </summary>
        /// <typeparam name="K">Key Type of the dictonaries</typeparam>
        /// <typeparam name="V1">Value type of the first dictionary</typeparam>
        /// <typeparam name="V2">Value type of the second dictionary</typeparam>
        /// <typeparam name="R">Result type that will be returned</typeparam>
        /// <param name="input">First dictionary</param>
        /// <param name="toCompare">Second dictionary</param>
        /// <param name="toSelect">Results</param>
        /// <returns></returns>
        public static IEnumerable<R> SelectIntersectWithDictionaryKeys<K, V1, V2, R>(this IDictionary<K, V1> input, IDictionary<K, V2> toCompare, Func<K, V1, V2, R> toSelect)
        {
            return from i in input
                   join tc in toCompare on i.Key equals tc.Key
                   select toSelect(tc.Key, i.Value, tc.Value);
        }

        /// <summary>
        /// Selects from dictionary, except where the keys of another dictionary match
        /// </summary>
        /// <typeparam name="K">Key Type of the dictionaries</typeparam>
        /// <typeparam name="V1">Value type of the first dictionary (will be subtracted from)</typeparam>
        /// <typeparam name="V2">Value type of the second dictionary (will act as the filter)</typeparam>
        /// <typeparam name="R">Result from which to select</typeparam>
        /// <param name="input">first dictonary (will be subtracted from)</param>
        /// <param name="toCompare">filter dictionary</param>
        /// <param name="toSelect">To select.</param>
        /// <returns></returns>
        public static IEnumerable<R> SelectExceptDictionaryKeys<K, V1, V2, R>(this IDictionary<K, V1> input, IDictionary<K, V2> toCompare, Func<K, V1, R> toSelect)
        {
            return from d in input
                   where !toCompare.ContainsKey(d.Key)
                   select toSelect(d.Key, d.Value);
        }

        /// <summary>
        /// Selects from dictonary, except where values from list match the keys
        /// </summary>
        /// <typeparam name="K">Key type, and list type</typeparam>
        /// <typeparam name="V">Dictionary Value Type</typeparam>
        /// <typeparam name="R">Result Type</typeparam>
        /// <param name="input">dictionary</param>
        /// <param name="filter">filter</param>
        /// <param name="func">result</param>
        /// <returns></returns>
        public static IEnumerable<R> SelectExceptList<K, V, R>(this IDictionary<K, V> input, IEnumerable<K> filter, Func<K, V, R> func)
        {
            var dict = filter.ToDictionary(o => o);
            return input.SelectExceptDictionaryKeys(dict, func);
        }


        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings.ToDictionary(group => group.Key, group => group.ToList());
        }
    }




}
