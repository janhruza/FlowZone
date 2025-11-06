namespace HashHive;

/// <summary>
/// Representing the list of supported hashing algorithms.
/// </summary>
public enum HashAlgorithm : byte
{
    /// <summary>
    /// Representing the SHA-1 hashing algorithm.
    /// </summary>
    SHA1 = 0,

    /// <summary>
    /// Representing the SHA-256 hashing algorithm.
    /// </summary>
    SHA256 = 1,

    /// <summary>
    /// Representing the SHA-384 hashing algorithm.
    /// </summary>
    SHA384 = 2,

    /// <summary>
    /// Representing the SHA-512 hashing algorithm.
    /// </summary>
    SHA512 = 3,

    /// <summary>
    /// Representing the MD5 hashing algorithm.
    /// </summary>
    MD5 = 4
}
