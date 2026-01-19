using Microsoft.Win32.SafeHandles;

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FZCore;

/// <summary>
/// Provides low-level Win32 API operations to allocate and manage a developer console within a GUI application.
/// </summary>
public static class DevConsole
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FreeConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    private const int STD_INPUT_HANDLE = -10;
    private const int STD_OUTPUT_HANDLE = -11;
    private const int STD_ERROR_HANDLE = -12;

    static bool _opened = false;

    /// <summary>
    /// Determines whether the developer console is active or not.
    /// </summary>
    public static bool IsActive => _opened;

    /// <summary>
    /// Allocates a new console for the current process and redirects Standard Input, Output, and Error streams.
    /// </summary>
    public static void OpenConsole()
    {
        if (AllocConsole())
        {
            SetOutStream(STD_OUTPUT_HANDLE, isError: false);
            SetOutStream(STD_ERROR_HANDLE, isError: true);
            SetInStream();
            _opened = true;
        }
    }

    /// <summary>
    /// Detaches the console from the process and resets standard streams to null to prevent IO exceptions.
    /// </summary>
    public static void CloseConsole()
    {
        Console.SetOut(TextWriter.Null);
        Console.SetIn(TextReader.Null);
        FreeConsole();
        _opened = false;
    }

    /// <summary>
    /// Redirects a specific Win32 output handle to the .NET Console management system.
    /// </summary>
    /// <param name="handleType">The Win32 standard handle constant (Output or Error).</param>
    /// <param name="isError">If true, redirects to Console.Error; otherwise redirects to Console.Out.</param>
    private static void SetOutStream(int handleType, bool isError)
    {
        IntPtr handle = GetStdHandle(handleType);
        SafeFileHandle safeHandle = new SafeFileHandle(handle, ownsHandle: false);
        FileStream fs = new FileStream(safeHandle, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fs, Console.OutputEncoding) { AutoFlush = true };

        if (isError)
            Console.SetError(writer);
        else
            Console.SetOut(writer);
    }

    /// <summary>
    /// Redirects the Win32 standard input handle to the .NET Console.In stream.
    /// </summary>
    private static void SetInStream()
    {
        IntPtr handle = GetStdHandle(STD_INPUT_HANDLE);
        SafeFileHandle safeHandle = new SafeFileHandle(handle, ownsHandle: false);
        FileStream fs = new FileStream(safeHandle, FileAccess.Read);
        StreamReader reader = new StreamReader(fs, Console.InputEncoding);
        Console.SetIn(reader);
    }
}