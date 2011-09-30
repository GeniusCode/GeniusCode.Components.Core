namespace GeniusCode.Components.Support.Random
{
    public class RandomHelper
    {
        private static System.Random _random;
        public static int GetRandomNumber(int minValue, int maxValue)
        {
            if (_random == null)
            {
                _random = new System.Random();
            }
            return (_random.Next(minValue, maxValue) + 1);
        }

        public static int GetRandomNumber(int maxValue)
        {
            return GetRandomNumber(0, maxValue);
        }

    }

}
