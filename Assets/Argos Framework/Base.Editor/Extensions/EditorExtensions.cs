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

        /// <summary>
        /// Force to save all pending changes on this Editor/EditorWindow/SerializableObject derived instance. Use with Serialized Objects.
        /// </summary>
        /// <param name="instance">This Editor/EditorWindow/SerializableObject derived instance.</param>
        public static void SaveChangesOnAsset<T>(this T instance) where T : ScriptableObject
        {
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// Logs message to the Unity Console (identical to Debug.Log).
        /// </summary>
        /// <param name="instance">This Editor/EditorWindow/SerializableObject derived instance.</param>
        /// <param name="message">Message to log.</param>
        /// <remarks>This is a copy of the print() implementation of MonoBehaviour class.</remarks>
        public static void print<T>(this T instance, object message) where T : ScriptableObject
        {
            Debug.Log(message, instance);
        }
        #endregion
    }
}
