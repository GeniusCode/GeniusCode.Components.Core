namespace GeniusCode.Components.Delegates
{
    /// <summary>
    /// Compares two objects and returns a value indicating whether one is less than,
    /// equal to, or greater than the other.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a">The first object to compare.</param>
    /// <param name="b">The second object to compare</param>
    /// <returns>
    /// Value Condition Less than zero x is less than y.  Zero x equals y.  Greater
    /// than zero x is greater than y.</returns>
    public delegate int ComparerDelegate<in T>(T a, T b);
}