using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#if UNITY_STANDALONE_WIN
namespace Argos.Framework.Input
{
    /// <summary>
    /// Microsoft XInput interface used for manage the XBox360 vibrators.
    /// </summary>
    public static class XInput
    {
        #region Constants
        const int ERROR_SUCCESS = 0;
        const ushort MAX_VIBRATION_LEVEL = 65000;
        const int PLAYER_ONE = 0;
        #endregion

        #region Structs
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct XInputVibration
        {
            public ushort LeftMotorSpeed;
            public ushort RightMotorSpeed;

            public XInputVibration(ushort left, ushort right)
            {
                LeftMotorSpeed = left;
                RightMotorSpeed = right;
            }
        }
        #endregion

        #region External DLL Imports
        [System.Runtime.InteropServices.DllImport("xinput9_1_0.dll", EntryPoint = "XInputSetState", CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        static extern int SetState(int dwUserIndex, ref XInputVibration pVibration);
        #endregion

        #region Vars
        static XInputVibration _vibratorsSetup = new XInputVibration();
        #endregion

        /// <summary>
        /// Set vibrators speed.
        /// </summary>
        /// <param name="left">Left engine.</param>
        /// <param name="right">Right engine.</param>
        /// <returns>Return true if a Xbox360 controller or compatible is conected and avaible as first player.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool SetVibration(float left, float right)
        {
            XInput._vibratorsSetup.LeftMotorSpeed = (ushort)(XInput.MAX_VIBRATION_LEVEL * left);
            XInput._vibratorsSetup.RightMotorSpeed = (ushort)(XInput.MAX_VIBRATION_LEVEL * right);

            return XInput.SetState(XInput.PLAYER_ONE, ref XInput._vibratorsSetup) == XInput.ERROR_SUCCESS;
        }
    } 
}
#endif
