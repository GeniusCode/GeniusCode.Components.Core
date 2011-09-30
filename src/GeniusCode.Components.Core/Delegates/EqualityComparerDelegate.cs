
namespace GeniusCode.Components.Delegates
{
    /// <summary>
    /// Determines whether the specified objects are equal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a">The first object of type T to compare.</param>
    /// <param name="b">The second object of type T to compare.</param>
    /// <returns>true if the specified objects are equal; otherwise, false.</returns>
    public delegate bool EqualityComparerDelegate<in T>(T a, T b);
}
