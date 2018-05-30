using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    #region Enums
    /// <summary>
    /// Enumeration of platforms supported by Argos.Framework.
    /// </summary>
    [Flags]
    public enum ArgosSupportedPlatforms
    {
        Windows = 1,
        Linux = 2,
        OSX = 4,
        Desktop = Windows | Linux | OSX,

        UWPDesktop = 8,
        UWPXBoxOne = 16,
        UniversalWindowsPlatform = UWPDesktop | UWPXBoxOne,

        XBoxOne = 32,
        PS4 = 64,
        NintendoSwitch = 128,
        Console = XBoxOne | PS4 | NintendoSwitch
    } 
    #endregion

    public static class PlatformHelper
    {
        /// <summary>
        /// Return the current platform.
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

                        throw new NotImplementedException($"Platform not supported: {Application.platform}");
                }
            }
        }
    }

}