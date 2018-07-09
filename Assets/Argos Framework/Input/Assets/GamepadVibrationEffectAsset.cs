using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    [CreateAssetMenu(fileName = "GamepadVibrationEffect", menuName = "Argos.Framework/Input/Gamepad Vibration Effect")]
    public sealed class GamepadVibrationEffectAsset : ScriptableObject
    {
        #region Enums
        public enum VibratorType
        {
            /// <summary>
            /// Strong engine (left engine).
            /// </summary>
            Strong,
            /// <summary>
            /// Weak engine (right engine).
            /// </summary>
            Weak,
            /// <summary>
            /// Both.
            /// </summary>
            Both
        }
        #endregion

        #region Inspector fields
        [SerializeField]
        [Tooltip("Strong = left engine\nWeak = right engine")]
        VibratorType _type;
        [SerializeField]
        [Tooltip("Curves allow to define complex patterns.")]
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
        AnimationCurve _strongCurve;
        [SerializeField]
        AnimationCurve _weakCurve;
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
        public float Duration { get { return this._duration; } }

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

#if UNITY_EDITOR
    [CustomEditor(typeof(GamepadVibrationEffectAsset))]
    public class GamepadVibrationEffectAssetEditor : ArgosCustomEditorBase
    {
        #region Constants
        const float MIN_DURATION = 0.01f;

        const string LABEL_POPUP_TYPE = "Vibration type";
        const string LABEL_TOGGLE_USE_CURVES = "Use curves";
        const string LABEL_TOGGLE_LOOP = "Loop";
        const string LABEL_DEFAULT_VALUE = "Intensity";
        const string LABEL_STRONG_VALUE = "Strong intensity";
        const string LABEL_WEAK_VALUE = "Weak intensity";
        const string LABEL_DURATION_VALUE = "Duration";
        const string LABEL_CONTROL_PLAY = "Play";
        const string LABEL_CONTROL_PAUSE = "Pause";
        const string LABEL_CONTROL_RESTART = "Restart";
        const string LABEL_CONTROL_STOP = "Stop";
        const string MESSAGE_CANNOT_EDIT_VALUES = "You can't edit values until the effect is finished or stoped.";
        #endregion

        #region Enums
        public enum VibrationPlaybackState
        {
            Playing,
            Paused,
            Stoped
        }
        #endregion

        #region Internal vars
        GamepadVibrationEffectAsset _target;
        SerializedProperty _type, _useCurves, _loop, _strongForce, _weakForce, _duration, _strongCurve, _weakCurve;
        VibrationPlaybackState _playbackState = VibrationPlaybackState.Stoped;
        EditorCoroutine _coroutine;
        Timer _timer;
        #endregion

        #region Events
        private void OnEnable()
        {
            this._target = (GamepadVibrationEffectAsset)this.target;

            this._type = this.serializedObject.FindProperty("_type");
            this._useCurves = this.serializedObject.FindProperty("_useCurves");
            this._loop = this.serializedObject.FindProperty("_loop");
            this._strongForce = this.serializedObject.FindProperty("_strongForce");
            this._weakForce = this.serializedObject.FindProperty("_weakForce");
            this._duration = this.serializedObject.FindProperty("_duration");
            this._strongCurve = this.serializedObject.FindProperty("_strongCurve");
            this._weakCurve = this.serializedObject.FindProperty("_weakCurve");

            this.HeaderTitle = "Gamepad Vibration Effect";

            ForceFeedback.CheckForAvailableJoystick();

            this._timer = new Timer(Timer.TimerMode.EditorMode);
            this._timer.Pause();
        }

        private void OnDisable()
        {
            this.Stop();
            ForceFeedback.ReleaseJoystick();
        }

        private void OnDestroy()
        {
            this.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            bool isMainGUIEnable = !Application.isPlaying && this._playbackState == VibrationPlaybackState.Stoped;

            this.serializedObject.Update();
            {
                GUI.enabled = !Application.isPlaying;

                this.DrawPlaybackControls();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    this.DrawProgressBar();
                    EditorGUILayout.Space();

                    GUI.enabled = isMainGUIEnable;

                    EditorGUILayout.PropertyField(this._type, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_POPUP_TYPE));
                    EditorGUILayout.PropertyField(this._useCurves, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_TOGGLE_USE_CURVES));
                    EditorGUILayout.PropertyField(this._loop, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_TOGGLE_LOOP));

                    GUI.enabled = (this._target.UseCurves || !this._target.Loop) && isMainGUIEnable;
                    {
                        EditorGUILayout.PropertyField(this._duration, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_DURATION_VALUE));
                        if (this._target.Duration < GamepadVibrationEffectAssetEditor.MIN_DURATION)
                        {
                            this._duration.floatValue = GamepadVibrationEffectAssetEditor.MIN_DURATION;
                        }
                    }
                    GUI.enabled = isMainGUIEnable;

                    if (!this._target.UseCurves)
                    {
                        if (this._target.Type == GamepadVibrationEffectAsset.VibratorType.Both || this._target.Type == GamepadVibrationEffectAsset.VibratorType.Strong)
                        {
                            EditorGUILayout.PropertyField(this._strongForce, new GUIContent(this._target.Type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                                                                                 GamepadVibrationEffectAssetEditor.LABEL_STRONG_VALUE :
                                                                                                                 GamepadVibrationEffectAssetEditor.LABEL_DEFAULT_VALUE));
                        }

                        if (this._target.Type == GamepadVibrationEffectAsset.VibratorType.Both || this._target.Type == GamepadVibrationEffectAsset.VibratorType.Weak)
                        {
                            EditorGUILayout.PropertyField(this._weakForce, new GUIContent(this._target.Type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                                                                               GamepadVibrationEffectAssetEditor.LABEL_WEAK_VALUE :
                                                                                                               GamepadVibrationEffectAssetEditor.LABEL_DEFAULT_VALUE));
                        }
                    }
                    else
                    {
                        if (this._target.Type == GamepadVibrationEffectAsset.VibratorType.Both || this._target.Type == GamepadVibrationEffectAsset.VibratorType.Strong)
                        {
                            EditorGUILayout.PropertyField(this._strongCurve, new GUIContent(this._target.Type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                                                                                 GamepadVibrationEffectAssetEditor.LABEL_STRONG_VALUE :
                                                                                                                 GamepadVibrationEffectAssetEditor.LABEL_DEFAULT_VALUE));
                        }

                        if (this._target.Type == GamepadVibrationEffectAsset.VibratorType.Both || this._target.Type == GamepadVibrationEffectAsset.VibratorType.Weak)
                        {
                            EditorGUILayout.PropertyField(this._weakCurve, new GUIContent(this._target.Type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                                                                               GamepadVibrationEffectAssetEditor.LABEL_WEAK_VALUE :
                                                                                                               GamepadVibrationEffectAssetEditor.LABEL_DEFAULT_VALUE));
                        }
                    }

                    GUI.enabled = true;

                    if (this._playbackState != VibrationPlaybackState.Stoped)
                    {
                        EditorGUILayout.HelpBox(GamepadVibrationEffectAssetEditor.MESSAGE_CANNOT_EDIT_VALUES, MessageType.Warning);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            this.serializedObject.ApplyModifiedProperties();

            this.Repaint();
        }
        #endregion

        #region Methods & Functions
        void DrawPlaybackControls()
        {
            switch (GUILayout.Toolbar(-1, new string[] { (this._playbackState == VibrationPlaybackState.Playing ?
                                                                                 GamepadVibrationEffectAssetEditor.LABEL_CONTROL_PAUSE :
                                                                                 GamepadVibrationEffectAssetEditor.LABEL_CONTROL_PLAY),
                                                         GamepadVibrationEffectAssetEditor.LABEL_CONTROL_RESTART,
                                                         GamepadVibrationEffectAssetEditor.LABEL_CONTROL_STOP }))
            {
                case 0: // Play/Pause:

                    switch (this._playbackState)
                    {
                        case VibrationPlaybackState.Playing:

                            this.Pause();
                            break;

                        case VibrationPlaybackState.Paused:

                            this.Resume();
                            break;

                        case VibrationPlaybackState.Stoped:

                            this.Play();
                            break;
                    }
                    break;

                case 1: // Restart:

                    this.Play();
                    break;

                case 2: // Stop:

                    this.Stop();
                    break;
            }
        }

        void Play()
        {
            this.Stop();
            this._playbackState = VibrationPlaybackState.Playing;
            this._coroutine = this.StartCoroutine(this.VibrationPlaybackCoroutine());
        }

        void Pause()
        {
            this._playbackState = VibrationPlaybackState.Paused;
            this._timer.Pause();
        }

        void Resume()
        {
            this._playbackState = VibrationPlaybackState.Playing;
            this._timer.Resume();
        }

        void Stop()
        {
            this._playbackState = VibrationPlaybackState.Stoped;
            this._timer.Stop();
        }

        void SetVibration(Vector2 intensity)
        {
            if (!XInput.SetVibration(intensity))
            {
                ForceFeedback.SetVibration(intensity);
            }
        }

        float GetIntensityFromCurve(AnimationCurve curve)
        {
            return curve.Evaluate(this._timer.Value);
        }

        void DrawProgressBar()
        {
            bool condition = this._target.Loop && !this._target.UseCurves;

            float currentTime = condition ? 
                                (this._playbackState == VibrationPlaybackState.Stoped ? 0f : 1f) :
                                (this._timer.Value);

            float value = condition ? currentTime : currentTime / this._target.Duration;

            string label = condition ?
                           "Playback time: ∞" :
                           $"Playback time: {currentTime.ToString("0.00")} / {this._target.Duration.ToString("0.00")}";

            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), value, label);
        }
        #endregion

        #region Coroutines
        IEnumerator VibrationPlaybackCoroutine()
        {
            this._timer.Reset();

            while (this._playbackState != VibrationPlaybackState.Stoped && !Application.isPlaying)
            {
                if (this._playbackState == VibrationPlaybackState.Playing)
                {
                    if (!this._target.Loop && this._timer.Value >= this._target.Duration)
                    {
                        this._playbackState = VibrationPlaybackState.Stoped;
                    }
                    else
                    {
                        if (this._playbackState == VibrationPlaybackState.Paused)
                        {
                            this.SetVibration(Vector2.zero);
                        }
                        else
                        {
                            Vector2 intensity = Vector2.zero;

                            if (!this._target.UseCurves)
                            {
                                intensity = new Vector2(this._target.Type != GamepadVibrationEffectAsset.VibratorType.Weak ? this._target.StrongForce : -1,
                                                        this._target.Type != GamepadVibrationEffectAsset.VibratorType.Strong ? this._target.WeakForce : -1);
                            }
                            else
                            {
                                intensity = new Vector2(this._target.Type != GamepadVibrationEffectAsset.VibratorType.Weak ? this.GetIntensityFromCurve(this._target.StrongCurve) : -1,
                                                        this._target.Type != GamepadVibrationEffectAsset.VibratorType.Strong ? this.GetIntensityFromCurve(this._target.WeakCurve) : -1);

                                if (this._target.Loop && this._timer.Value >= this._target.Duration)
                                {
                                    this._timer.Reset();
                                }
                            }

                            //Debug.Log(intensity);
                            this.SetVibration(intensity);
                        }
                    }
                }

                yield return null;
            }

            this.SetVibration(Vector2.zero);
            this._timer.Stop();
        } 
        #endregion
    }
#endif
}