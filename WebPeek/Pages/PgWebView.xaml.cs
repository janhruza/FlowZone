using System;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Wpf;

namespace WebPeek.Pages;

/// <summary>
/// Representing the WebView2 browser page class.
/// </summary>
public partial class PgWebView : Page
{
    const string DEFAULT_URL = "https://www.bing.com/";
    /// <summary>
    /// Creates a new instance of the <see cref="PgWebView"/> class.
    /// </summary>
    /// <param name="link">The URL to navigate to.</param>
    public PgWebView(string link = DEFAULT_URL)
    {
        InitializeComponent();

        if (Uri.TryCreate(link, UriKind.Absolute, out var uri) == true)
        {
            WebView = new WebView2
            {
                Source = uri
            };
        }

        else
        {
            WebView = new WebView2
            {
                Source = new Uri(DEFAULT_URL)
            };
        }

        this.Content = WebView;
    }

    /// <summary>
    /// Representing the <see cref="WebView2"/> control used for browsing.
    /// </summary>
    public WebView2 WebView { get; }
}
