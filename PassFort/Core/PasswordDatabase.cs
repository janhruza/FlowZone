using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using FZCore;

using static PassFort.Messages;

namespace PassFort.Core
{
    /// <summary>
    /// Representing the password database file structure.
    /// </summary>
    public class PasswordDatabase
    {
        /// <summary>
        /// Loaded archive path.
        /// </summary>
        private string? _filePath;

        /// <summary>
        /// Extracted archive directory path. Database files are located there.
        /// </summary>
        private string? _dirPath;

        private PasswordCollection _entries = [];

        /// <summary>
        /// Representing the list of all loaded password entries.
        /// </summary>
        public PasswordCollection Entries => _entries;

        private static readonly string _metadata    = "metadata";
        private static readonly string _passwords   = "index";
        private static readonly int _keySize = 32;
        private static readonly int _ivSize = 16;

        private byte[] GenerateBytes(int count)
        {
            using (RandomNumberGenerator rnd = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[count];
                rnd.GetBytes(bytes);
                return bytes;
            }
        }

        /// <summary>
        /// Creates a new empty database structure.
        /// </summary>
        /// <param name="filePath">Path to the existing database.</param>
        /// <param name="name">Name of the database.</param>
        public PasswordDatabase(string? filePath = null, string? name = null)
        {
            _filePath = filePath;
            Id = Guid.CreateVersion7();
            Name = (string.IsNullOrEmpty(name) == false ? name : "Database");
            CreationTime = DateTime.Now;

            // get IV and Key
            _key = GenerateBytes(_keySize);
            _iV = GenerateBytes(_ivSize);
        }

        #region Metadata

        /// <summary>
        /// Representing the database name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Representing the database creation time.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Representing the database ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Representing the encryption key.
        /// </summary>
        private byte[] _key { get; set; }

        /// <summary>
        /// Representing the encryption IV.
        /// </summary>
        private byte[] _iV { get; set; }

        #endregion

        /*
         * DATABASE FILE FORMAT
         * 
         * FILE
         *  - metadata      File with database info
         *  - password1     Password item 1
         *  - password2...  Password item 2 (and so on)
         *  
         *  Passwords are stored in their own files, encrypted, along with their own metadata
         *  
         * METADATA file (binary)
         *  -   ID                          GUID
         *  -   Database name               string
         *  -   Creation time (in binary)   long
         *  -   Key                         byte[32]
         *  -   IV                          byte[16]
         *  
         * PASSWORD files (binary)
         *  -   ID                          GUID
         *  -   Creation time               long
         *  -   Title                       string
         *  -   Service address             string
         *  -   Username                    string
         *  -   Password (encrypted)        string
         *  -   Additional notes            string
         */

        #region Read methods

        /// <summary>
        /// Reads metadata from the given password file.
        /// </summary>
        /// <returns></returns>
        public bool ReadMetadata()
        {
            try
            {
                if (Directory.Exists(_dirPath) == false)
                {
                    Log.Error(NO_DB_OPENED, nameof(ReadMetadata));
                    return false;
                }

                string path = Path.Combine(_dirPath, _metadata);
                using (FileStream fs = File.OpenRead(path))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        this.Id = new Guid(br.ReadBytes(16));
                        this.Name = br.ReadString();
                        this.CreationTime = DateTime.FromBinary(br.ReadInt64());
                        this._key = br.ReadBytes(_keySize);
                        this._iV = br.ReadBytes(_ivSize);
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(ReadMetadata));
                return false;
            }
        }

        /// <summary>
        /// Reads all the stored password entries from the opened database file. It also stores the list of <paramref name="entries"/> in <see cref="_entries"/>.
        /// </summary>
        /// <param name="entries">List of entries that will be returned. If an error occurred, an emty list is returned.</param>
        /// <returns>True, if the operation succeeded, otherwise false.</returns>
        public bool ReadPasswordEntries(out PasswordCollection entries)
        {
            try
            {
                entries = [];
                _entries = entries;

                if (_dirPath == null)
                {
                    Log.Error(NO_DB_OPENED, nameof(ReadPasswordEntries));
                    return false;
                }

                string path = Path.Combine(_dirPath, _passwords);
                string[] lines = File.ReadAllLines(path);

                if (lines.Length == 0)
                {
                    // no passwords saved
                    return true;
                }

                // list files
                foreach (string line in lines)
                {
                    string file = Path.Combine(_dirPath, line);
                    if (File.Exists(path) == false)
                    {
                        // file not found, skip
                        continue;
                    }

                    // gets the password data and structures them
                    ReadOnlySpan<byte> bytes = File.ReadAllBytes(file);
                    PasswordEntry? pe = PasswordEntry.ReadFromData(bytes);

                    // input data check
                    if (pe == null)
                    {
                        Log.Warning($"Unable to parse password entry \'{file}\'.", nameof(ReadPasswordEntries));
                        continue;
                    }

                    // decrypt password entry
                    if (DecryptEntry(pe) == false)
                    {
                        App.CriticalError(CANT_DECRYPT_ENTRY, nameof(ReadPasswordEntries));
                    }

                    entries.Add(pe);
                }

                return true;
            }

            catch (Exception ex)
            {
                entries = [];
                Log.Error(ex, nameof(ReadPasswordEntries));
                return false;
            }
        }

        #endregion

        #region Write methods

        /// <summary>
        /// Writes all password <paramref name="entries"/> into the opened database file.
        /// </summary>
        /// <param name="entries">List of entries to be wrtten into the file.</param>
        /// <returns>True, if the operation succeeded, otherwise false.</returns>
        public bool WritePasswordEntries(PasswordCollection entries)
        {
            try
            {
                if (_dirPath == null)
                {
                    return false;
                }

                string indexFile = Path.Combine(_dirPath, _passwords);
                List<string> list = [];

                foreach (PasswordEntry entry in entries)
                {
                    // encrypts the entry
                    if (EncryptEntry(entry) == false)
                    {
                        // critical error, show message
                        App.CriticalError(CANT_ENCRYPT_ENTRY, nameof(WritePasswordEntries));
                    }

                    using (MemoryStream? ms = PasswordEntry.WriteRawData(entry))
                    {
                        if (ms == null)
                        {
                            continue;
                        }

                        // gets the id as the filename
                        // name is base64 encoded entry ID, without padding equal signs (=)
                        string name = Convert.ToBase64String(entry.Id.ToByteArray()).TrimEnd('=');
                        string path = Path.Combine(_dirPath, name);
                        File.WriteAllBytes(path, ms.ToArray());

                        // register file into the index file
                        list.Add(name);
                    }
                }

                // rewrites the index file
                File.WriteAllLines(indexFile, list);

                return true;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(WritePasswordEntries));
                return false;
            }
        }

        /// <summary>
        /// Writes metadata into a file inside a TEMP directory.
        /// </summary>
        /// <returns></returns>
        public bool WriteMetadata()
        {
            try
            {
                if (_dirPath == null)
                {
                    Log.Error(NO_DB_OPENED, nameof(WriteMetadata));
                    return false;
                }

                string path = Path.Combine(_dirPath, _metadata);
                using (FileStream fs = File.Create(path))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(this.Id.ToByteArray());
                        bw.Write(this.Name);                        // string
                        bw.Write(this.CreationTime.ToBinary());     // long
                        bw.Write(this._key);                        // bytes (32)
                        bw.Write(this._iV);                         // bytes (16)
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(WriteMetadata));
                return false;
            }
        }

        #endregion

        #region Archiving methods

        /// <summary>
        /// Opens the database file for the future operations such as reading metadata and stored password data.
        /// </summary>
        /// <returns>True, if file was opened, otherwise false.</returns>
        public bool OpenArchive()
        {
            try
            {
                if (File.Exists(_filePath) == false)
                {
                    Log.Error("File not found.", nameof(OpenArchive));
                    return false;
                }

                string dirName = Path.Combine(Path.GetTempPath(), Guid.CreateVersion7().ToString());
                ZipFile.ExtractToDirectory(_filePath, dirName);

                // load the _dirPath with the recieved temp path
                this._dirPath = dirName;
                _current = this;
                return true;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(OpenArchive));
                return false;
            }
        }

        /// <summary>
        /// Closes opened archive file and writes all the data into the source file.
        /// </summary>
        /// <returns>True, if closing and saving is successful, otherwise false.</returns>
        public bool CloseArchive()
        {
            try
            {
                if (_filePath == null)
                {
                    Log.Error(NO_DB_OPENED, nameof(CloseArchive));
                    return false;
                }

                if (Directory.Exists(_dirPath) == false)
                {
                    Log.Error(NO_DB_OPENED, nameof(CloseArchive));
                    return false;
                }

                ZipFile.CreateFromDirectory(_dirPath, _filePath, CompressionLevel.NoCompression, false);

                // delete the temp folder and clear the _dirPath
                Directory.Delete(this._dirPath, true);
                _dirPath = null;
                _current = null;

                return true;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(CloseArchive));
                return false;
            }
        }

        /// <summary>
        /// Attempts to rewrite the metadata file and the password index file.
        /// </summary>
        /// <returns>True, if the operation s successful, otherwise false.</returns>
        public bool Save()
        {
            try
            {
                if (_filePath == null)
                {
                    Log.Error(NO_DB_OPENED, nameof(Save));
                    return false;
                }

                if (Directory.Exists(_dirPath) == false)
                {
                    Log.Error(NO_DB_OPENED, nameof(Save));
                    return false;
                }

                return (WriteMetadata() && WritePasswordEntries(_entries));
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(CloseArchive));
                return false;
            }
        }

        /// <summary>
        /// Determines whether the password database is opened or not.
        /// </summary>
        /// <returns>True, if the database file is opened, otherwise false.</returns>
        public bool IsOpened()
        {
            return _dirPath != null;
        }

        #endregion

        #region Encryption-related methods

        /// <summary>
        /// Encrypts the plain text string using AES and returns the encrypted string encoded as base64 string.
        /// </summary>
        /// <param name="value">Plain text strng to be encrypted.</param>
        /// <returns>An encrypted value of <paramref name="value"/> as base64 string, otherwise null.</returns>
        private string? EncryptString(string value)
        {
            if (IsOpened() == false)
            {
                Log.Error(NO_DB_OPENED, nameof(EncryptString));
                return null;
            }

            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iV;
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    var plainBytes = Encoding.UTF8.GetBytes(value);
                    var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        /// <summary>
        /// Decrypts AES encoded data into a plain text string.
        /// </summary>
        /// <param name="value">A base64 encoded string that contains AES encrypted data.</param>
        /// <returns>A plain text result of the AES decryption as string. If an error occurrs, null is returned.</returns>
        private string? DecryptString(string value)
        {
            if (IsOpened() == false)
            {
                Log.Error(NO_DB_OPENED, nameof(DecryptString));
                return null;
            }

            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iV;
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    var encryptedBytes = Convert.FromBase64String(value);
                    var plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }

        private bool EncryptEntry(PasswordEntry entry)
        {
            if (IsOpened() == false)
            {
                Log.Error(NO_DB_OPENED, nameof(EncryptEntry));
                return false;
            }

            // encrypts all sensitive fields/properties inside of the entry
            entry.Username = EncryptString(entry.Username);
            entry.Password = EncryptString(entry.Password);
            return true;
        }

        private bool DecryptEntry(PasswordEntry entry)
        {
            if (IsOpened() == false)
            {
                Log.Error(NO_DB_OPENED, nameof(DecryptEntry));
                return false;
            }

            // decrypts all sensitive fields/properties inside of the entry
            entry.Username = DecryptString(entry.Username);
            entry.Password = DecryptString(entry.Password);
            return true;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Creates a new database file with the specified database <paramref name="name"/>. Location is determined by the <paramref name="path"/> argument.
        /// </summary>
        /// <param name="name">Database name.</param>
        /// <param name="path">Path where the database file will be created.</param>
        /// <returns>An instance of the newly created <see cref="PasswordDatabase"/>, otherwise null.</returns>
        public static PasswordDatabase? Create(string name, string path)
        {
            // create instance
            PasswordDatabase db = new PasswordDatabase(filePath:path, name:name);

            // write data
            // create temp folder
            db._dirPath = Path.Combine(Path.GetTempPath(), ((ulong)DateTime.Now.ToBinary()).ToString());
            _ = Directory.CreateDirectory(db._dirPath);

            if (db.WriteMetadata() == false)
            {
                Log.Error("Unable to create metadata file.", nameof(Create));
                return null;
            }

            if (db.WritePasswordEntries(db._entries) == false)
            {
                Log.Error("Unable to create password index file.", nameof(Create));
                return null;
            }

            if (db.CloseArchive() == false)
            {
                Log.Error("Unable to create database file.", nameof(Create));
                return null;
            }

            // return db
            return db;
        }

        private static PasswordDatabase? _current;

        /// <summary>
        /// Representing the last opened database.
        /// When the <see cref="OpenArchive"/> is called, the value is set to the calling database and when
        /// the <see cref="CloseArchive"/> is called, the value is set to null.
        /// </summary>
        public static PasswordDatabase? Current => _current;

        #endregion
    }
}
