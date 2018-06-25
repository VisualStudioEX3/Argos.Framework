using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Utils;

namespace Argos.Framework.Input
{
    public sealed class GamepadVibrationPool
    {
        #region Constants
        const int MAX_EFFECTS = 4;
        #endregion

        #region Structs
        struct VibrationData
        {
            #region Internal vars
            Timer _timer;
            Vector2 _force;
            #endregion

            #region Public vars
            public GamepadVibrationEffectAsset Effect;
            public bool IsBackgroundEffect;
            #endregion

            #region Properties
            public bool IsAvailable { get; private set; }

            public bool IsFinished
            {
                get
                {
                    return this.IsAvailable || this._timer.Value > this.Effect.Duration;
                }
            }
            #endregion

            #region Static members
            public static VibrationData Empty
            {
                get
                {
                    return new VibrationData()
                    {
                        IsAvailable = true
                    };
                }
            }

            public static Vector2 NoOverrideVibration
            {
                get
                {
                    return new Vector2(-1f, -1f);
                }
            }
            #endregion

            #region Constructor
            public VibrationData(GamepadVibrationEffectAsset effect, bool isBackgroundEffect)
            {
                this._force = VibrationData.NoOverrideVibration;
                this._timer = new Timer();

                this.IsAvailable = false;
                this.IsBackgroundEffect = isBackgroundEffect;
                this.Effect = effect;
            }
            #endregion

            #region Methods & Functions
            public Vector2 Evaluate()
            {
                return this.IsAvailable ?
                       VibrationData.NoOverrideVibration :
                       (this.Effect.UseCurves ? this.EvaluateCurve() : this.EvaluateConstant());
            }

            Vector2 EvaluateConstant()
            {
                if (this.IsFinished && !this.Effect.Loop)
                {
                    this.Stop();
                }
                else
                {
                    if (this.Effect.Loop || !this.IsFinished)
                    {
                        this._force.x = this.Effect.Type != GamepadVibrationEffectAsset.VibratorType.Weak ? this.Effect.StrongForce : (this.IsBackgroundEffect ? 0f : -1f);
                        this._force.y = this.Effect.Type != GamepadVibrationEffectAsset.VibratorType.Strong ? this.Effect.WeakForce : (this.IsBackgroundEffect ? 0f : -1f);
                    }
                }

                return this._force;
            }

            Vector2 EvaluateCurve()
            {
                if (this.IsFinished)
                {
                    if (this.Effect.Loop)
                    {
                        this._timer.Reset();
                    }
                    else
                    {
                        this.Stop();
                    }
                }

                this._force.x = this.Effect.Type != GamepadVibrationEffectAsset.VibratorType.Weak ? this.Effect.StrongCurve.Evaluate(this._timer.Value) : (this.IsBackgroundEffect ? 0f : -1f);
                this._force.y = this.Effect.Type != GamepadVibrationEffectAsset.VibratorType.Strong ? this.Effect.WeakCurve.Evaluate(this._timer.Value) : (this.IsBackgroundEffect ? 0f : -1f);

                return this._force;
            }

            public void Stop()
            {
                this.IsAvailable = true;
                InputManager.Instance.SetGamepadVibration(Vector2.zero);
            }
            #endregion
        }
        #endregion

        #region Internal vars
        VibrationData[] _effects;
        #endregion

        #region Propreties
        public bool Enable { get; set; }
        #endregion

        #region Constructor
        public GamepadVibrationPool()
        {
            this._effects = new VibrationData[GamepadVibrationPool.MAX_EFFECTS];

            for (int i = 0; i < GamepadVibrationPool.MAX_EFFECTS; i++)
            {
                this._effects[i] = VibrationData.Empty;
            }

            this.Enable = true;
        }
        #endregion

        #region Update logic
        public void Update()
        {
            if (this.Enable)
            {
                for (int i = 0; i < GamepadVibrationPool.MAX_EFFECTS; i++)
                {
                    InputManager.Instance.SetGamepadVibration(this._effects[i].Evaluate());
                }
            }
            else
            {
                InputManager.Instance.SetGamepadVibration(Vector2.zero);
            }
        }
        #endregion

        #region Methods & Functions
        public void PlayEffect(GamepadVibrationEffectAsset effect, bool isBackgroundEffect = false)
        {
            if (isBackgroundEffect)
            {
                this._effects[0] = new VibrationData(effect, true);
            }
            else
            {
                for (int i = 0; i < GamepadVibrationPool.MAX_EFFECTS; i++)
                {
                    if (this._effects[i].IsAvailable)
                    {
                        this._effects[i] = new VibrationData(effect, false);
                    }
                }
            }
        }

        public void StopEffect(GamepadVibrationEffectAsset effect)
        {
            for (int i = 0; i < GamepadVibrationPool.MAX_EFFECTS; i++)
            {
                if (this._effects[i].Effect == effect)
                {
                    this._effects[i].Stop();
                }
            }
        }

        public void StopAllEffects(bool includeBackgroundEffect = true)
        {
            for (int i = includeBackgroundEffect ? 0 : 1; i < GamepadVibrationPool.MAX_EFFECTS; i++)
            {
                this._effects[i].Stop();
            }
        }
        #endregion
    }
}
