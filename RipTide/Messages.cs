namespace RipTide;

/// <summary>
/// Representing all in-app messages.
/// </summary>
public static class Messages
{
    /// <summary>
    /// Unable to verify input data. Please make sure all the required fields are properly filled.
    /// </summary>
    public const string UnableToVerifyFields = "Unable to verify input data. Please make sure all the required fields are properly filled.";

    /// <summary>
    /// Unable to start the download process. Please make sure you have the downloader executable present in the current working directory.
    /// </summary>
    public const string UnableToStartDownload = "Unable to start the download process. Please make sure you have the downloader executable present in the current working directory.";

    /// <summary>
    /// Select output folder location
    /// </summary>
    public const string SelecOutputFolder = "Select output folder location";

    /// <summary>
    /// Do you want to delete the custom downloader executable path? After this operation, RipTide will assume the 'yt-dlp.exe' file is placed in the startup directory.
    /// </summary>

    public const string ResetCustomDownloader = $"Do you want to delete the custom downloader executable path? After this operation, RipTide will assume the 'yt-dlp.exe' file is placed in the startup directory.";
}
