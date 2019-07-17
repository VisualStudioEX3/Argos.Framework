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

        #region Methods & Functions
        void CheckAxisRanges()
        {
            this._target.map.leftStickAxes = new Vector2Int(Mathf.Clamp(this._target.map.leftStickAxes.x, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX),
                                                            Mathf.Clamp(this._target.map.leftStickAxes.y, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX));

            this._target.map.rightStickAxes = new Vector2Int(Mathf.Clamp(this._target.map.rightStickAxes.x, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX),
                                                             Mathf.Clamp(this._target.map.rightStickAxes.y, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX));

            this._target.map.DPadAxes = new Vector2Int(Mathf.Clamp(this._target.map.DPadAxes.x, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX),
                                                       Mathf.Clamp(this._target.map.DPadAxes.y, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX));
        }
        #endregion

        #region Event listeners
        private void OnEnable()
        {
            this._target = (GenericGamepadInputLayoutAsset)this.target;
            this._map = this.serializedObject.FindProperty("map");
            this.HeaderTitle = "Generic Gamepad Layout";
        }

        void OnDisable()
        {
            AssetDatabase.ForceReserializeAssets(new string[] { AssetDatabase.GetAssetOrScenePath(this.target) }, ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);
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