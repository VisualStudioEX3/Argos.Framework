using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(SceneAsset))]
    public class SceneDrawer : ArgosPropertyDrawerBase
    {
        #region Internal vars
        SerializedProperty _sceneAssetReference;
        SerializedProperty _assetPath;
        SerializedProperty _scenePath;
        SerializedProperty _sceneIndex;

        UnityEditor.SceneAsset _sceneAssetEditor;

        string _guid;
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this._sceneAssetReference = property.FindPropertyRelative("_asset");
            this._assetPath = property.FindPropertyRelative("_assetPath");
            this._scenePath = property.FindPropertyRelative("_scenePath");
            this._sceneIndex = property.FindPropertyRelative("_sceneIndex");

            if (this._sceneAssetReference.objectReferenceValue && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this._sceneAssetReference.objectReferenceValue, out _guid, out long localID))
            {
                this._sceneAssetEditor = AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(AssetDatabase.GUIDToAssetPath(_guid));
            }

            Rect propertyField = position;
            if (property.IsArrayElement())
            {
                propertyField.height -= 2f;
            }

            this._sceneAssetEditor = (UnityEditor.SceneAsset)EditorGUI.ObjectField(propertyField, label, this._sceneAssetEditor, typeof(UnityEditor.SceneAsset), true);

            this._sceneAssetReference.objectReferenceValue = this._sceneAssetEditor ?? null;
            if (this._sceneAssetEditor)
            {
                this._assetPath.stringValue = AssetDatabase.GetAssetOrScenePath(this._sceneAssetEditor);
                this._scenePath.stringValue = this._assetPath.stringValue.Replace("Assets/", string.Empty).Replace(".unity", string.Empty);
                this._sceneIndex.intValue = SceneUtility.GetBuildIndexByScenePath(this._assetPath.stringValue);
            }
            else
            {
                this._assetPath.stringValue = this._scenePath.stringValue = string.Empty;
                this._sceneIndex.intValue = -1;
            }
        }
        #endregion
    }
}