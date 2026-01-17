using FZCore;
using FZCore.Extra;

using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace HashHive;

/// <summary>
/// Representing the main application class.
/// This class contains the Main method (<see cref="Application_Startup(object, StartupEventArgs)"/>).
/// This class derives directly from the <see cref="Application"/> class.
/// </summary>
public partial class App : BaseApplication
{
    /// <summary>
    /// Representing the base application title.
    /// </summary>
    public const string Title = "HashHive";

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // log app startup
        Log.AppStarted();

        // set app theme
        FZCore.Core.SetApplicationTheme(FZThemeMode.System);

        // create main window
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // log app exit
        Log.AppExited();
    }

    /// <summary>
    /// Centers the given <paramref name="window"/>.
    /// </summary>
    /// <param name="window">Target window to be centered.</param>
    public static void CenterWindow(Window window)
    {
        double top = (SystemParameters.PrimaryScreenHeight - window.Height) / 2;
        double left = (SystemParameters.PrimaryScreenWidth - window.Width) / 2;
        window.Top = top;
        window.Left = left;
        return;
    }

    #region Hashing functions

    /// <summary>
    /// Computes the SHA-256 hash.
    /// </summary>
    /// <param name="input">Input data (as text).</param>
    /// <returns>SHA-256 hash of the <paramref name="input"/> as <see cref="System.String"/>.</returns>
    public static string SHA256Hash(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "INVALID_INPUT";
        }

        byte[] buf = Encoding.UTF8.GetBytes(input);
        byte[] data = SHA256.HashData(buf);
        return Convert.ToHexString(data);
    }

    /// <summary>
    /// Computes the SHA-512 hash.
    /// </summary>
    /// <param name="input">Input data (as text).</param>
    /// <returns>SHA-512 hash of the <paramref name="input"/> as <see cref="System.String"/>.</returns>
    public static string SHA512Hash(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "INVALID_INPUT";
        }

        byte[] buf = Encoding.UTF8.GetBytes(input);
        byte[] data = SHA512.HashData(buf);
        return Convert.ToHexString(data);
    }

    /// <summary>
    /// Computes the MD5 hash.
    /// </summary>
    /// <param name="input">Input data (as text).</param>
    /// <returns>MD5 hash of the <paramref name="input"/> as <see cref="System.String"/>.</returns>
    public static string MD5Hash(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "INVALID_INPUT";
        }

        byte[] buf = Encoding.UTF8.GetBytes(input);
        byte[] data = MD5.HashData(buf);
        return Convert.ToHexString(data);
    }

    /// <summary>
    /// Computes the SHA-1 hash.
    /// </summary>
    /// <param name="input">Input data (as text).</param>
    /// <returns>SHA-1 hash of the <paramref name="input"/> as <see cref="System.String"/>.</returns>
    public static string SHA1Hash(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "INVALID_INPUT";
        }

        byte[] buf = Encoding.UTF8.GetBytes(input);
        byte[] data = SHA1.HashData(buf);
        return Convert.ToHexString(data);
    }

    /// <summary>
    /// Computes the SHA-384 hash.
    /// </summary>
    /// <param name="input">Input data (as text).</param>
    /// <returns>SHA-384 hash of the <paramref name="input"/> as <see cref="System.String"/>.</returns>
    public static string SHA384Hash(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "INVALID_INPUT";
        }

        byte[] buf = Encoding.UTF8.GetBytes(input);
        byte[] data = SHA384.HashData(buf);
        return Convert.ToHexString(data);
    }

    /// <summary>
    /// Computes the SHA-1 hash of the given <paramref name="input"/> data.
    /// </summary>
    /// <param name="input">Input data as <see cref="System.Byte"/> array.</param>
    /// <returns>Computed hash.</returns>
    public static string SHA1Hash(byte[] input)
    {
        byte[] hash = SHA1.HashData(input);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// Computes the SHA-256 hash of the given <paramref name="input"/> data.
    /// </summary>
    /// <param name="input">Input data as <see cref="System.Byte"/> array.</param>
    /// <returns>Computed hash.</returns>
    public static string SHA256Hash(byte[] input)
    {
        byte[] hash = SHA256.HashData(input);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// Computes the SHA-384 hash of the given <paramref name="input"/> data.
    /// </summary>
    /// <param name="input">Input data as <see cref="System.Byte"/> array.</param>
    /// <returns>Computed hash.</returns>
    public static string SHA384Hash(byte[] input)
    {
        byte[] hash = SHA384.HashData(input);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// Computes the SHA-512 hash of the given <paramref name="input"/> data.
    /// </summary>
    /// <param name="input">Input data as <see cref="System.Byte"/> array.</param>
    /// <returns>Computed hash.</returns>
    public static string SHA512Hash(byte[] input)
    {
        byte[] hash = SHA512.HashData(input);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// Computes the MD5 hash of the given <paramref name="input"/> data.
    /// </summary>
    /// <param name="input">Input data as <see cref="System.Byte"/> array.</param>
    /// <returns>Computed hash.</returns>
    public static string MD5Hash(byte[] input)
    {
        byte[] hash = MD5.HashData(input);
        return Convert.ToHexString(hash);
    }

    #endregion
}

