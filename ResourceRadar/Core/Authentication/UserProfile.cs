using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FZCore;

namespace ResourceRadar.Core.Authentication;

/*
 * USER PROFILE FILESYSTEM STRUCTURE
 * [FOLDER]     USER_ID         User folder, name is user's ID
 *  - [FILE]    settings.json   User profile data and settings
 *  - [FILE]    items.bin       List of all user's inventory items in binary format.
 */

/// <summary>
/// Representing a user profile class.
/// </summary>
public class UserProfile
{
    private const string FILE_PROFILE = "settings.json";
    private const string FILE_ITEMS = "items.bin";

    /// <summary>
    /// Representing the user profiles folder.
    /// </summary>
    public static string ProfilesFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");

    /// <summary>
    /// Creates a new instance of the <see cref="UserProfile"/> class.
    /// </summary>
    public UserProfile()
    {
        Id = ulong.MinValue;
        Name = Environment.UserName;
        Description = string.Empty;
        Settings = new ProfileSettings();
        Items = [];
    }

    /// <summary>
    /// Representing the profile unique identification number.
    /// </summary>
    public ulong Id { get; init; }

    /// <summary>
    /// Representing the user profile name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Representing the description of this profile.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Representing the profile settings.
    /// </summary>
    public ProfileSettings Settings { get; set; }

    /// <summary>
    /// Representing all user's stored inventory items.
    /// </summary>
    [JsonIgnore]
    public List<InventoryItem> Items { get; set; }

    /// <summary>
    /// Logouts the currently loaded profile.
    /// </summary>
    /// <returns>Always returns true.</returns>
    public static bool Logout()
    {
        _current = null;
        Log.Success($"Active user was logged out.", nameof(Logout));
        return true;
    }

    /// <summary>
    /// Sets the currently loaded profile to <paramref name="profile"/>.
    /// </summary>
    /// <param name="profile">New active profile.</param>
    /// <returns>True, if the active profile has changed, otherwise false.</returns>
    public static bool Login(UserProfile? profile)
    {
        if (profile == null)
        {
            Log.Error("User profile is null.", nameof(Login));
            return false;
        }

        _current = profile;
        Log.Success($"User \'{profile.Name}\' was logged in.", nameof(Login));
        return true;
    }

    private static UserProfile? _current;

    /// <summary>
    /// Representing the user account in use (if any).
    /// </summary>
    public static UserProfile? Current => _current;

    /// <summary>
    /// Attempts to create a new user profile and it's structure on drive.
    /// </summary>
    /// <param name="userName">Name of the user profile.</param>
    /// <param name="description">Additional optional description of the user profile.</param>
    /// <returns>True, if profile was created, otherwise false.</returns>
    public static async Task<bool> CreateUserProfile(string? userName, string? description)
    {
        try
        {
            if (string.IsNullOrEmpty(userName) == true)
            {
                Log.Error($"Unable to create a new profile because the name is null.", nameof(CreateUserProfile));
                return false;
            }

            // define the user profile
            UserProfile profile = new UserProfile
            {
                Id = (ulong)DateTime.Now.ToBinary(),
                Name = userName,
                Description = description ?? string.Empty,
                Items = [],
                Settings = new ProfileSettings()
            };

            // write profile data
            string userFolder = Path.Combine(ProfilesFolder, profile.Id.ToString());
            _ = Directory.CreateDirectory(userFolder);

            string profileData = Path.Combine(userFolder, FILE_PROFILE);
            string userItems = Path.Combine(userFolder, FILE_ITEMS);

            // writes profile data into the settings file
            string jsonProfileData = JsonSerializer.Serialize<UserProfile>(profile);
            File.WriteAllText(profileData, jsonProfileData);

            // writes the transactions list

            return await WriteUserItems(profile, []);
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(CreateUserProfile));
            return false;
        }
    }

    /// <summary>
    /// Attempts to write user inventory data.
    /// </summary>
    /// <param name="profile">Target profile.</param>
    /// <param name="collection">List of all items to be written to the user profile.</param>
    /// <returns>True, if the operation is successful, otherwise false.</returns>
    public static async Task<bool> WriteUserItems(UserProfile? profile, InventoryItemsCollection? collection)
    {
        if (profile == null)
        {
            return false;
        }

        if (collection == null)
        {
            return false;
        }

        bool result = await Task.Run<bool>(() => {
            try
            {
                string path = Path.Combine(ProfilesFolder, profile.Id.ToString(), FILE_ITEMS);
                using (FileStream fs = File.Create(path))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        // write number of items
                        bw.Write(collection.Count);

                        // write all items one at a time
                        foreach (InventoryItem item in collection)
                        {
                            // basic info
                            bw.Write(item.Id);
                            bw.Write(item.Name);
                            bw.Write(item.Category);
                            bw.Write(item.Brand);
                            bw.Write(item.Model);

                            // storage info
                            bw.Write(item.Storage.Room);
                            bw.Write(item.Storage.StorageArea);
                            bw.Write(item.Storage.Condition);

                            // purchase details
                            bw.Write(item.PurchaseDate.ToBinary());
                            bw.Write(item.PurchasePrice);
                            bw.Write(item.PurchasedFrom);
                            bw.Write(item.HasWarranty);
                            bw.Write(item.WarrantyExpiry.HasValue == true ? item.WarrantyExpiry.Value.ToBinary() : DateTime.MinValue.ToBinary());

                            // metadata
                            bw.Write(item.IsInUse);
                            bw.Write(item.LastUsed.HasValue == true ? item.LastUsed.Value.ToBinary() : DateTime.MinValue.ToBinary());
                            bw.Write(item.ExpirationDate.HasValue == true ? item.ExpirationDate.Value.ToBinary() : DateTime.MinValue.ToBinary());

                            // maintanence history
                            bw.Write(item.MaintenanceHistory.Count);
                            foreach (string mHist in item.MaintenanceHistory)
                            {
                                bw.Write(mHist);
                            }

                            // attachments
                            bw.Write(item.Attachments.Count);
                            foreach (string attachment in item.Attachments)
                            {
                                bw.Write(attachment);
                            }

                            // notes
                            bw.Write(item.Notes);
                        }
                    }
                }

                return true;
            }

            catch  (Exception ex)
            {
                Log.Error(ex, nameof(WriteUserItems));
                return false;
            }
        });

        return result;
    }

    /// <summary>
    /// Attempts to save <paramref name="profile"/> settings.
    /// </summary>
    /// <param name="profile">Target user profile to save.</param>
    /// <returns>True, if the profile settings were saved, otherwise false.</returns>
    public static async Task<bool> SaveSettings(UserProfile? profile)
    {
        if (profile == null)
        {
            return false;
        }

        bool result = await Task.Run<bool>(() =>
        {
            try
            {
                string data = JsonSerializer.Serialize<UserProfile>(profile);
                string path = Path.Combine(ProfilesFolder, profile.Id.ToString(), FILE_PROFILE);
                File.WriteAllText(path, data, Encoding.UTF8);
                return true;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(SaveSettings));
                return false;
            }
        });

        return result;
    }

    /// <summary>
    /// Gets the number of profiles.
    /// </summary>
    /// <returns></returns>
    public static int GetProfilesCount()
    {
        try
        {
            if (Directory.Exists(ProfilesFolder) == false)
            {
                return 0;
            }

            return Directory.GetDirectories(ProfilesFolder).Length;
        }


        catch (Exception ex)
        {
            Log.Error(ex, nameof(GetProfilesCount));
            return 0;
        }
    }

    /// <summary>
    /// Sets the given <paramref name="profile"/> as the current profile unless the <paramref name="profile"/> is null.
    /// </summary>
    /// <param name="profile">Profile to be set as current.</param>
    /// <returns>True, if the <paramref name="profile"/> is set as active, otherwise false.</returns>
    public static bool SetCurrent(UserProfile? profile)
    {
        if (profile == null)
        {
            return false;
        }

        _current = profile;
        FZCore.Core.SetApplicationTheme(App.Current, profile.Settings.ThemeMode);
        return true;
    }

    /// <summary>
    /// Asynchronously gets a list of all created profiles.
    /// </summary>
    /// <returns>List of all created profiles.</returns>
    public static async Task<List<UserProfile>> GetProfilesAsync()
    {
        if (Directory.Exists(ProfilesFolder) == false)
        {
            return [];
        }

        List<UserProfile> profiles = await Task.Run<List<UserProfile>>(() =>
        {
            try
            {
                List<UserProfile> profiles = [];

                foreach (string dir in Directory.GetDirectories(ProfilesFolder))
                {
                    if (File.Exists(Path.Combine(dir, FILE_PROFILE)) == false || File.Exists(Path.Combine(dir, FILE_ITEMS)) == false)
                    {
                        Log.Warning($"Incomplete profile \'{dir}\'. Loading skipped.", nameof(GetProfilesAsync));
                        continue;
                    }

                    try
                    {
                        // load profile data
                        string data = File.ReadAllText(Path.Combine(dir, FILE_PROFILE), Encoding.UTF8);
                        UserProfile? profile = JsonSerializer.Deserialize<UserProfile>(data);

                        if (profile == null)
                        {
                            Log.Error("Unable to deserialize profile data.", nameof(GetProfilesAsync));
                            continue;
                        }

                        // load profile items
                        using (FileStream fs = File.OpenRead(Path.Combine(dir, FILE_ITEMS)))
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                int count = br.ReadInt32();
                                for (int x = 0; x < count; x++)
                                {
                                    InventoryItem item = new InventoryItem();

                                    // basic data
                                    item.Id = br.ReadInt64();
                                    item.Name = br.ReadString();
                                    item.Category = br.ReadString();
                                    item.Brand = br.ReadString();
                                    item.Model = br.ReadString();

                                    // storge data
                                    item.Storage.Room = br.ReadString();
                                    item.Storage.StorageArea = br.ReadString();
                                    item.Storage.Condition = br.ReadString();

                                    // purchase details
                                    item.PurchaseDate = DateTime.FromBinary(br.ReadInt64());
                                    item.PurchasePrice = br.ReadDecimal();
                                    item.PurchasedFrom = br.ReadString();
                                    item.HasWarranty = br.ReadBoolean();
                                    item.WarrantyExpiry = DateTime.FromBinary(br.ReadInt64());

                                    // metadata
                                    item.IsInUse = br.ReadBoolean();
                                    item.LastUsed = DateTime.FromBinary(br.ReadInt64());
                                    item.ExpirationDate = DateTime.FromBinary(br.ReadInt64());

                                    List<string> mHistory = [];
                                    int mCount = br.ReadInt32();
                                    for (int m = 0; m < mCount; m++)
                                    {
                                        mHistory.Add(br.ReadString());
                                    }

                                    // add history
                                    item.MaintenanceHistory = mHistory;

                                    List<string> mAttachments = [];
                                    int aCount = br.ReadInt32();
                                    for (int a = 0; a < aCount; a++)
                                    {
                                        mAttachments.Add(br.ReadString());
                                    }

                                    // add attachments
                                    item.Attachments = mAttachments;

                                    // additional notes
                                    item.Notes = br.ReadString();

                                    // add item to profile
                                    profile.Items.Add(item);
                                }
                            }
                        }

                        // add loaded profile
                        profiles.Add(profile);
                    }

                    catch (Exception ex)
                    {
                        Log.Error(ex, nameof(GetProfilesAsync));
                        continue;
                    }
                }

                return profiles;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(GetProfilesAsync));
                return [];
            }
        });

        return profiles;
    }
}
