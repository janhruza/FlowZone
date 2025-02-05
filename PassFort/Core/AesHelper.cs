using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PassFort.Core;

/// <summary>
/// Representing the helper class for various AES encryption tasks. This class was originally generated using the PHI-4 model.
/// </summary>
public static class AesHelper
{
    private const int IvSize = 16; // AES block size in bytes (128-bit IV)

    /// <summary>
    /// Encrypts a given password using AES encryption.
    /// </summary>
    /// <param name="password">The password to encrypt.</param>
    /// <param name="key">The encryption key (128, 192, or 256 bits).</param>
    /// <returns>A byte array containing the IV followed by the encrypted password.</returns>
    /// <exception cref="ArgumentNullException">Thrown when password is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when the key size is invalid.</exception>
    public static byte[] EncryptPassword(string password, byte[] key)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        using MemoryStream ms = new MemoryStream();
        ms.Write(aes.IV, 0, IvSize); // Store IV at the beginning
        using CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using StreamWriter sw = new StreamWriter(cs, Encoding.UTF8);
        sw.Write(password);

        return ms.ToArray();
    }

    /// <summary>
    /// Decrypts an AES-encrypted password.
    /// </summary>
    /// <param name="encryptedData">The encrypted data containing the IV and the encrypted password.</param>
    /// <param name="key">The decryption key (128, 192, or 256 bits).</param>
    /// <returns>The decrypted password as a string.</returns>
    /// <exception cref="ArgumentException">Thrown when encryptedData is invalid or key size is incorrect.</exception>
    public static string DecryptPassword(byte[] encryptedData, byte[] key)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;

        byte[] iv = new byte[IvSize];
        Array.Copy(encryptedData, 0, iv, 0, IvSize);
        aes.IV = iv;

        using MemoryStream ms = new MemoryStream(encryptedData, IvSize, encryptedData.Length - IvSize);
        using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs, Encoding.UTF8);

        return sr.ReadToEnd();
    }

    /// <summary>
    /// Validates whether the provided key size is supported for AES encryption.
    /// </summary>
    /// <param name="key">The encryption key.</param>
    /// <returns>True if the key size is valid; otherwise, false.</returns>
    private static bool IsValidKeySize(byte[] key) => key.Length is 16 or 24 or 32;
}
