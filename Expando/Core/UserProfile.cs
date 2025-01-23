using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        Id = 0;
        Username = Environment.UserName;
        CreationDate = DateTime.Now;
    }

    /// <summary>
    /// Representing the user identification number.
    /// </summary>
    public long Id { get; init; }

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

    #region Static methods and members

    /// <summary>
    /// Representing a path to the users index file.
    /// </summary>
    public const string UsersIndexFile = "UsersIndexes.bin";

    /// <summary>
    /// Representing a folder where all users data are stored (a folder where other users folders are stored).
    /// </summary>
    public const string UserFolders = "Data";

    private static List<UserProfile> _profiles = [];

    /// <summary>
    /// Representing a list of all loaded users.
    /// </summary>
    public static List<UserProfile> Profiles => _profiles;

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
    public static bool LoadUsers(string usersIndexFile, out Dictionary<long, string> usersData)
    {
        usersData = [];

        // file not found
        if (File.Exists(usersIndexFile) == false) return false;

        using (FileStream fs = File.OpenRead(usersIndexFile))
        {
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // number of all users
                int count = reader.ReadInt32();

                // reads all users entries
                for (int x = 0; x < count; x++)
                {
                    long userId = reader.ReadInt64();       // user Id
                    string userDir = reader.ReadString();   // path to the user's folder

                    // adds loaded data into the return dictionary
                    usersData.Add(userId, userDir);
                }
            }
        }

        return true;
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

        catch (Exception)
        {
            // unable to create the index file
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
                string transactionsPath = Path.Combine(userDir, "transactions.bin");
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

        catch (Exception)
        {
            // unable to create FS objects
            return false;
        }
    }

    private static bool AddUserToTheIndexFile(ref UserProfile user)
    {
        try
        {
            using (FileStream fs = new FileStream(UsersIndexFile, FileMode.Append, FileAccess.ReadWrite))
            {
                int count = 0;
                using (BinaryReader reader = new BinaryReader(fs))
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

        catch (Exception)
        {
            // unable to operate with the index file
            return false;
        }
    }

    #endregion
}
