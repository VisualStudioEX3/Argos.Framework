using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
#endif
using Argos.Framework.Input.Extensions;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Gamepad Vibration Effect.
    /// </summary>
    /// <remarks>Create a vibration/rumble effect for compatible gamepads (XBox controllers and joysticks with ForceFeedback support) wich defines force type (strong, weak or both), intensitiy, duration and even complex patterns using curves.
    /// 
    /// The gamepad vibration effects only supported on Windows (desktop and UWP apps) and XBox One systems.
    /// ForceFeedback only supported on Windows desktop apps.</remarks>
    [CreateAssetMenu(fileName = "GamepadVibrationEffect", menuName = "Argos.Framework/Input/Gamepad Vibration Effect")]
    public class GamepadVibrationEffectAsset : ScriptableObject
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
        public VibratorType _type;
        [SerializeField]
        [Tooltip("Curves allow to define complex patterns.")]
        public bool _useCurves;
        [SerializeField]
        public bool _loop;

        [SerializeField]
        [Range(0f, 1f)]
        public float _strongForce;
        [SerializeField]
        [Range(0f, 1f)]
        public float _weakForce;

        [SerializeField]
        public float _duration;

        [SerializeField]
        public AnimationCurve _strongCurve;
        [SerializeField]
        public AnimationCurve _weakCurve;
        #endregion

        #region Events
        private void OnDestroy()
        {
            InputManager.Instance?.SetGamepadVibration(Vector2.zero);
        } 
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GamepadVibrationEffectAsset))]
    public class GamepadVibrationEffectAssetEditor : Editor
    {
        #region Constants
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

        #region Internal vars
        GamepadVibrationEffectAsset _target;
        SerializedProperty _type, _useCurves, _loop, _strongForce, _weakForce, _duration, _strongCurve, _weakCurve;
        #endregion

        #region Static vars
        static Task _vibrationPlaybackTest;
        static CancellationTokenSource _cancellationTokenSource;
        static CancellationToken _cancellationToken;
        static GamepadVibrationEffectAsset _testTarget;
        static bool _isApplicationPlaying;
        #endregion

        #region Internal class
        /// <summary>
        /// Internal static class for manage in editor mode the playback vibration tests in separate thread.
        /// </summary>
        static class VibrationTask
        {
            #region Enums
            public enum VibrationPlaybackState
            {
                Playing,
                Paused,
                Stoped
            }
            #endregion

            #region Internal vars
            static Task _task;
            static System.Diagnostics.Stopwatch _stopWatch;
            static int _duration;
            static float _currentTime;
            #endregion

            #region Properties
            public static GamepadVibrationEffectAsset Asset { get; set; }
            public static VibrationPlaybackState State { get; private set; }
            #endregion

            #region Constructor
            static VibrationTask()
            {
                VibrationTask._stopWatch = new System.Diagnostics.Stopwatch();
                VibrationTask.State = VibrationPlaybackState.Stoped;
            }
            #endregion

            #region Methods & Functions
            static float GetIntensityFromCurve(AnimationCurve curve)
            {
                return curve.Evaluate((float)(VibrationTask._currentTime / 1000f));
            }

            public static void Play()
            {
                VibrationTask.Stop();
                VibrationTask.State = VibrationPlaybackState.Playing;

                VibrationTask._task = Task.Run(() =>
                {
                    VibrationTask._duration = (int)(VibrationTask.Asset._duration * 1000f);
                    VibrationTask._stopWatch.Start();

                    while (VibrationTask.State != VibrationPlaybackState.Stoped && !GamepadVibrationEffectAssetEditor._isApplicationPlaying)
                    {
                        VibrationTask._currentTime = VibrationTask._stopWatch.ElapsedMilliseconds;

                        if (!VibrationTask.Asset._loop && VibrationTask._currentTime > VibrationTask._duration)
                        {
                            VibrationTask.State = VibrationPlaybackState.Stoped;
                        }
                        else
                        {
                            if (VibrationTask.State == VibrationPlaybackState.Paused)
                            {
                                InputManager.Instance?.SetGamepadVibration(Vector2.zero);
                            }
                            else
                            {
                                Vector2 intensity = Vector2.zero;

                                if (!VibrationTask.Asset._useCurves)
                                {
                                    switch (VibrationTask.Asset._type)
                                    {
                                        case GamepadVibrationEffectAsset.VibratorType.Strong:

                                            intensity = new Vector2(VibrationTask.Asset._strongForce, -1f);
                                            break;

                                        case GamepadVibrationEffectAsset.VibratorType.Weak:

                                            intensity = new Vector2(-1f, VibrationTask.Asset._weakForce);
                                            break;

                                        case GamepadVibrationEffectAsset.VibratorType.Both:

                                            intensity = new Vector2(VibrationTask.Asset._strongForce, VibrationTask.Asset._weakForce);
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (VibrationTask.Asset._type)
                                    {
                                        case GamepadVibrationEffectAsset.VibratorType.Strong:

                                            intensity = new Vector2(VibrationTask.GetIntensityFromCurve(VibrationTask.Asset._strongCurve), -1f);
                                            break;

                                        case GamepadVibrationEffectAsset.VibratorType.Weak:

                                            intensity = new Vector2(-1f, VibrationTask.GetIntensityFromCurve(VibrationTask.Asset._weakCurve));
                                            break;

                                        case GamepadVibrationEffectAsset.VibratorType.Both:

                                            intensity = new Vector2(VibrationTask.GetIntensityFromCurve(VibrationTask.Asset._strongCurve), 
                                                                    VibrationTask.GetIntensityFromCurve(VibrationTask.Asset._weakCurve));
                                            break;
                                    }

                                    if (VibrationTask.Asset._loop && VibrationTask._currentTime > VibrationTask._duration)
                                    {
                                        VibrationTask._stopWatch.Reset();
                                    }
                                }

                                InputManager.Instance?.SetGamepadVibration(intensity);
                            }
                        }
                    }

                    InputManager.Instance?.SetGamepadVibration(Vector2.zero);
                });
            }

            public static void Pause()
            {
                VibrationTask._stopWatch.Stop();
                VibrationTask.State = VibrationPlaybackState.Paused;
            }

            public static void Resume()
            {
                VibrationTask._stopWatch.Start();
                VibrationTask.State = VibrationPlaybackState.Playing;
            }

            public static void Stop()
            {
                VibrationTask.State = VibrationPlaybackState.Stoped;
                VibrationTask._stopWatch.Stop();
                VibrationTask._stopWatch.Restart();
            }
            #endregion
        } 
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
        }

        private void OnDisable()
        {
            VibrationTask.Stop();
        }

        public override void OnInspectorGUI()
        {
            bool isMainGUIEnable = !Application.isPlaying && VibrationTask.State == VibrationTask.VibrationPlaybackState.Stoped;

            GamepadVibrationEffectAssetEditor._isApplicationPlaying = Application.isPlaying;

            this.serializedObject.Update();
            {
                GUI.enabled = !Application.isPlaying;

                this.PlaybackControls();
                
                GUI.enabled = isMainGUIEnable;
                
                EditorGUILayout.BeginVertical("helpbox");
                {
                    EditorGUILayout.PropertyField(this._type, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_POPUP_TYPE));
                    EditorGUILayout.PropertyField(this._useCurves, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_TOGGLE_USE_CURVES));
                    EditorGUILayout.PropertyField(this._loop, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_TOGGLE_LOOP));

                    GUI.enabled = (this._target._useCurves || !this._target._loop) && isMainGUIEnable;
                    {
                        EditorGUILayout.PropertyField(this._duration, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_DURATION_VALUE));
                        if (this._target._duration < 0f)
                        {
                            this._target._duration = 0f;
                        }
                    }
                    GUI.enabled = isMainGUIEnable;

                    if (!this._target._useCurves)
                    {
                        if (this._target._type == GamepadVibrationEffectAsset.VibratorType.Both || this._target._type == GamepadVibrationEffectAsset.VibratorType.Strong)
                        {
                            EditorGUILayout.PropertyField(this._strongForce, new GUIContent(this._target._type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                                                                                  GamepadVibrationEffectAssetEditor.LABEL_STRONG_VALUE :
                                                                                                                  GamepadVibrationEffectAssetEditor.LABEL_DEFAULT_VALUE));
                        }

                        if (this._target._type == GamepadVibrationEffectAsset.VibratorType.Both || this._target._type == GamepadVibrationEffectAsset.VibratorType.Weak)
                        {
                            EditorGUILayout.PropertyField(this._weakForce, new GUIContent(this._target._type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                                                                                GamepadVibrationEffectAssetEditor.LABEL_WEAK_VALUE :
                                                                                                                GamepadVibrationEffectAssetEditor.LABEL_DEFAULT_VALUE));
                        }
                    }
                    else
                    {
                        if (this._target._type == GamepadVibrationEffectAsset.VibratorType.Both || this._target._type == GamepadVibrationEffectAsset.VibratorType.Strong)
                        {
                            EditorGUILayout.PropertyField(this._strongCurve, new GUIContent(this._target._type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                                                                                  GamepadVibrationEffectAssetEditor.LABEL_STRONG_VALUE :
                                                                                                                  GamepadVibrationEffectAssetEditor.LABEL_DEFAULT_VALUE));
                        }

                        if (this._target._type == GamepadVibrationEffectAsset.VibratorType.Both || this._target._type == GamepadVibrationEffectAsset.VibratorType.Weak)
                        {
                            EditorGUILayout.PropertyField(this._weakCurve, new GUIContent(this._target._type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                                                                                GamepadVibrationEffectAssetEditor.LABEL_WEAK_VALUE :
                                                                                                                GamepadVibrationEffectAssetEditor.LABEL_DEFAULT_VALUE));
                        }
                    }

                    GUI.enabled = true;

                    if (VibrationTask.State != VibrationTask.VibrationPlaybackState.Stoped)
                    {
                        EditorGUILayout.HelpBox(GamepadVibrationEffectAssetEditor.MESSAGE_CANNOT_EDIT_VALUES, MessageType.Warning);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            this.serializedObject.ApplyModifiedProperties();

            // Force to redraw inspector view each frame:
            EditorUtility.SetDirty(target);
        }
        #endregion

        #region Methods & Functions
        void PlaybackControls()
        {
            switch (GUILayout.Toolbar(-1, new string[] { (VibrationTask.State == VibrationTask.VibrationPlaybackState.Playing ? 
                                                                                 GamepadVibrationEffectAssetEditor.LABEL_CONTROL_PAUSE : 
                                                                                 GamepadVibrationEffectAssetEditor.LABEL_CONTROL_PLAY),
                                                         GamepadVibrationEffectAssetEditor.LABEL_CONTROL_RESTART,
                                                         GamepadVibrationEffectAssetEditor.LABEL_CONTROL_STOP }))
            {
                case 0: // Play/Pause:

                    switch (VibrationTask.State)
                    {
                        case VibrationTask.VibrationPlaybackState.Playing:

                            VibrationTask.Pause();
                            break;

                        case VibrationTask.VibrationPlaybackState.Paused:

                            VibrationTask.Resume();
                            break;

                        case VibrationTask.VibrationPlaybackState.Stoped:

                            VibrationTask.Asset = this._target;
                            VibrationTask.Play();
                            break;
                    }
                    break;

                case 1: // Restart:

                    VibrationTask.Play();
                    break;

                case 2: // Stop:

                    VibrationTask.Stop();
                    break;
            }
        }
        #endregion
    }
#endif
}