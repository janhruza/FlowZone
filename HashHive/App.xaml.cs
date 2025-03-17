using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using FZCore;

namespace HashHive;

/// <summary>
/// Representing the main application class.
/// This class contains the Main method (<see cref="Application_Startup(object, StartupEventArgs)"/>).
/// This class derives directly from the <see cref="Application"/> class.
/// </summary>
public partial class App : Application
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

        using (var sha = SHA256.Create())
        {
            byte[] data = sha.ComputeHash(buf);
            return Convert.ToHexString(data);
        }
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

        using (var sha = SHA512.Create())
        {
            byte[] data = sha.ComputeHash(buf);
            return Convert.ToHexString(data);
        }
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

        using (var md5 = MD5.Create())
        {
            byte[] data = md5.ComputeHash(buf);
            return Convert.ToHexString(data);
        }
    }

    #endregion
}

