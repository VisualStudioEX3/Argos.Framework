using UnityEngine;
using System.Collections;
using Argos.Framework;

namespace Argos.Framework.Utils.Debug
{
    [AddComponentMenu("Argos.Framework/Utils/Debug/Screen Shotter")]
    [ExecuteInEditMode]
    public class Screenshotter : MonoBehaviour
    {
        #region Private constants
        const string DEFAULT_NAME = "Screenshoot";
        #endregion

        #region Internal vars
        string _lastName;
        #endregion

        #region Public vars
        public KeyCode screenshotKey = KeyCode.F12;
        public string Name = DEFAULT_NAME;
        [ReadOnly]
        public string FinalName;
        #endregion

        #region Update logic
        void Update()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                UnityEngine.Debug.LogError("Screenshooter: The Name field never be an empty string.");
                this.Name = DEFAULT_NAME;
            }

            // Generate preview:
            if (this._lastName != this.Name)
            {
                this._lastName = this.Name;
                this.FinalName = string.Format("{0} {1}.png", this.Name, System.DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
            }

            if (UnityEngine.Input.GetKeyDown(this.screenshotKey))
            {
                string path = string.Format("{0}/Screen Shoots/", Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')));
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string fileName = string.Format("{0}{1} {2}.png", path, this.Name, System.DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
                ScreenCapture.CaptureScreenshot(fileName);
                print("Created Screenshot: " + fileName);
            }
        }
        #endregion
    }
}