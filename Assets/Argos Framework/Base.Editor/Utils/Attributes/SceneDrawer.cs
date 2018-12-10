using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
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
}