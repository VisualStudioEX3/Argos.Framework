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
        public GenericGamepadInputLayout map;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Reset the map values to default.
        /// </summary>
        public void Reset()
        {
            this.map.button1 = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton0;
            this.map.button2 = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton1;
            this.map.button3 = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton2;
            this.map.button4 = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton3;
            this.map.start = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton4;
            this.map.select = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton5;
            this.map.leftBumper = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton6;
            this.map.rightBumper = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton7;
            this.map.leftStick = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton8;
            this.map.rightStick = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton9;
            this.map.leftTrigger = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton10;
            this.map.rightTrigger = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton11;
            this.map.DPadLeft = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton12;
            this.map.DPadRight = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton13;
            this.map.DPadUp = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton14;
            this.map.DPadDown = GenericGamepadInputLayout.UnityJoystickButtons.JoystickButton15;

            this.map.leftStickAxes = new Vector2Int(1, 2);
            this.map.rightStickAxes = new Vector2Int(3, 4);
            this.map.dPadAxes = new Vector2Int(5, 6);
            this.map.triggersAxes = new Vector2Int(7, 8);

            this.map.leftStickInvertY = this.map.rightStickInvertY = true;
            this.map.dPadInvertY = false;
        } 
        #endregion
    }
}