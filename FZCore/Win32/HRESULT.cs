namespace FZCore.Win32;

/// <summary>
/// Represents a Windows HRESULT value, which is used to indicate success or failure of an operation in COM and Windows
/// APIs.
/// </summary>
/// <remarks>An HRESULT is a 32-bit or 64-bit value commonly used in Windows programming to encode error and
/// status codes. A non-negative value typically indicates success, while a negative value indicates failure. This
/// struct provides implicit conversions to and from <see langword="long"/> for interoperability with native HRESULT
/// values.</remarks>
/// <remarks>
/// Initializes a new instance of the HRESULT structure with the specified value.
/// </remarks>
/// <param name="value">The 32-bit integer value representing the HRESULT code to initialize the structure with.</param>
public struct HRESULT(long value)
{
    private long _value = value;

    /// <summary>
    /// Defines an implicit conversion from an HRESULT value to its underlying 64-bit integer representation.
    /// </summary>
    /// <remarks>This operator enables seamless use of HRESULT values in contexts where a 64-bit integer is
    /// required, such as interop scenarios or low-level error handling. The conversion returns the raw numeric value of
    /// the HRESULT.</remarks>
    /// <param name="hresult">The HRESULT value to convert.</param>
    public static implicit operator long(HRESULT hresult) => hresult._value;

    /// <summary>
    /// Defines an implicit conversion from a 64-bit signed integer to an HRESULT value.
    /// </summary>
    /// <remarks>This operator enables seamless assignment of a long value to an HRESULT without explicit
    /// casting. Use this conversion when working with APIs or code that require HRESULT values derived from integer
    /// error codes.</remarks>
    /// <param name="value">The 64-bit signed integer to convert to an HRESULT.</param>
    public static implicit operator HRESULT(long value) => new HRESULT(value);

    /// <summary>
    /// Determines whether the specified HRESULT value indicates success.
    /// </summary>
    /// <remarks>An HRESULT is considered successful if its value is greater than or equal to zero. This
    /// method is commonly used to check the result of COM or Win32 operations.</remarks>
    /// <param name="hr">The HRESULT value to evaluate.</param>
    /// <returns><see langword="true"/> if the HRESULT value represents a successful operation; otherwise, <see
    /// langword="false"/>.</returns>
    public static bool SUCCEEDED(HRESULT hr) => hr._value >= 0;

    /// <summary>
    /// Determines whether the specified HRESULT value indicates a failure result.
    /// </summary>
    /// <remarks>An HRESULT is considered a failure if its most significant bit is set (i.e., the value is
    /// negative). This method is commonly used to check the result of COM or Win32 operations.</remarks>
    /// <param name="hr">The HRESULT value to evaluate.</param>
    /// <returns>true if the HRESULT value represents a failure; otherwise, false.</returns>
    public static bool FAILED(HRESULT hr) => hr._value < 0;
}
