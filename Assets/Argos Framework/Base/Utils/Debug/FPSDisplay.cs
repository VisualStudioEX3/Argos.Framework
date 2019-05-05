using UnityEngine;
using System.Collections;

namespace Argos.Framework.Utils.Debug
{
    /// <summary>
    /// FPS display counter.
    /// </summary>
    /// <remarks>Based on this source: http://wiki.unity3d.com/index.php/FramesPerSecond </remarks>
    [AddComponentMenu("Argos.Framework/Utils/Debug/FPS Display"), DisallowMultipleComponent, ExecuteInEditMode]
    public sealed class FPSDisplay : MonoBehaviour
    {
        #region Constants
        const string DISPLAY_FORMAT = " ({0:0.0} ms.) {1:0.} FPS ";
        #endregion

        #region Internal vars
        float deltaTime;
        GUIStyle style;
        #endregion

        #region Public vars
        public Color color = Color.yellow;
        public TextAnchor alignment = TextAnchor.UpperLeft;
        [Range(8, 96)]
        public int size = 14;
        #endregion

        #region Methods & Functions
        GUIStyle SetStyle()
        {
            if (this.style.alignment != this.alignment)
            {
                this.style.alignment = this.alignment;
            }

            if (this.style.fontSize != this.size)
            {
                this.style.fontSize = this.size;
            }

            if (this.style.normal.textColor != this.color)
            {
                this.style.normal.textColor = this.color;
            }

            return this.style;
        }
        #endregion

        #region Initializers
        void Awake()
        {
            this.style = new GUIStyle();
        } 
        #endregion

        #region Update logic
        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        } 
        #endregion

        #region Event listeners
        void OnGUI()
        {
            this.SetStyle();
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height),
                      string.Format(FPSDisplay.DISPLAY_FORMAT, deltaTime * 1000f, 1.0f / deltaTime),
                      this.SetStyle());
        }
        #endregion
    }
}