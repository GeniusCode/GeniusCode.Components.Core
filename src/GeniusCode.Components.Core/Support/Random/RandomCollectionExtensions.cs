using System;
using System.Collections.Generic;
using System.Linq;

namespace GeniusCode.Components.Support.Random
{
    public static class RandomCollectionExtensions
    {
        public static RandomCollection<T> ToRandomCollection<T>(this IEnumerable<T> input, Func<T, int> getWeight)
        {
            var weightedItems = from t in input
                                select new WeightedItem<T>(t, getWeight(t));

            return new RandomCollection<T>(weightedItems);
        }

        public static Dictionary<int, WeightedItem<T>> ToWeightedDictionary<T>(this IList<WeightedItem<T>> items, out int weightTotal)
        {
            int runningTotal = 0;
            var newDictonary = new Dictionary<int, WeightedItem<T>>(items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                // calculate new key by adding running total
                WeightedItem<T> item = items[i];

                int key = runningTotal + item.Weight;
                item.WeightKey = key;
                newDictonary.Add(key, item);

                runningTotal = key;
            }

            weightTotal = runningTotal;
            return newDictonary;
        }

        public static WeightedItem<T> GetClosestMatch<T>(this Dictionary<int, WeightedItem<T>> items, int value, int totalWeight)
        {
            if (value < 0) throw new Exception("Value may not be negative");

            // if value is too high, throw exception
            if (value > totalWeight) throw new Exception("Not enough weight in collection for value");

            WeightedItem<T> item;
            if (items.TryGetValue(value, out item))
                return item;

            var q = from t in items
                    where value <= t.Key
                    orderby t.Key ascending
                    select t;

            return q.First().Value;
        }

        public static WeightedItem<T> PickRandomItem<T>(this Dictionary<int, WeightedItem<T>> items, int totalWeight)
        {
            var randomNumber = RandomHelper.GetRandomNumber(totalWeight);
            return items.GetClosestMatch(randomNumber, totalWeight);
        }



    }
}