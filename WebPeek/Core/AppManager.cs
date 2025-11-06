using System.Collections.Generic;
using System.IO;

namespace WebPeek.Core;

/// <summary>
/// Representing the application manager class.
/// </summary>
public static class AppManager
{
    private static Dictionary<int, WebApplication> dWebApps;

    /// <summary>
    /// Gets the registered web applications.
    /// </summary>
    /// <returns>List of registered web applications.</returns>
    public static IEnumerable<WebApplication> GetApps()
    {
        // Returns the registered web applications
        return dWebApps.Values;
    }

    private static int GetHashCode(WebApplication webApp)
    {
        // Simple hash code generation based on the name and link of the application
        return (webApp.Name.GetHashCode() ^ webApp.Link.GetHashCode());
    }

    #region Import/export methods

    /*
     * Apps file structure:
     *  1. Number of applications (int)
     *  2. For each application:
     *  {
     *      1. Hash code (int)
     *      2. Name (string) (length-prefixed)
     *      3. Link (string) (length-prefixed)
     *  }
     */

    private const string sAppsFileName = "apps.dat";

    internal static bool ExportApps(string sFileName = sAppsFileName)
    {
        // exports the registered applications to a file

        using (FileStream fs = File.Create(sFileName))
        {
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                // write the number of applications
                bw.Write(dWebApps.Count);

                // write each application
                foreach (var kvp in dWebApps)
                {
                    WebApplication webApp = kvp.Value;
                    bw.Write(kvp.Key); // hash code
                    bw.Write(webApp.Name); // name
                    bw.Write(webApp.Link); // link
                }
            }
        }

        return true;
    }

    internal static bool ImportApps(string sFileName = sAppsFileName)
    {
        // imports applications from a file
        if (!File.Exists(sFileName))
        {
            // file does not exist
            return false;
        }

        using (FileStream fs = File.OpenRead(sFileName))
        {
            using (BinaryReader br = new BinaryReader(fs))
            {
                // read the number of applications
                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int hashCode = br.ReadInt32(); // read hash code
                    string name = br.ReadString(); // read name
                    string link = br.ReadString(); // read link

                    // create a new web application instance
                    WebApplication webApp = new WebApplication(name, link);

                    // register the application
                    RegisterApp(webApp);
                }
            }
        }

        return true;
    }

    #endregion

    static AppManager()
    {
        dWebApps = new Dictionary<int, WebApplication>();
    }

    /// <summary>
    /// Registers a new application with the given name and link.
    /// </summary>
    /// <param name="webApp">The web application to register.</param>
    /// <returns><see langword="true"/> if the application is registered, otherwise <see langword="false"/>.</returns>
    public static bool RegisterApp(WebApplication webApp)
    {
        if (string.IsNullOrEmpty(webApp.Name) || string.IsNullOrEmpty(webApp.Link))
        {
            return false;
        }

        int hash = GetHashCode(webApp);
        if (dWebApps.ContainsKey(hash) == true)
        {
            // app already registered
            return false;
        }

        // register the app
        dWebApps.Add(hash, webApp);

        return true;
    }

    /// <summary>
    /// Unregisters an application.
    /// </summary>
    /// <param name="webApp">Application to be unregistered.</param>
    /// <returns><see langword="true"/> if the application is unregistered, otherwise <see langword="false"/>.</returns>
    public static bool UnregisterApp(WebApplication webApp)
    {
        int hash = GetHashCode(webApp);
        if (dWebApps.ContainsKey(hash) == false)
        {
            // app not registered
            return false;
        }

        // unregister the app
        dWebApps.Remove(hash);
        return true;
    }
}
