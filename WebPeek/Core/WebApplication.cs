namespace WebPeek.Core;

/// <summary>
/// Representing a class that holds information about a web application.
/// </summary>
public class WebApplication
{
    /// <summary>
    /// Creates a new instance of the <see cref="WebApplication"/> class with default values.
    /// </summary>
    public WebApplication()
    {
        this.Name = string.Empty;
        this.Link = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="WebApplication"/> class with specified name and link.
    /// </summary>
    /// <param name="sName">Display name of the application.</param>
    /// <param name="sLink">Link to the associated web page.</param>
    public WebApplication(string sName, string sLink)
    {
        this.Name = sName;
        this.Link = sLink;
    }

    /// <summary>
    /// Representing the name of the web application.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Representing the link to the web application.
    /// </summary>
    public string Link { get; }
}
