using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PassFort.Core;

/// <summary>
/// Representing the helper class for various AES encryption tasks.
/// </summary>
public static class AesHelper
{
    /// <summary>
    /// Encrypts the given <paramref name="password"/> with the provided <paramref name="key"/>.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Invalid key size.</exception>
    public static byte[] EncryptPassword(string password, byte[] key)
    {
        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            throw new ArgumentException("Key must be 128, 192, or 256 bits long.");

        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV(); // Generate a new IV for each encryption

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(password); // Write the password to the stream
        }

        var ivAndCiphertext = aes.IV;
        ivAndCiphertext = ivAndCiphertext.Concat(ms.ToArray()).ToArray(); // Concatenate IV and ciphertext

        return ivAndCiphertext; // Return both IV and encrypted data together
    }

    /// <summary>
    /// Decrypts the given <paramref name="encryptedData"/> using the provided <paramref name="key"/>.
    /// </summary>
    /// <param name="encryptedData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Invalid key size.</exception>
    public static string DecryptPassword(byte[] encryptedData, byte[] key)
    {
        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            throw new ArgumentException("Key must be 128, 192, or 256 bits long.");

        using var aes = Aes.Create();
        aes.Key = key;

        byte[] iv = new byte[aes.BlockSize / 8]; // IV size
        byte[] ciphertext = new byte[encryptedData.Length - iv.Length];

        Array.Copy(encryptedData, 0, iv, 0, iv.Length); // Extract the IV from beginning of encrypted data
        Array.Copy(encryptedData, iv.Length, ciphertext, 0, ciphertext.Length);

        aes.IV = iv; // Set the extracted IV

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(ciphertext);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd(); // Read the decrypted password
    }
}