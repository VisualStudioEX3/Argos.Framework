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

        ReorderableDictionary _axisList;
        ReorderableDictionary _actionList;
        #endregion

        #region Event listeners
        private void OnEnable()
        {
            this._axes = this.serializedObject.FindProperty("_axes._elements");
            this._actions = this.serializedObject.FindProperty("_actions._elements");

            this._axisList = new ReorderableDictionary(this._axes, "Axes", false, true, true, true, "New axis name");
            this._actionList = new ReorderableDictionary(this._actions, "Actions", false, true, true, true, "New action name");

            this.HeaderTitle = "Input Map";
        }

        private void OnDisable()
        {
            this._axisList?.Dispose();
            this._actionList?.Dispose();
        }

        protected override bool ShouldHideOpenButton()
        {
            return true;
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
