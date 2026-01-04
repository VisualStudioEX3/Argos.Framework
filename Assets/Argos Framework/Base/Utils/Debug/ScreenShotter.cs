using UnityEngine;
using System.Collections;
using Argos.Framework;

namespace Argos.Framework.Utils.Debug
{
    /// <summary>
    /// Utility for make screenshots in gameplay and editor mode.
    /// </summary>
    /// <remarks>For now, only works on desktop platforms.</remarks>
    [AddComponentMenu("Argos.Framework/Utils/Debug/Screen Shotter"), DisallowMultipleComponent, ExecuteInEditMode]
    public sealed class ScreenShotter : MonoBehaviour
    {
        #region Constants
        const string DEFAULT_NAME = "Screenshot";
        #endregion

        #region Internal vars
        string _path;
        string _lastName;
#pragma warning disable 414
        [SerializeField, DynamicLabel(true, true)]
        string _finalName;
#pragma warning restore
        #endregion

        #region Public vars
        [Tooltip("Only works in play mode. Use \"Take Screenshot\" button in editor mode.")]
        public KeyCode screenshotKey = KeyCode.F12;
        public new string name = DEFAULT_NAME;
        #endregion

        #region Properties
        public string ScreenshotsPath
        {
            get
            {
                if (string.IsNullOrEmpty(this._path))
                {
                    this._path = $"{Application.dataPath.Remove(Application.dataPath.LastIndexOf('/'))}/Screenshots/";
                }

                return this._path;
            }
        }
        #endregion

        #region Update logic
        void Update()
        {
            // TODO: Study how to implement in a way to apply for other platforms like consoles.
            if (Utils.ApplicationUtility.IsDesktopPlatform)
            {
                if (string.IsNullOrEmpty(this.name))
                {
                    UnityEngine.Debug.LogError("ScreenShotter: The Name field never be an empty string.");
                    this.name = DEFAULT_NAME;
                }

                // Generate preview:
                if (this._lastName != this.name)
                {
                    this._lastName = this.name;
                    this._finalName = this.CreateFileName();
                }

                if (UnityEngine.Input.GetKeyDown(this.screenshotKey))
                {
                    this.TakeScreenshot();
                }
            }
        }
        #endregion

        #region Methods & Functions
        string CreateFileName()
        {
            // TODO: Study how to implement in a way to apply for other platforms like consoles.
            return $"{this.name} {System.DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")}.png";
        }

        /// <summary>
        /// Takes a screenshot.
        /// </summary>
        /// <returns>Return the screenshot path and filename.</returns>
        public string TakeScreenshot()
        {
            // TODO: Study how to implement in a way to apply for other platforms like consoles.
            if (!System.IO.Directory.Exists(this.ScreenshotsPath))
            {
                System.IO.Directory.CreateDirectory(this.ScreenshotsPath);
            }

            string fileName = $"{this.ScreenshotsPath}{this.CreateFileName()}";

            ScreenCapture.CaptureScreenshot(fileName);

            print("ScreenShotter: Created Screenshot: " + fileName);

            return fileName;
        }
        #endregion
    }
}