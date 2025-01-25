using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FZCore;

namespace Expando.Core;

/// <summary>
/// Representing a user profile data class.
/// </summary>
public class UserProfile
{
    /// <summary>
    /// Creates a new instance of the <see cref="UserProfile"/> class with the default values.
    /// </summary>
    public UserProfile()
    {
        Id = ulong.MinValue;
        Username = Environment.UserName;
        CreationDate = DateTime.MinValue;
    }

    /// <summary>
    /// Representing the user identification number.
    /// </summary>
    public ulong Id { get; init; }

    /// <summary>
    /// Representing the user's name.
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    /// Representing the creation date of the profile.
    /// </summary>
    public DateTime CreationDate { get; init; }

    private List<Transaction> _transactions = [];

    /// <summary>
    /// Representing a list of all user transactions.
    /// </summary>
    public List<Transaction> Transactions => _transactions;

    /// <summary>
    /// Gets a new list of all the user's expanses.
    /// </summary>
    /// <returns></returns>
    public List<Transaction> GetExpanses()
    {
        List<Transaction> list = [];
        if (_transactions == null) return list;

        // gets only the expanses
        list = _transactions.Where(x => x.Type == Transaction.TypeExpanse).ToList();
        return list;
    }

    /// <summary>
    /// Gets a new list of all the user's incomes.
    /// </summary>
    /// <returns></returns>
    public List<Transaction> GetIncomes()
    {
        List<Transaction> list = [];
        if (_transactions == null) return list;

        // gets only the incomes
        list = _transactions.Where(x => x.Type == Transaction.TypeIncome).ToList();
        return list;
    }

    /// <summary>
    /// Attempts to save the list of user's transactions to the transactions file. THis operation will overwrite the user's transactions list.
    /// </summary>
    /// <returns>True if the reqrite is successful, otherwise false.</returns>
    public bool SaveTransactions()
    {
        try
        {
            int count = Transactions.Count;
            string path = Path.Combine(UserFolders, this.Id.ToString(), TransactionsFile);
            using (FileStream fs = File.Create(path))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(count);

                    for (int x = 0; x < count; x++)
                    {
                        // the transaction object
                        Transaction tr = Transactions[x];

                        // transaction data
                        bw.Write(tr.Id);
                        bw.Write(tr.UserId);
                        bw.Write(tr.Timestamp);
                        bw.Write(tr.Type);
                        bw.Write(tr.Value);
                        bw.Write(tr.Description);
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    #region Static methods and members

    /// <summary>
    /// Representing a path to the users index file.
    /// </summary>
    public const string UsersIndexFile = "UserIndex.bin";

    /// <summary>
    /// Representing a transactions file name.
    /// </summary>
    public const string TransactionsFile = "transactions.bin";

    /// <summary>
    /// Representing a folder where all users data are stored (a folder where other users folders are stored).
    /// </summary>
    public const string UserFolders = "Data";

    private static List<UserProfile> _profiles = [];

    /// <summary>
    /// Representing a list of all loaded users.
    /// </summary>
    public static List<UserProfile> Profiles => _profiles;

    /// <summary>
    /// Representing the currently selected user profile (if any).
    /// </summary>
    public static UserProfile? Current { get; set; }

    /// <summary>
    /// Determines whether any user profile is loaded.
    /// </summary>
    /// <returns>True if any user profile is loaded, otherwise false.</returns>
    public static bool IsProfileLoaded()
    {
        if (Current == null) return false;
        if (Current.Id == ulong.MinValue) return false;

        return true;
    }

    /*
     * Users Index File structure
     *  - Number of users:  int     (4 bytes)
     *  - Single user data
     *      - User ID:      long    (8 bytes)
     *      - Path length:  int     (4 bytes)
     *      - Folder path:  string  (Path length bytes)
     */

    /// <summary>
    /// Loads the users data from the users index file. This will not load any <see cref="UserProfile"/>s but a <see cref="Dictionary{TKey, TValue}"/> of user Id's and paths to their directories.
    /// </summary>
    /// <param name="usersIndexFile">Path to the users index file.</param>
    /// <param name="usersData">Where the users data will be stored.</param>
    /// <returns>True on success, false on error.</returns>
    public static bool LoadUsers(string usersIndexFile, out Dictionary<ulong, string> usersData)
    {
        usersData = [];

        // file not found
        if (File.Exists(usersIndexFile) == false)
        {
            // file not found but it may indicate first use where no profiles are created
            return true;
        }

        using (FileStream fs = File.OpenRead(usersIndexFile))
        {
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // number of all users
                int count = reader.ReadInt32();

                // reads all users entries
                for (int x = 0; x < count; x++)
                {
                    ulong userId = reader.ReadUInt64();     // user Id
                    string userDir = reader.ReadString();   // path to the user's folder

                    // adds loaded data into the return dictionary
                    usersData.Add(userId, userDir);
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Attempts to load all the saved users into the <see cref="Profiles"/>.
    /// </summary>
    /// <returns>A nonzero value if an error occurred, otherwise 0.</returns>
    public static int LoadUsersData()
    {
        try
        {
            // Load users index file
            if (LoadUsers(UsersIndexFile, out Dictionary<ulong, string> userData) == false)
            {
                // unable to read users index file
                return 1;
            }

            // clear the profiles list
            _profiles = [];

            // handle all users, one folder at a time
            foreach (var key in userData)
            {
                string path = key.Value;
                if (Directory.Exists(path) == false)
                {
                    // user folder not found, user record is invalid
                    // not a critical error, just a warning (in this context)
                    continue;
                }

                // read user data
                string infoPath = Path.Combine(path, "profile.bin");

                if (LoadUserData(infoPath, out UserProfile profile) == false)
                {
                    // unable to load profile data
                    // another warning, profile will be skipped
                    continue;
                }

                // read transactions data
                string transPath = Path.Combine(path, TransactionsFile);

                if (LoadTransactions(transPath, ref profile) == false)
                {
                    // unable to load list of user's transactions
                    // not critical, the user will be skipped (may change in the future to be treated as an error)
                    continue;
                }

                // add loaded profile into the list of profiles
                _profiles.Add(profile);
            }

            return 0;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return ex.HResult;
        }
    }

    private static bool LoadUserData(string userInfoFile, out UserProfile profile)
    {
        // default init
        profile = new UserProfile();

        try
        {
            if (File.Exists(userInfoFile) == false)
            {
                return false;
            }

            // reinitialization of the return object
            using (FileStream fs = File.OpenRead(userInfoFile))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    profile = new UserProfile
                    {
                        Id = br.ReadUInt64(),                               // long
                        Username = br.ReadString(),                         // string
                        CreationDate = DateTime.FromBinary(br.ReadInt64()), // long
                    };
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            // unable to manipulate with the profile info file or invalid data inside
            Log.Error(ex);
            return false;
        }
    }

    private static bool LoadTransactions(string transactionsFile, ref UserProfile profile)
    {
        try
        {
            if (File.Exists(transactionsFile) == false)
            {
                // file not found
                return false;
            }

            // initializes the user's transaction history
            profile._transactions = [];

            using (FileStream fileStream = File.OpenRead(transactionsFile))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    // get number of transactions
                    int count = reader.ReadInt32();

                    for (int x = 0; x < count; x++)
                    {
                        Transaction transaction = new Transaction
                        {
                            Id = reader.ReadUInt64(),
                            UserId = reader.ReadUInt64(),
                            Timestamp = reader.ReadInt64(),
                            Type = reader.ReadBoolean(),
                            Value = reader.ReadDecimal(),
                            Description = reader.ReadString()
                        };

                        // adds the transaction into the list
                        profile._transactions.Add(transaction);
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    /*
     * User folder structure
     * 
     * FOLDER STRUCTURE
     * 
     *  -   FILENAME            DESCRIPTION
     *  -   PROFILE.BIN         User's data
     *  -   TRANSACTIONS.BIN    List of all the user's transactions
     *  
     *  FILES STRUCTURE
     *  
     *  PROFILE.BIN
     *  -   PROPERTY            TYPE    SIZE (bytes)
     *  -   ID                  long    8
     *  -   Username length     int     4
     *  -   Username            string  previous value
     *  -   Creation timestamp  long    8
     *  
     *  TRANSACTIONS.BIN
     *  -   PROPERTY            TYPE    SIZE (bytes)
     *  -   ID                  long    8
     *  -   User ID             long    8
     *  -   Timestamp           long    8
     *  -   Type                bool    1
     *  -   Value               decimal 16
     *  -   Description length  int     4
     *  -   Description         string  previous value
     */

    /// <summary>
    /// Creates a new user, writes it's user profile structure to the drive and updates the users index file.
    /// </summary>
    /// <param name="user">Reference to the new user class.</param>
    /// <returns>Nonzero value if an error occurred, otherwise 0.</returns>
    /// <remarks>
    /// Return values
    /// <list type="bullet">0:  Success</list>
    /// <list type="bullet">1:  Unable to create the index file.</list>
    /// <list type="bullet">2:  Unable to create user data on the filesystem.</list>
    /// <list type="bullet">3:  Unable to add user to the index file.</list>
    /// </remarks>
    public static int CreateUser(ref UserProfile user)
    {
        // check if users indexes file exists
        if (File.Exists(UsersIndexFile) == false)
        {
            // need to create users index file
            if (CreateUsersIndexFile() == false) return 1; // unable to create index file
        }

        // add user to the file system (create their folder and put the data files in there)
        if (WriteUserData(ref user) == false)
        {
            // unable to create FS data or user with given ID already exists
            return 2;
        }

        // add user into the index file
        if (AddUserToTheIndexFile(ref user) == false)
        {
            // unable to modify the index file
            return 3;
        }

        return 0;
    }

    private static bool CreateUsersIndexFile()
    {
        try
        {
            using (FileStream fs = File.Create(UsersIndexFile))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    // 0 means no users
                    writer.Write(0);
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            // unable to create the index file
            Log.Error(ex);
            return false;
        }
    }

    private static bool WriteUserData(ref UserProfile user)
    {
        try
        {
            // create user folder
            string userDir = Path.Combine(UserFolders, user.Id.ToString());
            if (Directory.Exists(userDir) == true)
            {
                // user already exists
                return false;
            }

            // creates the user folder
            _ = Directory.CreateDirectory(userDir);

            // writes the PROFILE.BIN file
            {
                string profileDataPath = Path.Combine(userDir, "profile.bin");
                using (FileStream fileStream = File.Create(profileDataPath))
                {
                    using (BinaryWriter bw = new BinaryWriter(fileStream))
                    {
                        // write ID, Username and creation timestamp
                        bw.Write(user.Id);                      // User ID
                        bw.Write(user.Username);                // Username
                        bw.Write(user.CreationDate.ToBinary()); // Profile creation time
                    }
                }
            }

            // writes the TRANSACTIONS.BIN file
            {
                string transactionsPath = Path.Combine(userDir, TransactionsFile);
                using (FileStream fileStream = File.Create(transactionsPath))
                {
                    using (BinaryWriter bw = new BinaryWriter(fileStream))
                    {
                        bw.Write(0); // 0, no transactions yet
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            // unable to create FS objects
            Log.Error(ex);
            return false;
        }
    }

    private static bool AddUserToTheIndexFile(ref UserProfile user)
    {
        try
        {
            using (FileStream fs = File.Open(UsersIndexFile, FileMode.Open, FileAccess.ReadWrite))
            {
                int count;

                // it's important to leave the stream open for the BinaryWriter that writes data into the stream later
                using (BinaryReader reader = new BinaryReader(fs, Encoding.Default, leaveOpen:true))
                {
                    count = reader.ReadInt32();
                    count++;
                }

                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    // rewrite the user count on the begining
                    fs.Seek(0, SeekOrigin.Begin);
                    writer.Write(count);

                    // append data to the end
                    fs.Seek(0, SeekOrigin.End);
                    writer.Write(user.Id);          // User ID (long)

                    string path = Path.Combine(UserFolders, user.Id.ToString());
                    writer.Write(path);             // path to the created user folder
                }

            }
            return true;
        }

        catch (Exception ex)
        {
            // unable to operate with the index file
            Log.Error(ex);
            return false;
        }
    }

    /// <summary>
    /// Recreates the user index file based on the current list of all loaded users. Current solution when users are deleted.
    /// </summary>
    /// <returns>True if the operation is successful, otherwise false.</returns>
    public static bool RebuildUserIndexFile()
    {
        try
        {
            if (UserProfile.Profiles.Count == 0)
            {
                // no loaded profiles or
                // no profiles at all
                return false;
            }

            using (FileStream fs = File.Create(UsersIndexFile))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(UserProfile.Profiles.Count);

                    for (int x = 0; x < UserProfile.Profiles.Count; x++)
                    {
                        writer.Write(UserProfile.Profiles[x].Id);
                        string path = Path.Combine(UserFolders, UserProfile.Profiles[x].Id.ToString());
                        writer.Write(path);
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    #endregion
}
