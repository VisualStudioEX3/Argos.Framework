using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Utils
{
    public static class EditorSkinUtility
    {
        #region Properties
        /// <summary>
        /// Returns the current Unity editor skin.
        /// </summary>
        public static GUISkin Skin
        {
            get
            {
                return EditorGUIUtility.GetBuiltinSkin(EditorGUIUtility.isProSkin ? EditorSkin.Scene : EditorSkin.Inspector);
            }
        } 
        #endregion
    } 
}
