using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework.Utils;

namespace Argos.Framework.Input
{
    [CustomEditor(typeof(GenericGamepadInputLayoutAsset))]
    public class GenericGamepadInputLayoutAssetEditor : ArgosCustomEditorBase
    {
        #region Constants
        readonly static Vector2Int MIN_AXIS_INDEX = Vector2Int.one * Gamepad.MIN_AXIS_INDEX;
        readonly static Vector2Int MAX_AXIS_INDEX = Vector2Int.one * Gamepad.MAX_AXIS_INDEX; 
        #endregion

        #region Internal vars
        GenericGamepadInputLayoutAsset _target;
        SerializedProperty _map;
        #endregion

        #region Methods & Functions
        void CheckAxisRanges()
        {
            this.ClampAxis(ref this._target.map.leftStickAxes);
            this.ClampAxis(ref this._target.map.rightStickAxes);
            this.ClampAxis(ref this._target.map.dPadAxes);
        }

        void ClampAxis(ref Vector2Int axis)
        {
            axis = VectorsUtility.Clamp(axis, GenericGamepadInputLayoutAssetEditor.MIN_AXIS_INDEX, GenericGamepadInputLayoutAssetEditor.MAX_AXIS_INDEX);
        }
        #endregion

        #region Event listeners
        private void OnEnable()
        {
            this._target = (GenericGamepadInputLayoutAsset)this.target;
            this._map = this.serializedObject.FindProperty("map");
            this.HeaderTitle = "Generic Gamepad Layout";
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            {
                EditorGUILayout.HelpBox("Use this asset to setup axis and button layout for any generic gamepad. This setup is used by Input Manager to tell to Input Map assets what are they the right axes and buttons they be read.", MessageType.Info);
                EditorGUILayout.HelpBox("This values not affect to XBox, PS4 or Nintendo Switch Pro controller map setup.", MessageType.Warning);

                this._map.isExpanded = true;
                EditorGUILayout.PropertyField(this._map, true);

                this.CheckAxisRanges();
            }
            this.serializedObject.ApplyModifiedProperties();
        }
        #endregion
    } 
}