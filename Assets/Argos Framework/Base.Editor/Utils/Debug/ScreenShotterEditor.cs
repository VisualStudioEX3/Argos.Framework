using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Argos.Framework;

namespace Argos.Framework.Utils.Debug
{
    [CustomEditor(typeof(ScreenShotter))]
    public sealed class ScreenShotterEditor : Editor
    {
        #region Event listeners
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspectorWithoutScriptField();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Take Screenshot"))
                {
                    (this.target as ScreenShotter).TakeScreenshot();
                }

                if (GUILayout.Button("Open Screenshot Folder Location"))
                {
                    EditorUtility.RevealInFinder((this.target as ScreenShotter).ScreenshotsPath);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}
