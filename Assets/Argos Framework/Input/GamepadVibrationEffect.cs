using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Argos.Framework;

namespace Argos.Framework.Input
{
    public class GamepadVibrationEffect : MonoBehaviour
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
            this._timer = new Timer(this.UseUnScaledTime ? Timer.TimerMode.UnScaledTime : Timer.TimerMode.ScaledTime);
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
            Vector2 intensity;

            this._timer.Reset();
            this.IsPlaying = true;

            while (this.IsPlaying)
            {
                currentTime = _timer.Value;

                if (currentTime >= this.Effect.Duration)
                {
                    if (this.Effect.Loop)
                    {
                        _timer.Reset(); 
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
        } 
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GamepadVibrationEffect))]
    public class GamepadVibrationEffectEditor : Editor
    {
        #region Internal vars
        GamepadVibrationEffect _target;
        SerializedProperty _effect, _fixedUpdate, _useUnScaledTime, _playOnStart;
        #endregion

        #region Events
        private void OnEnable()
        {
            this._target = (GamepadVibrationEffect)this.target;

            this._effect = this.serializedObject.FindProperty("Effect");
            this._fixedUpdate = this.serializedObject.FindProperty("FixedUpdate");
            this._useUnScaledTime = this.serializedObject.FindProperty("UseUnScaledTime");
            this._playOnStart = this.serializedObject.FindProperty("PlayOnStart");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            {
                EditorGUILayout.PropertyField(this._effect);
                EditorGUILayout.PropertyField(this._fixedUpdate);
                this._useUnScaledTime.boolValue = EditorGUILayout.Popup("Update mode", this._useUnScaledTime.boolValue ? 1 : 0, new string[] { "Normal", "Unscaled Time" }) == 0 ? false : true;
                EditorGUILayout.PropertyField(this._playOnStart);

                if (this._target.Effect)
                {
                    var effectInfo = new StringBuilder();
                    effectInfo.AppendFormat("Type: {0} ({1}) | ", this._target.Effect.Type,
                                                               (this._target.Effect.Type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                               "Left & Right engines" :
                                                               this._target.Effect.Type == GamepadVibrationEffectAsset.VibratorType.Strong ?
                                                                   "Left engine" :
                                                                   "Right engine"));

                    effectInfo.AppendFormat("Use curves: {0} | ", this._target.Effect.UseCurves ? "Yes" : "No");
                    effectInfo.AppendFormat("Is looped: {0} {1}", this._target.Effect.Loop ? "Yes" : "No", !this._target.Effect.Loop ? "| " : string.Empty);
                    if (!this._target.Effect.Loop)
                    {
                        effectInfo.AppendFormat("Duration: {0:0.00} {1}", this._target.Effect.Duration, this._target.Effect.Duration == 1f ? "second" : "seconds");
                    }

                    EditorGUILayout.HelpBox(effectInfo.ToString(), MessageType.Info);
                }
            }
            this.serializedObject.ApplyModifiedProperties();
        } 
        #endregion
    }
#endif
}
