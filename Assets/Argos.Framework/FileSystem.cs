using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

namespace Argos.Framework.IO
{
    /// <summary>
    /// Multiplatform Input/Output file system manager.
    /// </summary>
    public sealed class FileSystem
    {
        #region Enums
        /// <summary>
        /// Game path constants.
        /// </summary>
        public enum GamePaths
        {
            /// <summary>
            /// Base directory for storing persistent data.
            /// </summary>
            Base,
            /// <summary>
            /// Game settings data folder.
            /// </summary>
            Settings,
            /// <summary>
            /// Game savegames data folder.
            /// </summary>
            SaveGames,
            /// <summary>
            /// Screenshots data folder.
            /// </summary>
            ScreenShots,
            /// <summary>
            /// Folder for store custom data created by player.
            /// </summary>
            CustomData
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Return the base data storage path for manage settings, savegames and other game data files.
        /// </summary>
        /// <remarks>The path format is: "%user_documents_folder%\%company_name%\%product_name%\"</remarks>
        public string BasePath { get; private set; }

        /// <summary>
        /// Settings path.
        /// </summary>
        public string SettingsPath { get; private set; }

        /// <summary>
        /// Save games path.
        /// </summary>
        public string SaveGamesPath { get; private set; }

        /// <summary>
        /// Screenshots path.
        /// </summary>
        public string ScreenShotsPath { get; private set; }

        /// <summary>
        /// Custom data path.
        /// </summary>
        public string CustomDataPath { get; private set; }
        #endregion

        #region Constructors
        public FileSystem()
        {
#if UNITY_STANDALONE
            // Using user documents folder as base path:
            this.BasePath = this.GenerateDataPath($@"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}\{Application.companyName}\{Application.productName}\");

            Debug.Log($"Base path for user data and settings storage: {this.BasePath}\n");

            // Settings path:
            this.SettingsPath = this.GenerateDataPath($@"{this.BasePath}\Settings\");

            // Save games path:
            this.SaveGamesPath = this.GenerateDataPath($@"{this.BasePath}\Save Games\");

            // Screenshots path:
            this.ScreenShotsPath = this.GenerateDataPath($@"{this.BasePath}\Screenshots\");
            
            // Custom data path:
            this.CustomDataPath = this.GenerateDataPath($@"{this.BasePath}\Custom Data\");

#else
            // TODO: See each platform (XBox One, PS4, Nintendo Switch) for right implementation.
#endif
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Generate path, fix formatting, and create target directory if not exists.
        /// </summary>
        /// <param name="path">Desired path.</param>
        /// <returns>Fixed safe path.</returns>
        string GenerateDataPath(string path)
        {
            var finalPath = this.RemoveIllegalCharacters(path);

#if !UNITY_STANDALONE_WIN && !UNITY_XBOXONE && !UNITY_WSA && !UNITY_WSA_10_0
            // On non Microsoft systems (Linux, OSX, PS4...), change Windows path separator character to Unix path separator character:
            finalPath = finalPath.Replace('\\', '/');
#endif

            this.CreateDirectoryIfNotExist(finalPath);

            return finalPath;
        }

        /// <summary>
        /// Remove all illegar path characters.
        /// </summary>
        /// <param name="path">Desired path.</param>
        /// <returns>Fixed safe path.</returns>
        string RemoveIllegalCharacters(string path)
        {
            return new string(path.Where(e => !Path.GetInvalidPathChars().Contains(e)).ToArray()).Replace(@"\"[0], System.IO.Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Create target path directory if not exist.
        /// </summary>
        /// <param name="path">Desired path.</param>
        void CreateDirectoryIfNotExist(string path)
        {
#if UNITY_STANDALONE
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
#else
            // TODO: See each platform (XBox One, PS4, Nintendo Switch) for right implementation.
#endif
        }

        /// <summary>
        /// Formatting predefined path.
        /// </summary>
        /// <param name="path">Constant game path.</param>
        /// <param name="filename">Name of the file.</param>
        /// <returns>Formatted game path with the filename.</returns>
        public string FormatPath(GamePaths path, string filename)
        {
            string finalPath = "";
            switch (path)
            {
                case GamePaths.Base:
                    finalPath = this.BasePath;
                    break;
                case GamePaths.Settings:
                    finalPath = this.SettingsPath;
                    break;
                case GamePaths.SaveGames:
                    finalPath = this.SaveGamesPath;
                    break;
                case GamePaths.ScreenShots:
                    finalPath = this.ScreenShotsPath;
                    break;
                case GamePaths.CustomData:
                    finalPath = this.CustomDataPath;
                    break;
            }

            return $"{finalPath}{filename}";
        }

        /// <summary>
        /// Save data to disc.
        /// </summary>
        /// <param name="path">Predefined path where the file is stored.</param>
        /// <param name="filename">Name of the file to save the data.</param>
        /// <param name="data">Data value.</param>
        public void SaveData(GamePaths path, string filename, object data)
        {
#if UNITY_STANDALONE
            File.WriteAllText(this.FormatPath(path, filename), JsonUtility.ToJson(data, true));
#else
            // TODO: See each platform (XBox One, PS4, Nintendo Switch) for right implementation.
#endif
        }

        /// <summary>
        /// Load data from disc.
        /// </summary>
        /// <typeparam name="T">Type of the data to read.</typeparam>
        /// <param name="path">Predefined path where the file is stored.</param>
        /// <param name="filename">Name of the file to load the data.</param>
        /// <returns>Return the deserialized data.</returns>
        public T LoadData<T>(GamePaths path, string filename)
        {
#if UNITY_STANDALONE
            return JsonUtility.FromJson<T>(File.ReadAllText(this.FormatPath(path, filename)));
#else
            // TODO: See each platform (XBox One, PS4, Nintendo Switch) for right implementation.
#endif
        }

        /// <summary>
        /// Check if a file exists.
        /// </summary>
        /// <param name="path">Predefined path where the file is stored.</param>
        /// <param name="filename">Name of the file.</param>
        /// <returns>Return true if the file exists.</returns>
        public bool FileExist(GamePaths path, string filename)
        {
#if UNITY_STANDALONE
            return File.Exists(this.FormatPath(path, filename));
#else
            // TODO: See each platform (XBox One, PS4, Nintendo Switch) for right implementation.
#endif
        }
        #endregion
    }
}