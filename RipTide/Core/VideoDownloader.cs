using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FZCore;

namespace RipTide.Core;

/// <summary>
/// Representing the video downloader class.
/// </summary>
public class VideoDownloader
{
    /// <summary>
    /// Creates a new instance of the <see cref="VideoDownloader"/> class.
    /// </summary>
    public VideoDownloader()
    {
        Address = string.Empty;
        Location = string.Empty;
        Format = VideoDownloader.OutputFormat;
        Cookies = CookiesBrowser.None;
        AdditionalParameters = [];
    }

    /// <summary>
    /// Representing the video URL address.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Representing the destination folder for the downloaded video.
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Representing the output format.
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// Representing a list of additional, user-specified parameters.
    /// </summary>
    public List<string> AdditionalParameters { get; set; }

    /// <summary>
    /// Representing a browser to use it's cookies from to download videos.
    /// </summary>
    public CookiesBrowser Cookies {  get; set; }

    /// <summary>
    /// Attempts to start the video download.
    /// <paramref name="process">Output process that is responsible for downloading the video.</paramref>
    /// </summary>
    public bool Download(out Process? process)
    {
        if (DownloaderExists == false)
        {
            process = null;
            return false;
        }

        try
        {
            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = CommandPrompt,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Normal,
                    ArgumentList =
                    {
                        // YT-DLP command
                        "/C",
                        YT_DLP,

                        // folder path
                        "-P",
                        Location,

                        // output format
                        "-o",
                        Format,

                        // additional parameters
                        // use cookies (if any)
                        CookiesByBrowser[Cookies],

                        // user specified params
                        string.Join(' ', AdditionalParameters),

                        // show progress in the console window title
                        "--console-title",

                        // video URL
                        Address
                    }
                }
            };

            string sList = string.Join(' ', process.StartInfo.ArgumentList);

            // start the download process
            return process.Start();
        }

        catch (Exception ex)
        {
            process = null;
            Log.Error(ex, nameof(VideoDownloader.Download));
            return false;
        }
    }

    /// <summary>
    /// Attempts to start the asynchronous video download.
    /// </summary>
    /// <returns></returns>
    public Task<bool> DownloadAsync()
    {
        return Task.Run(() =>
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = CommandPrompt,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal,
                        ArgumentList =
                    {
                        // YT-DLP command
                        "/C",
                        YT_DLP,

                        // folder path
                        "-P",
                        Location,

                        // output format
                        "-o",
                        Format,

                        // additional parameters
                        // use cookies (if any)
                        CookiesByBrowser[Cookies],

                        // user specified params
                        string.Join(' ', AdditionalParameters),

                        // show progress in the console window title
                        "--console-title",

                        // video URL
                        Address
                    }
                    }
                };

                string sList = string.Join(' ', process.StartInfo.ArgumentList);

                // start the download process
                if (process.Start() == false)
                {
                    Log.Error("Failed to start the video download process.", nameof(VideoDownloader.DownloadAsync));
                    return false;
                }

                process.WaitForExit();
                return process.ExitCode == 0;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(VideoDownloader.Download));
                return false;
            }
        });
    }

    #region Static code

    /// <summary>
    /// Representing the source where can the YT-DLP be downloaded from (not the executable but the source page).
    /// </summary>
    public const string Link = "https://github.com/yt-dlp/yt-dlp";

    /// <summary>
    /// Representing the latest release page for the YT-DLP project.
    /// </summary>
    public const string LinkRelease = "https://github.com/yt-dlp/yt-dlp/releases/latest";

    /// <summary>
    /// Representing the source with all the supported web pages and domains.
    /// </summary>
    public const string SupportedSites = "https://github.com/yt-dlp/yt-dlp/blob/master/supportedsites.md";

    /// <summary>
    /// Representic the YT-DLP help.
    /// </summary>
    public const string Help = "https://github.com/yt-dlp/yt-dlp/blob/master/README.md#usage-and-options";

    /// <summary>
    /// Representing the main command line interpreter for this app.
    /// </summary>
    public const string CommandPrompt = "cmd.exe";

    /// <summary>
    /// Path to the YT-DLP video downloader executable.
    /// </summary>
    public const string YT_DLP = "yt-dlp.exe";

    /// <summary>
    /// Gets the version of the found downloader.
    /// </summary>
    /// <returns></returns>
    public static Version GetVersion()
    {
        if (DownloaderExists == false)
        {
            // downloader was not found
            return new Version(0, 0, 0);
        }

        FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(YT_DLP);
        return new Version(fileVersion.FileMajorPart, fileVersion.FileMinorPart, fileVersion.FileBuildPart);
    }

    private static bool _downloaderExists()
    {
        if (File.Exists(YT_DLP) == false)
        {
            // downloader was not found
            Log.Error("Downloader was not found.", nameof(DownloaderExists));
            return false;
        }

        // downloader found
        return true;
    }

    /// <summary>
    /// Determines whether the video downloader was found and is usable to download videos.
    /// </summary>
    public static bool DownloaderExists => _downloaderExists();

    /// <summary>
    /// Representing the default format for the downloaded video.
    /// </summary>
    public const string OutputFormat = "%(title)s.%(ext)s";

    /// <summary>
    /// Representing a dictionary of all supported cookies browsers and their command line option switches.
    /// </summary>
    public static Dictionary<CookiesBrowser, string> CookiesByBrowser => new Dictionary<CookiesBrowser, string>
    {
        { CookiesBrowser.None, "--no-cookies-from-browser" },
        { CookiesBrowser.Brave, "--cookies-from-browser brave" },
        { CookiesBrowser.Chrome, "--cookies-from-browser chrome" },
        { CookiesBrowser.Chromium, "--cookies-from-browser chromium" },
        { CookiesBrowser.Edge, "--cookies-from-browser edge" },
        { CookiesBrowser.Firefox, "--cookies-from-browser firefox" },
        { CookiesBrowser.Opera, "--cookies-from-browser opera" },
        { CookiesBrowser.Safari, "--cookies-from-browser safari" },
        { CookiesBrowser.Vivaldi, "--cookies-from-browser vivaldi" },
        { CookiesBrowser.Whale, "--cookies-from-browser whale" }
    };

    #endregion
}
