using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 
#endif
using Argos.Framework;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Gamepad input layout for generic gamepads.
    /// </summary>
    [CreateAssetMenu(fileName = "New Generic Gamepad Input Layout", menuName = "Argos.Framework/Input/Generic Gamepad Input Layout")]
    public class GenericGamepadInputLayoutAsset : ScriptableObject
    {
        #region Public vars
        /// <summary>
        /// Map values for generic gamepad.
        /// </summary>
        [SerializeField]
        public GenericGamepadInputLayout Map;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Reset the map values to default.
        /// </summary>
        public void Reset()
        {
            this.Map.Button1 = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton0;
            this.Map.Button2 = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton1;
            this.Map.Button3 = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton2;
            this.Map.Button4 = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton3;
            this.Map.Start = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton4;
            this.Map.Select = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton5;
            this.Map.LeftBumper = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton6;
            this.Map.RightBumper = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton7;
            this.Map.LeftStick = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton8;
            this.Map.RightStick = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton9;
            this.Map.LeftTrigger = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton10;
            this.Map.RightTrigger = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton11;
            this.Map.DPadLeft = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton12;
            this.Map.DPadRight = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton13;
            this.Map.DPadUp = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton14;
            this.Map.DPadDown = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton15;

            this.Map.LeftStickAxes = new Vector2Int(1, 2);
            this.Map.RightStickAxes = new Vector2Int(3, 4);
            this.Map.DPadAxes = new Vector2Int(5, 6);
            this.Map.TriggersAxes = new Vector2Int(7, 8);

            this.Map.LeftStickInvertY = this.Map.RightStickInvertY = true;
            this.Map.DPadInvertY = false;
        } 
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GenericGamepadInputLayoutAsset))]
    public class GenericGamepadInputLayoutAssetEditor : Editor
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
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Generic Gamepad Layout", EditorStyles.centeredGreyMiniLabel);

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
#endif
}