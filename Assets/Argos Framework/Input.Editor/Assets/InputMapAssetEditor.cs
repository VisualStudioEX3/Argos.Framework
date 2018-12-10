using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Argos.Framework.Input
{
    [CustomEditor(typeof(InputMapAsset))]
    public class InputMapAssetEditor : ArgosCustomEditorBase
    {
        #region Internal vars
        private ReorderableList _axisList;
        private ReorderableList _actionList;
        #endregion

        #region Events
        private void OnEnable()
        {
            this._axisList = EditorHelper.CreateNamedList(this, this._axisList, "Axes", "_axes", "Axis Setup");
            this._actionList = EditorHelper.CreateNamedList(this, this._actionList, "Actions", "_actions", "Keys");
            this.HeaderTitle = "Input Map";
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            {
                this._axisList.DoLayoutList();
                EditorGUILayout.Space();

                this._actionList.DoLayoutList();
            }
            this.serializedObject.ApplyModifiedProperties();
        }
        #endregion
    } 
}
