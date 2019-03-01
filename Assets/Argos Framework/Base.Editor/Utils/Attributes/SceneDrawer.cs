using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(SceneAsset))]
    public class SceneDrawer : ArgosPropertyDrawerBase
    {
        #region Internal vars
        SerializedProperty _sceneAssetReference;
        SerializedProperty _scenePath;

        UnityEditor.SceneAsset _sceneAssetEditor;

        string _guid;
        long _localID;
        #endregion

        #region Events
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this._sceneAssetReference = property.FindPropertyRelative("_asset");
            this._scenePath = property.FindPropertyRelative("_path");

            if (this._sceneAssetReference.objectReferenceValue && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this._sceneAssetReference.objectReferenceValue, out _guid, out _localID))
            {
                this._sceneAssetEditor = AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(AssetDatabase.GUIDToAssetPath(_guid));
            }

            this._sceneAssetEditor = (UnityEditor.SceneAsset)EditorGUI.ObjectField(position, label, this._sceneAssetEditor, typeof(UnityEditor.SceneAsset), true);

            this._sceneAssetReference.objectReferenceValue = this._sceneAssetEditor ? this._sceneAssetEditor : null;
            this._scenePath.stringValue = this._sceneAssetEditor ? AssetDatabase.GetAssetOrScenePath(this._sceneAssetEditor) : string.Empty;
        }
        #endregion
    }
}