using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// Method extensions for Editor scripts.
    /// </summary>
    public static class EditorExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Draw the default inspector without render the Script field.
        /// </summary>
        /// <param name="editor">The Editor instance.</param>
        public static void DrawDefaultInspectorWithoutScriptField(this Editor editor)
        {
            editor.serializedObject.Update();

            var ite = editor.serializedObject.GetIterator();
            ite.NextVisible(true);

            EditorGUI.BeginChangeCheck();

            while (ite.NextVisible(false))
            {
                EditorGUILayout.PropertyField(ite, true);
            }

            if (EditorGUI.EndChangeCheck())
            {
                editor.serializedObject.ApplyModifiedProperties();
            }
        }
        #endregion
    }
}
