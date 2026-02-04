namespace FZCore.Extensions;

/// <summary>
/// Representing various <see cref="string"/> extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Reduces the length of the string using the <see cref="string.Substring(int, int)"/> method, where starting index is 0 and length is <paramref name="length"/>.
    /// </summary>
    /// <param name="value">Target string to be modified.</param>
    /// <param name="length">
    /// Maximum length of the new string.
    /// If <paramref name="length"/> is less or equal to than 0, it's set to the value of 25.
    /// </param>
    /// <returns>
    /// A new string with the desired length.
    /// If the <paramref name="value"/> was longer, three dots (...) are added at the end of the string.
    /// If the <paramref name="length"/> is greater than length of the <paramref name="value"/>, the original string (<paramref name="value"/>) is returned.
    /// </returns>
    public static string Reduce(this string value, int length = 25)
    {
        if (length <= 0)
        {
            length = 25;
        }

        if (value.Length <= length)
        {
            return value;
        }

        return value[..length] + "...";
    }
}
