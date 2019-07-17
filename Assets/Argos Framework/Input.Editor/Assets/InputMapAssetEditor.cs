using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework.IMGUI;

namespace Argos.Framework.Input
{
    [CustomEditor(typeof(InputMapAsset))]
    public class InputMapAssetEditor : ArgosCustomEditorBase
    {
        #region Internal vars
        SerializedProperty _axes;
        SerializedProperty _actions;

        InputAxisDictionaryControl _axisList;
        ReorderableDictionary _actionList;
        #endregion

        #region Event listeners
        private void OnEnable()
        {
            this._axes = this.serializedObject.FindProperty("_axes._elements");
            this._actions = this.serializedObject.FindProperty("_actions._elements");

            this._axisList = new InputAxisDictionaryControl(this._axes);
            this._actionList = new ReorderableDictionary(this._actions, "Actions");

            this.HeaderTitle = "Input Map";
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            {
                EditorGUILayout.Space();
                this._axisList.DoLayoutList();

                EditorGUILayout.Space();
                this._actionList.DoLayoutList();
            }
            this.serializedObject.ApplyModifiedProperties();
        }
        #endregion
    } 
}
