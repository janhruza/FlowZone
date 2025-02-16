using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Representing the user profiles folder.
    /// </summary>
    public static string ProfilesFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");

    /// <summary>
    /// Creates a new instance of the <see cref="UserProfile"/> class.
    /// </summary>
    public UserProfile()
    {
        Id = ulong.MaxValue;
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

            string profileData = Path.Combine(userFolder, "settings.json");
            string userItems = Path.Combine(userFolder, "items.bin");

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
                string path = Path.Combine(ProfilesFolder, profile.Id.ToString(), "items.bin");
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
}
