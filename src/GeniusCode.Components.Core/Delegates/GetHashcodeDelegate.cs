namespace GeniusCode.Components.Delegates
{
    /// <summary>
    /// Returns a hashcode for the specified object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a">The object on which to calculate a hashcode</param>
    /// <returns></returns>
    public delegate int GetHashcodeDelegate<in T>(T a);
}