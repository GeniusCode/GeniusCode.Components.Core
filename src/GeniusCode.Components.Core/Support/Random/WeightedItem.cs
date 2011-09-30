using System;

namespace GeniusCode.Components.Support.Random
{
    public class WeightedItem<T>
    {
        public WeightedItem(T item, int weight)
        {
            if (weight < 0) throw new Exception("Weight cannot be negative");
            if (weight == 0) weight = 1;

            Item = item;
            Weight = weight;
        }

        public T Item { get; private set; }
        public int Weight { get; private set; }
        public int WeightKey { get; internal set; }

    }
}