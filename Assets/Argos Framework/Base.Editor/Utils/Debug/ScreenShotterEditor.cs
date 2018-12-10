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
        #region Events
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspectorWithoutScriptField();
        } 
        #endregion
    }
}
