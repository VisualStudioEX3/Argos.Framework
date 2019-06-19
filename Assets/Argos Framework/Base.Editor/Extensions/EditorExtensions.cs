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
        /// <param name="editor">The <see cref="Editor"/> instance.</param>
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
        /// Force to save all pending changes on this <see cref="Editor"/>/<see cref="EditorWindow"/>/<see cref="ScriptableObject"/>derived instance. Use with Serialized Objects.
        /// </summary>
        /// <param name="instance">This <see cref="Editor"/>/<see cref="EditorWindow"/>/<see cref="ScriptableObject"/> derived instance.</param>
        public static void SaveChangesOnAsset<T>(this T instance) where T : ScriptableObject
        {
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// Logs message to the Unity Console (identical to Debug.Log).
        /// </summary>
        /// <param name="instance">This <see cref="Editor"/>/<see cref="EditorWindow"/>/<see cref="ScriptableObject"/> derived instance.</param>
        /// <param name="message">Message to log.</param>
        /// <remarks>This is a copy of the <see cref="MonoBehaviour.print(object)"/> implementation of <see cref="MonoBehaviour"/> class.</remarks>
        public static void Print(this ScriptableObject instance, object message)
        {
            Debug.Log(message, instance);
        }

        /// <summary>
        /// Draws the Unity native component inspector.
        /// </summary>
        /// <param name="component"><see cref="UnityEngine.Object"/> instance.</param>
        public static void DrawNativeComponentInspector(this UnityEngine.Object component)
        {
            Editor.CreateEditor(component).OnInspectorGUI();
        }
        #endregion
    }
}
