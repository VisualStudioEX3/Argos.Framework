using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework.Input
{
    [AddComponentMenu("Argos.Framework/Input/Gamepad Vibration Effect Controller")]
    public class GamepadVibrationEffectController : MonoBehaviour
    {
        #region Internal vars
        Timer _timer;
        YieldInstruction _wait;
        #endregion

        #region Public vars
        public GamepadVibrationEffectAsset effect;
        public bool fixedUpdate = false;
        public bool useUnScaledTime = false;
        public bool playOnStart = false;
        #endregion

        #region Properties
        public bool IsPlaying { get; private set; }
        #endregion

        #region Initializers
        private void Start()
        {
            this._timer = new Timer(this.useUnScaledTime ? TimerModes.UnScaledTime : TimerModes.ScaledTime, false);
            this._wait = this.fixedUpdate ? (YieldInstruction)new WaitForFixedUpdate() : (YieldInstruction)new WaitForEndOfFrame();

            if (this.playOnStart)
            {
                this.Play();
            }
        }

        private void OnDestroy()
        {
            this.OnDisable();
        }
        #endregion

        #region Methods
        public void Play()
        {
            if (this.IsPlaying)
            {
                StopAllCoroutines();
            }

            StartCoroutine(this.VibrationCoroutine());
        }

        public void Stop()
        {
            this.IsPlaying = false;
        }
        #endregion

        #region Event listener
        private void OnDisable()
        {
            StopAllCoroutines();
            InputManager.Instance.SetGamepadVibration(Vector2.zero);
        }
        #endregion

        #region Coroutines
        IEnumerator VibrationCoroutine()
        {
            float currentTime;
            float duration = this.effect.Duration;
            Vector2 intensity;

            this._timer.Start();
            this.IsPlaying = true;

            while (this.IsPlaying)
            {
                intensity = Vector2.zero;
                currentTime = this._timer.Value;

                if (currentTime >= duration)
                {
                    if (this.effect.Loop)
                    {
                        this._timer.Reset(true);
                    }
                    else
                    {
                        this.IsPlaying = false;
                    }
                }

                if (!this.effect.UseCurves)
                {
                    intensity = new Vector2(this.effect.Type != GamepadVibrationEffectAsset.VibratorType.Weak ? this.effect.StrongForce : -1,
                                            this.effect.Type != GamepadVibrationEffectAsset.VibratorType.Strong ? this.effect.WeakForce : -1);
                }
                else
                {
                    intensity = new Vector2(this.effect.Type != GamepadVibrationEffectAsset.VibratorType.Weak ? this.effect.StrongCurve.Evaluate(currentTime) : -1,
                                            this.effect.Type != GamepadVibrationEffectAsset.VibratorType.Strong ? this.effect.WeakCurve.Evaluate(currentTime) : -1);
                }

                InputManager.Instance.SetGamepadVibration(intensity);

                yield return this._wait;
            }

            InputManager.Instance.SetGamepadVibration(Vector2.zero);
            this._timer.Stop();
        } 
        #endregion
    }
}
