using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneDrawer : ArgosPropertyDrawerBase
    {
        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.String;
        } 
        #endregion

        #region Events
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

            scene = (SceneAsset)EditorGUI.ObjectField(position, label, scene, typeof(SceneAsset), true);

            property.stringValue = AssetDatabase.GetAssetOrScenePath(scene);
        }
        #endregion
    }
}