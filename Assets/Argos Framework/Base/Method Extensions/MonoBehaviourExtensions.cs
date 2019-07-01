using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// MonoBehaviour method extensions.
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Get the name of this class.
        /// </summary>
        /// <param name="instance"><see cref="MonoBehaviour"/> instance.</param>
        /// <returns>Return the name of this class.</returns>
        /// <remarks>This function is useful for use with log messages.</remarks>
        public static string GetClassName(this MonoBehaviour instance)
        {
            return instance.GetType().Name;
        }

        /// <summary>
        /// Take screenshot at the end of the current frame.
        /// </summary>
        /// <param name="instance"><see cref="MonoBehaviour"/> instance.</param>
        /// <param name="onEndOfFrame">Event listener that receive the <see cref="Texture2D"/> result of the screenshot at the end of the current frame.</param>
        /// <remarks>This method ease the task to take a screenshot just when the current frame is finished to render entirely and store in a variable.</remarks>
        public static void TakeScreenshot(this MonoBehaviour instance, Action<Texture2D> onEndOfFrame)
        {
            if (onEndOfFrame != null)
            {
                instance.StartCoroutine(MonoBehaviourExtensions.TakeScreenshotCoroutine(onEndOfFrame)); 
            }
        }

        static IEnumerator TakeScreenshotCoroutine(Action<Texture2D> onEndOfFrame)
        {
            yield return new WaitForEndOfFrame();
            onEndOfFrame.Invoke(ScreenCapture.CaptureScreenshotAsTexture());
        } 
        #endregion
    }
}