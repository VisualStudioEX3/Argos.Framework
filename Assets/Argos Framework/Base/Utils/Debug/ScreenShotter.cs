using UnityEngine;
using System.Collections;
using Argos.Framework;

namespace Argos.Framework.Utils.Debug
{
    [AddComponentMenu("Argos.Framework/Utils/Debug/Screen Shotter"), DisallowMultipleComponent, ExecuteInEditMode]
    public sealed class ScreenShotter : MonoBehaviour
    {
        #region Private constants
        const string DEFAULT_NAME = "Screenshot";
        #endregion

        #region Internal vars
        string _lastName;
#pragma warning disable 414
        [SerializeField, DinamicLabel(true, true)]
        string _finalName;
#pragma warning restore
        #endregion

        #region Public vars
        public KeyCode screenshotKey = KeyCode.F12;
        public string Name = DEFAULT_NAME;
        
        #endregion

        #region Update logic
        void Update()
        {
            if (Helpers.ArgosSupportedPlatforms.Desktop.HasFlag(Helpers.ArgosHelper.CurrentPlatform))
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    UnityEngine.Debug.LogError("ScreenShotter: The Name field never be an empty string.");
                    this.Name = DEFAULT_NAME;
                }

                // Generate preview:
                if (this._lastName != this.Name)
                {
                    this._lastName = this.Name;
                    this._finalName = string.Format("{0} {1}.png", this.Name, System.DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
                }

                if (UnityEngine.Input.GetKeyDown(this.screenshotKey))
                {
                    string path = string.Format("{0}/Screenshots/", Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')));
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    string fileName = string.Format("{0}{1} {2}.png", path, this.Name, System.DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
                    ScreenCapture.CaptureScreenshot(fileName);
                    print("Created Screenshot: " + fileName);
                } 
            }
        }
        #endregion
    }
}