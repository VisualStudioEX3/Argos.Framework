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
        public Color Color = Color.yellow;
        public TextAnchor Alignment = TextAnchor.UpperLeft;
        [Range(8, 96)]
        public int Size = 14;
        #endregion

        #region Events
        void Awake()
        {
            this.style = new GUIStyle();
        }

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        void OnGUI()
        {
            this.SetStyle();
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height),
                      string.Format(FPSDisplay.DISPLAY_FORMAT, deltaTime * 1000f, 1.0f / deltaTime),
                      this.SetStyle());
        } 
        #endregion

        #region Methods & Functions
        GUIStyle SetStyle()
        {
            if (this.style.alignment != this.Alignment)
            {
                this.style.alignment = this.Alignment;
            }

            if (this.style.fontSize != this.Size)
            {
                this.style.fontSize = this.Size;
            }

            if (this.style.normal.textColor != this.Color)
            {
                this.style.normal.textColor = this.Color;
            }

            return this.style;
        } 
        #endregion
    } 
}