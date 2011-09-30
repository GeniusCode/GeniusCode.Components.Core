using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeniusCode.Components.Extensions;

namespace GeniusCode.Components.Support.Random
{
    public class RandomCollection<T> : IEnumerable<WeightedItem<T>>, IEnumerable<T>
    {

        #region Protected Members
        protected virtual List<WeightedItem<T>> OnPickUniqueRandomValues(int desiredNumber, bool performUniqueTest)
        {
            List<WeightedItem<T>> output;

            TryPickUniqueRandomValues(desiredNumber, performUniqueTest, out output);
            return output;
        }
        #endregion


        #region Assets
        protected internal Dictionary<int, WeightedItem<T>> Items;

        private int _totalWeight;

        #endregion

        #region Public Members
        public bool TryPickUniqueRandomValues(int desiredNumber, bool performUniqueTest, out List<WeightedItem<T>> output)
        {
            // if desired number is less than 1, it doesn't make sense...
            if (desiredNumber < 1) throw new Exception("Count must be greater than 0");


            if (desiredNumber >= TotalNumberOfItems)
            {
                output = AsWeightedItems().ToList();
                var gotEnough = desiredNumber == this.TotalNumberOfItems;
                return gotEnough;
            }

            // otherwise perform calculations
            output = new List<WeightedItem<T>>();

            var myList = this;
            for (var i = 0; i < desiredNumber; i++)
            {
                var newItem = GetNewItem(performUniqueTest, ref myList);
                output.Add(newItem);
            }

            if (performUniqueTest)
            {
                if (output.CustomDistinctOn(o => o.WeightKey).Count() != output.Count)
                    throw new Exception("Values returned were not random, duplicates found");
            }


            return true;
        }


        public WeightedItem<T> PickClosestItem(int value)
        {
            return Items.GetClosestMatch(value, _totalWeight);
        }
        public List<WeightedItem<T>> PickUniqueRandomValues(int desiredNumber, bool performUniqueTest)
        {
            return OnPickUniqueRandomValues(desiredNumber, performUniqueTest);
        }


        public List<WeightedItem<T>> PickUniqueRandomValues(int count)
        {
            return PickUniqueRandomValues(count, false);
        }

        public WeightedItem<T> PickRandomItem()
        {
            var randomNumber = RandomHelper.GetRandomNumber(_totalWeight);
            return PickClosestItem(randomNumber);
        }

        public void SetWeightedItems(IEnumerable<WeightedItem<T>> items)
        {
            Items = items.ToArray().ToWeightedDictionary(out _totalWeight);
        }

        public IEnumerable<WeightedItem<T>> AsWeightedItems()
        {
            return this;
        }

        public IEnumerable<T> AsItems()
        {
            return this;
        }

        public int TotalNumberOfItems
        {
            get
            {
                return Items.Count;
            }
        }

        public int TotalWeight
        {
            get
            {
                return _totalWeight;
            }
        }
        public void CheckIntegrity()
        {
            Items.ToList().ForEach(i =>
            {
                if (i.Key != i.Value.WeightKey) throw new Exception(string.Format("Integrity error on random collection.  DictonaryKey was <{0}>, WeightKey was <{1}>",i.Key, i.Value.WeightKey));
            });
        }
        #endregion



        #region Constuctors

        public RandomCollection(IEnumerable<WeightedItem<T>> items)
        {
            SetWeightedItems(items);
        }
        private RandomCollection(Dictionary<int, WeightedItem<T>> itemDictionary)
        {
            Items = itemDictionary;
            _totalWeight = itemDictionary.Values.Sum(a => a.Weight);
        }
        #endregion

        #region Helpers

        private RandomCollection<T> GetNewRandomCollectionExcludingWeightedItem(WeightedItem<T> item)
        {

            var newItems = new Dictionary<int, WeightedItem<T>>(Items);
            newItems.Remove(item.WeightKey);

            return new RandomCollection<T>(newItems);
        }

        private static WeightedItem<T> GetNewItem(bool performUniqueTest, ref RandomCollection<T> myList)
        {
            var newItem = myList.PickRandomItem();
            myList = myList.GetNewRandomCollectionExcludingWeightedItem(newItem);

            if (performUniqueTest)
            {
                myList.CheckIntegrity();
                if (myList.AsWeightedItems().Any(a => a.WeightKey == newItem.WeightKey))
                    throw new Exception("Uhoh!");
            }
            return newItem;
        }






        #endregion

        #region implementations
        public IEnumerator<WeightedItem<T>> GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Items.Values.Select(o => o.Item).GetEnumerator();
        }
        #endregion


    }
}
