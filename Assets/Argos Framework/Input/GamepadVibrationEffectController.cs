using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Argos.Framework.Utils;

namespace Argos.Framework.Input
{
    public class GamepadVibrationEffectController : MonoBehaviour
    {
        #region Internal vars
        Timer _timer;
        YieldInstruction _wait;
        #endregion

        #region Public vars
        public GamepadVibrationEffectAsset Effect;
        public bool FixedUpdate = false;
        public bool UseUnScaledTime = false;
        public bool PlayOnStart = false;
        #endregion

        #region Properties
        public bool IsPlaying { get; private set; }
        #endregion

        #region Initializers
        private void Start()
        {
            this._timer = new Timer(this.UseUnScaledTime ? TimerModes.UnScaledTime : TimerModes.ScaledTime);
            this._wait = this.FixedUpdate ? (YieldInstruction)new WaitForFixedUpdate() : (YieldInstruction)new WaitForEndOfFrame();

            if (this.PlayOnStart)
            {
                this.Play();
            }
        } 
        #endregion

        #region Events
        private void OnDisable()
        {
            StopAllCoroutines();
            InputManager.Instance.SetGamepadVibration(Vector2.zero);
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

        #region Coroutines
        IEnumerator VibrationCoroutine()
        {
            float currentTime;
            float duration = this.Effect.Duration;
            Vector2 intensity;

            this._timer.Start();
            this.IsPlaying = true;

            while (this.IsPlaying)
            {
                intensity = Vector2.zero;
                currentTime = _timer.Value;

                if (currentTime >= duration)
                {
                    if (this.Effect.Loop)
                    {
                        this._timer.Reset(true);
                    }
                    else
                    {
                        this.IsPlaying = false;
                    }
                }

                if (!this.Effect.UseCurves)
                {
                    intensity = new Vector2(this.Effect.Type != GamepadVibrationEffectAsset.VibratorType.Weak ? this.Effect.StrongForce : -1,
                                            this.Effect.Type != GamepadVibrationEffectAsset.VibratorType.Strong ? this.Effect.WeakForce : -1);
                }
                else
                {
                    intensity = new Vector2(this.Effect.Type != GamepadVibrationEffectAsset.VibratorType.Weak ? this.Effect.StrongCurve.Evaluate(currentTime) : -1,
                                            this.Effect.Type != GamepadVibrationEffectAsset.VibratorType.Strong ? this.Effect.WeakCurve.Evaluate(currentTime) : -1);
                }

                InputManager.Instance.SetGamepadVibration(intensity);

                yield return _wait;
            }

            InputManager.Instance.SetGamepadVibration(Vector2.zero);
            this._timer.Stop();
        } 
        #endregion
    }
}
