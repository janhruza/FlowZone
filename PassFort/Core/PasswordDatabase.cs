using System;
using System.IO;
using System.IO.Compression;
using FZCore;

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


        private static readonly string _metadata    = "metadata";
        private static readonly string _passwords   = "index";

        /// <summary>
        /// Creates a new empty database structure.
        /// </summary>
        /// <param name="filePath">Path to the existing database.</param>
        public PasswordDatabase(string? filePath)
        {
            _filePath = filePath;
            Id = Guid.CreateVersion7();
            Name = $"Database";
            CreationTime = DateTime.Now;
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
         *  -   Database name               string
         *  -   Creation time (in binary)   long
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
                if (File.Exists(_dirPath) == false)
                {
                    Log.Error("No database is opened.", nameof(ReadMetadata));
                    return false;
                }

                string path = Path.Combine(_dirPath, _metadata);
                using (FileStream fs = File.OpenRead(path))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        this.Name = br.ReadString();
                        this.CreationTime = DateTime.FromBinary(br.ReadInt64());
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

        public bool ReadPasswordEntries(out PasswordCollection entries)
        {
            entries = [];

            if (_dirPath == null)
            {
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
                if (File.Exists(Path.Combine(_dirPath, line)) == false)
                {
                    // file not found, skip
                    continue;
                }


            }

            return true;
        }

        #endregion

        #region Write methods

        public bool WritePasswordEntries(PasswordCollection entries)
        {
            if (_dirPath == null)
            {
                return false;
            }

            return true;
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
                    Log.Error("No database is opened.", nameof(WriteMetadata));
                    return false;
                }

                string path = Path.Combine(_dirPath, _metadata);
                using (FileStream fs = File.Create(path))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(this.Name);                        // string
                        bw.Write(this.CreationTime.ToBinary());     // long
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
                _dirPath = dirName;
                return true;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(OpenArchive));
                return false;
            }
        }

        /// <summary>
        /// Closes opened archive file and writes all the data into the source file (resaves the file).
        /// </summary>
        /// <returns>True, if closing and saving is successful, otherwise false.</returns>
        public bool CloseArchive()
        {
            try
            {
                if (File.Exists(_filePath) == false)
                {
                    Log.Error("No archive is loaded.", nameof(CloseArchive));
                    return false;
                }

                if (Directory.Exists(_dirPath) == false)
                {
                    Log.Error("No archive is opened.", nameof(CloseArchive));
                    return false;
                }

                ZipFile.CreateFromDirectory(_dirPath, _filePath, CompressionLevel.NoCompression, false);

                // delete the temp folder and clear the _dirPath
                Directory.Delete(this._dirPath, true);
                _dirPath = null;

                return true;
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(CloseArchive));
                return false;
            }
        }

        #endregion
    }
}
