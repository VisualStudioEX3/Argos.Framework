using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

namespace Argos.Framework.Helpers
{
    #region Enums
    /// <summary>
    /// Enumeration of platforms supported by Argos.Framework.
    /// </summary>
    [Flags]
    public enum ArgosSupportedPlatforms
    {
        Windows = 0,
        Linux = 1,
        OSX = 2,
        Desktop = Windows | Linux | OSX,

        UWPDesktop = 4,
        UWPXBoxOne = 8,
        UniversalWindowsPlatform = UWPDesktop | UWPXBoxOne,

        XBoxOne = 16,
        PS4 = 32,
        NintendoSwitch = 64,
        Console = XBoxOne | PS4 | NintendoSwitch
    }
    #endregion

    /// <summary>
    /// General helper class.
    /// </summary>
    public static class ArgosHelper
    {
        #region Properties
        /// <summary>
        /// Return the current supported platform.
        /// </summary>
        public static ArgosSupportedPlatforms CurrentPlatform
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:

                        return ArgosSupportedPlatforms.OSX;

                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.WindowsEditor:

                        return ArgosSupportedPlatforms.Windows;

                    case RuntimePlatform.LinuxPlayer:
                    case RuntimePlatform.LinuxEditor:

                        return ArgosSupportedPlatforms.Linux;

                    case RuntimePlatform.WSAPlayerX86:
                    case RuntimePlatform.WSAPlayerARM:

                        return ArgosSupportedPlatforms.UWPDesktop;

                    case RuntimePlatform.WSAPlayerX64:

                        switch (SystemInfo.deviceType)
                        {
                            case DeviceType.Console:

                                return ArgosSupportedPlatforms.UWPXBoxOne;

                            case DeviceType.Desktop:

                                return ArgosSupportedPlatforms.UWPDesktop;

                            default:

                                throw new NotImplementedException($"Platform not supported. Platform: {Application.platform}, Device type: {SystemInfo.deviceType}");
                        }

                    case RuntimePlatform.PS4:

                        return ArgosSupportedPlatforms.PS4;

                    case RuntimePlatform.XboxOne:

                        return ArgosSupportedPlatforms.XBoxOne;

                    case RuntimePlatform.Switch:

                        return ArgosSupportedPlatforms.NintendoSwitch;

                    default:

                        throw new NotImplementedException($"Platform not supported. Platform: {Application.platform}, Device type: {SystemInfo.deviceType}");
                }
            }
        } 
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Swap variable values.
        /// </summary>
        /// <typeparam name="T">Type of variables.</typeparam>
        /// <param name="a">First var.</param>
        /// <param name="b">Second var.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a; a = b; b = c;
        }

        /// <summary>
        /// Created a safe random seed for intializing the System.Random class.
        /// </summary>
        /// <returns>Return a random seed.</returns>
        /// <remarks>This functions calculated the seed using the System.Security.Cryptography.RandomNumberGenerator.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GenerateSafeRandomSeed()
        {
            var cryptoResult = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(cryptoResult);
            return BitConverter.ToInt32(cryptoResult, 0);
        }

        /// <summary>
        /// Generate a Int32 value based on a GUID value.
        /// </summary>
        /// <returns>Return a Int32 GUID value.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GenerateInt32GuidValue()
        {
            return BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
        }

        /// <summary>
        /// Set the ragdoll pose based on the character current pose.
        /// </summary>
        /// <param name="ragdoll">Ragdoll based on character.</param>
        /// <param name="character">Character to copy the pose.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetRagdollPose(Transform ragdoll, Transform character)
        {
            ragdoll.position = character.position;
            ragdoll.rotation = character.rotation;

            for (int i = 0; i < ragdoll.childCount; i++)
            {
                ArgosHelper.SetRagdollPose(ragdoll.GetChild(i), character.GetChild(i));
            }
        }

        /// <summary>
        /// Cleanup memory and unused assets.
        /// </summary>
        /// <param name="discardGCCollect">Discard System.GC.Collect() call during the cleanup process.</param>
        /// <returns>Return an AsyncOperation for controlling the wait period during the cleanup process.</returns>
        /// <remarks>This function is only a shortcut to call an System.GC.Collect() and UnityEngine.Resources.UnloadUnussedAssets() functions.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static AsyncOperation CleanUpMemoryAndAssets(bool discardGCCollect = false)
        {
            if (!discardGCCollect)
            {
                System.GC.Collect();
            }

            return Resources.UnloadUnusedAssets();
        }
        #endregion
    }
}
