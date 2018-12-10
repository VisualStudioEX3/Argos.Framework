using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Input
{
    [CustomEditor(typeof(GenericGamepadInputLayoutAsset))]
    public class GenericGamepadInputLayoutAssetEditor : ArgosCustomEditorBase
    {
        #region Internal vars
        GenericGamepadInputLayoutAsset _target;
        SerializedProperty _map;
        #endregion

        #region Events
        private void OnEnable()
        {
            this._target = (GenericGamepadInputLayoutAsset)this.target;
            this._map = this.serializedObject.FindProperty("Map");
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

        #region Methods & Functions
        void CheckAxisRanges()
        {
            this._target.Map.LeftStickAxes = new Vector2Int(Mathf.Clamp(this._target.Map.LeftStickAxes.x, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX),
                                                            Mathf.Clamp(this._target.Map.LeftStickAxes.y, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX));

            this._target.Map.RightStickAxes = new Vector2Int(Mathf.Clamp(this._target.Map.RightStickAxes.x, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX),
                                                             Mathf.Clamp(this._target.Map.RightStickAxes.y, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX));

            this._target.Map.DPadAxes = new Vector2Int(Mathf.Clamp(this._target.Map.DPadAxes.x, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX),
                                                       Mathf.Clamp(this._target.Map.DPadAxes.y, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX));
        }
        #endregion
    } 
}