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

            this.Map.LeftStickAxes = new Vector2(1f, 2f);
            this.Map.RightStickAxes = new Vector2(3f, 4f);
            this.Map.DPadAxes = new Vector2(5f, 6f);
            this.Map.TriggersAxes = new Vector2(7f, 8f);

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
        #region Events
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.HelpBox("This is the base map for use in gamepad input operations.\nUse this asset to define the axis and button map for any generic gamepad.\nThis values not affect to XBox, PS4 or Nintendo Switch controller maps.", MessageType.Info);
            EditorGUILayout.HelpBox("You can restore default values using Reset command on the asset context menu or from script.", MessageType.Warning);

            var serializedProperty = this.serializedObject.FindProperty("Map");
            serializedProperty.isExpanded = true;
            EditorGUILayout.PropertyField(serializedProperty, true);

            this.serializedObject.ApplyModifiedProperties();
        } 
        #endregion
    } 
#endif
}