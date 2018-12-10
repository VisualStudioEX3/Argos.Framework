using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}