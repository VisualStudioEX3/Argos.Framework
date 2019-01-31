using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework.Input.Extensions;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Gamepad Vibration Effect.
    /// </summary>
    /// <remarks>Create a vibration/rumble effect for compatible gamepads (XBox controllers and joysticks with ForceFeedback support) wich defines force type (strong, weak or both), duration and constant intensity or complex patterns using curves.
    /// 
    /// The gamepad vibration effects only supported on Windows (desktop and UWP apps) and XBox One systems.
    /// ForceFeedback only supported on Windows desktop apps.</remarks>
    [CreateAssetMenu(fileName = "New Gamepad Vibration Effect", menuName = "Argos.Framework/Input/Gamepad Vibration Effect")]
    public sealed class GamepadVibrationEffectAsset : ScriptableObject
    {
        #region Enums
        public enum VibratorType
        {
            /// <summary>
            /// Strong engine (Left engine).
            /// </summary>
            Strong,
            /// <summary>
            /// Weak engine (Right engine).
            /// </summary>
            Weak,
            /// <summary>
            /// Both (Left + Right engines).
            /// </summary>
            Both
        }
        #endregion

        #region Inspector fields
#pragma warning disable 0649
        [SerializeField]
        VibratorType _type;
        [SerializeField]
        bool _useCurves;
        [SerializeField]
        bool _loop;

        [SerializeField]
        [Range(0f, 1f)]
        float _strongForce;
        [SerializeField]
        [Range(0f, 1f)]
        float _weakForce;

        [SerializeField]
        float _duration;

        [SerializeField]
        AnimationCurve _strongCurve = AnimationCurve.Constant(0f, 1f, 1f);
        [SerializeField]
        AnimationCurve _weakCurve = AnimationCurve.Constant(0f, 1f, 1f);
#pragma warning restore
        #endregion

        #region Properties
        /// <summary>
        /// Vibrator type: Strong = left engine, Weak = right engine, Both = Strong + Weak.
        /// </summary>
        public VibratorType Type { get { return this._type; } }

        /// <summary>
        /// This effect use pattern curves?
        /// </summary>
        public bool UseCurves { get { return this._useCurves; } }

        /// <summary>
        /// This effect is looped?
        /// </summary>
        public bool Loop { get { return this._loop; } }

        /// <summary>
        /// Strong constant force.
        /// </summary>
        public float StrongForce { get { return this._strongForce; } }

        /// <summary>
        /// Weak constant force.
        /// </summary>
        public float WeakForce { get { return this._weakForce; } }

        /// <summary>
        /// Effect duration.
        /// </summary>
        /// <remarks>When this effect use curves, return the curve time, or the longest curve time if use both of them. In other case, return the effect Duration value.</remarks>
        public float Duration
        {
            get
            {
                if (this._useCurves)
                {
                    switch (this.Type)
                    {
                        case VibratorType.Strong:

                            return this._strongCurve.keys[this._strongCurve.length - 1].time;

                        case VibratorType.Weak:

                            return this._weakCurve.keys[this._weakCurve.length - 1].time;

                        default:

                            return Mathf.Max(this._strongCurve.keys[this._strongCurve.length - 1].time,
                                             this._weakCurve.keys[this._weakCurve.length - 1].time);
                    }
                }
                else
                {
                    return this._duration;
                }
            }
        }

        /// <summary>
        /// Strong pattern curve.
        /// </summary>
        public AnimationCurve StrongCurve { get { return this._strongCurve; } }

        /// <summary>
        /// Weak pattern curve.
        /// </summary>
        public AnimationCurve WeakCurve { get { return this._weakCurve; } }
        #endregion
    }
}