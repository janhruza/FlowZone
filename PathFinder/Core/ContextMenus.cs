using FZCore;

using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace PathFinder.Core;

/// <summary>
/// Representing a class with varios context menu definitions.
/// </summary>
public static class ContextMenus
{
    private static void ExecuteAction(string path, string verb)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo(path)
            {
                Verb = verb,
                UseShellExecute = true
            };
            _ = Process.Start(psi);
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(ExecuteAction));
        }
    }

    /// <summary>
    /// Creates a basic context menu for all folders.
    /// </summary>
    /// <param name="folderPath">Path to the folder.</param>
    /// <param name="menu"></param>
    /// <returns>Operation result.</returns>
    /// <remarks>Also returns false if the number of items is 0.</remarks>
    public static bool CmFolderMenu(string folderPath, out ContextMenu menu)
    {
        menu = new ContextMenu();

        var info = new ProcessStartInfo(folderPath);
        foreach (string verb in info.Verbs)
        {
            var item = new MenuItem
            {
                Header = verb.Normalize()
            };

            item.Click += (s, e) =>
            {
                ExecuteAction(folderPath, verb);
            };

            _ = menu.Items.Add(item);
        }

        return menu.Items.Count > 0;
    }

    /// <summary>
    /// Creates a basic context menu for all folders.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="menu"></param>
    /// <returns>Operation result.</returns>
    /// <remarks>Also returns false if the number of items is 0.</remarks>
    public static bool CmFileMenu(string filePath, out ContextMenu menu)
    {
        menu = new ContextMenu();

        var info = new ProcessStartInfo(filePath);
        foreach (string verb in info.Verbs)
        {
            var item = new MenuItem
            {
                Header = verb.Normalize()
            };

            item.Click += (s, e) =>
            {
                ExecuteAction(filePath, verb);
            };

            _ = menu.Items.Add(item);
        }

        return menu.Items.Count > 0;
    }
}
