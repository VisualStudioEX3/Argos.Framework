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
    /// Gamepad base map for generic gamepads.
    /// </summary>
    [CreateAssetMenu(fileName = "New Gamepad Base Map", menuName = "Argos.Framework/Input/Gamepad Base Map")]
    public class GamepadBaseMapAsset : ScriptableObject
    {
        #region Public vars
        /// <summary>
        /// Map values for generic gamepad.
        /// </summary>
        [SerializeField]
        public GamepadBaseMap Map;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Reset the map values to default.
        /// </summary>
        public void Reset()
        {
            this.Map.Button1 = GamepadBaseMap.UnityJoystickButtons.JoystickButton0;
            this.Map.Button2 = GamepadBaseMap.UnityJoystickButtons.JoystickButton1;
            this.Map.Button3 = GamepadBaseMap.UnityJoystickButtons.JoystickButton2;
            this.Map.Button4 = GamepadBaseMap.UnityJoystickButtons.JoystickButton3;
            this.Map.Start = GamepadBaseMap.UnityJoystickButtons.JoystickButton4;
            this.Map.Select = GamepadBaseMap.UnityJoystickButtons.JoystickButton5;
            this.Map.LeftBumper = GamepadBaseMap.UnityJoystickButtons.JoystickButton6;
            this.Map.RightBumper = GamepadBaseMap.UnityJoystickButtons.JoystickButton7;
            this.Map.LeftStick = GamepadBaseMap.UnityJoystickButtons.JoystickButton8;
            this.Map.RightStick = GamepadBaseMap.UnityJoystickButtons.JoystickButton9;
            this.Map.LeftTrigger = GamepadBaseMap.UnityJoystickButtons.JoystickButton10;
            this.Map.RightTrigger = GamepadBaseMap.UnityJoystickButtons.JoystickButton11;
            this.Map.DPadLeft = GamepadBaseMap.UnityJoystickButtons.JoystickButton12;
            this.Map.DPadRight = GamepadBaseMap.UnityJoystickButtons.JoystickButton13;
            this.Map.DPadUp = GamepadBaseMap.UnityJoystickButtons.JoystickButton14;
            this.Map.DPadDown = GamepadBaseMap.UnityJoystickButtons.JoystickButton15;

            this.Map.LeftStickAxes = new Vector2Int(1, 2);
            this.Map.RightStickAxes = new Vector2Int(3, 4);
            this.Map.DPadAxes = new Vector2Int(5, 6);
            this.Map.TriggersAxes = new Vector2Int(7, 8);

            this.Map.LeftStickInvertY = this.Map.RightStickInvertY = true;
            this.Map.DPadInvertY = false;
        } 
        #endregion

        #region Events
        private void OnEnable()
        {
            this.Reset();
        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GamepadBaseMapAsset))]
    public class GamepadBaseMapAssetEditor : Editor
    {
        GamepadBaseMapAsset _target;

        #region Events
        private void OnEnable()
        {
            this._target = (GamepadBaseMapAsset)this.target;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.HelpBox("Use this asset to setup axis and button map for any generic gamepad.", MessageType.Info);
            EditorGUILayout.HelpBox("This values not affect to XBox, PS4 or Nintendo Switch Pro controller map setup.", MessageType.Warning);

            var serializedProperty = this.serializedObject.FindProperty("Map");
            serializedProperty.isExpanded = true;
            EditorGUILayout.PropertyField(serializedProperty, true);

            this.CheckAxisRanges();

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