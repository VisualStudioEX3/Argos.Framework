using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a string variable in a script be a serializable SceneAsset value (saved as asset path).
    /// </summary>
    public class SceneAttribute : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneDrawer : PropertyDrawer
    {
        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

            scene = (SceneAsset)EditorGUI.ObjectField(position, label, scene, typeof(SceneAsset), true);

            property.stringValue = AssetDatabase.GetAssetOrScenePath(scene);
        } 
        #endregion
    } 
#endif
}