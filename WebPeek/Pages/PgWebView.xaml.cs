using System;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Wpf;

namespace WebPeek.Pages;

/// <summary>
/// Representing the WebView2 browser page class.
/// </summary>
public partial class BrowserPage : Page
{
    const string DEFAULT_URL = "https://www.bing.com/";
    /// <summary>
    /// Creates a new instance of the <see cref="BrowserPage"/> class.
    /// </summary>
    /// <param name="link">The URL to navigate to.</param>
    public BrowserPage(string link = DEFAULT_URL)
    {
        InitializeComponent();
        WebView = new WebView2
        {
            Source = new Uri(link)
        };

        this.Content = WebView;
    }

    /// <summary>
    /// Representing the <see cref="WebView2"/> control used for browsing.
    /// </summary>
    public WebView2 WebView { get; }
}
