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
    /// <remarks>Create a vibration/rumble effect for compatible gamepads (XBox controllers and joysticks with ForceFeedback support) wich defines force type (strong, weak or both), duration and constant intensity or complex patterns using curves.
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

        #region Internal vars
        GamepadVibrationEffectAsset _target;
        SerializedProperty _type, _useCurves, _loop, _strongForce, _weakForce, _duration, _strongCurve, _weakCurve;
        #endregion

        #region Static vars
        static Task _vibrationPlaybackTest;
        static GamepadVibrationEffectAsset _testTarget;
        static bool _isApplicationPlaying;
        #endregion

        #region Internal class
        /// <summary>
        /// Internal static class for manage, in editor mode, the playback vibration tests, in a separate thread, without Argos InputManager class dependency.
        /// </summary>
        static class EditorVibrationTask
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
            #endregion

            #region Properties
            public static GamepadVibrationEffectAsset Asset { get; set; }
            public static VibrationPlaybackState State { get; private set; }
            public static float CurrentTime { get; private set; }
            #endregion

            #region Constructor
            static EditorVibrationTask()
            {
                EditorVibrationTask._stopWatch = new System.Diagnostics.Stopwatch();
                EditorVibrationTask.State = VibrationPlaybackState.Stoped;
            }
            #endregion

            #region Methods & Functions
            static float GetIntensityFromCurve(AnimationCurve curve)
            {
                return curve.Evaluate((float)(EditorVibrationTask.CurrentTime / 1000f));
            }

            static void SetVibration(Vector2 intensity)
            {
                if (!XInput.SetVibration(intensity))
                {
                    ForceFeedback.SetVibration(intensity);
                }
            }

            public static void Play()
            {
                EditorVibrationTask.Stop();
                EditorVibrationTask.State = VibrationPlaybackState.Playing;

                ForceFeedback.CheckForAvailableJoystick();

                EditorVibrationTask._task = Task.Run(() =>
                {
                    EditorVibrationTask._duration = (int)(EditorVibrationTask.Asset._duration * 1000f);

                    EditorVibrationTask._stopWatch.Restart();
                    EditorVibrationTask._stopWatch.Start();

                    while (EditorVibrationTask.State != VibrationPlaybackState.Stoped && !GamepadVibrationEffectAssetEditor._isApplicationPlaying)
                    {
                        EditorVibrationTask.CurrentTime = EditorVibrationTask._stopWatch.ElapsedMilliseconds;

                        if (!EditorVibrationTask.Asset._loop && EditorVibrationTask.CurrentTime > EditorVibrationTask._duration)
                        {
                            EditorVibrationTask.State = VibrationPlaybackState.Stoped;
                        }
                        else
                        {
                            if (EditorVibrationTask.State == VibrationPlaybackState.Paused)
                            {
                                EditorVibrationTask.SetVibration(Vector2.zero);
                            }
                            else
                            {
                                Vector2 intensity = Vector2.zero;

                                if (!EditorVibrationTask.Asset._useCurves)
                                {
                                    switch (EditorVibrationTask.Asset._type)
                                    {
                                        case GamepadVibrationEffectAsset.VibratorType.Strong:

                                            intensity = new Vector2(EditorVibrationTask.Asset._strongForce, -1f);
                                            break;

                                        case GamepadVibrationEffectAsset.VibratorType.Weak:

                                            intensity = new Vector2(-1f, EditorVibrationTask.Asset._weakForce);
                                            break;

                                        case GamepadVibrationEffectAsset.VibratorType.Both:

                                            intensity = new Vector2(EditorVibrationTask.Asset._strongForce, EditorVibrationTask.Asset._weakForce);
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (EditorVibrationTask.Asset._type)
                                    {
                                        case GamepadVibrationEffectAsset.VibratorType.Strong:

                                            intensity = new Vector2(EditorVibrationTask.GetIntensityFromCurve(EditorVibrationTask.Asset._strongCurve), -1f);
                                            break;

                                        case GamepadVibrationEffectAsset.VibratorType.Weak:

                                            intensity = new Vector2(-1f, EditorVibrationTask.GetIntensityFromCurve(EditorVibrationTask.Asset._weakCurve));
                                            break;

                                        case GamepadVibrationEffectAsset.VibratorType.Both:

                                            intensity = new Vector2(EditorVibrationTask.GetIntensityFromCurve(EditorVibrationTask.Asset._strongCurve), 
                                                                    EditorVibrationTask.GetIntensityFromCurve(EditorVibrationTask.Asset._weakCurve));
                                            break;
                                    }

                                    if (EditorVibrationTask.Asset._loop && EditorVibrationTask.CurrentTime > EditorVibrationTask._duration)
                                    {
                                        EditorVibrationTask._stopWatch.Restart();
                                    }
                                }

                                EditorVibrationTask.SetVibration(intensity);
                            }
                        }
                    }

                    EditorVibrationTask.SetVibration(Vector2.zero);
                    EditorVibrationTask.CurrentTime = 0f;
                });

                ForceFeedback.ReleaseJoystick();
            }

            public static void Pause()
            {
                EditorVibrationTask._stopWatch.Stop();
                EditorVibrationTask.State = VibrationPlaybackState.Paused;
            }

            public static void Resume()
            {
                EditorVibrationTask._stopWatch.Start();
                EditorVibrationTask.State = VibrationPlaybackState.Playing;
            }

            public static void Stop()
            {
                EditorVibrationTask.State = VibrationPlaybackState.Stoped;
                EditorVibrationTask._stopWatch.Stop();
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

            this.HeaderTitle = "Gamepad Vibration Effect";
        }

        private void OnDisable()
        {
            EditorVibrationTask.Stop();
        }

        public override void OnInspectorGUI()
        {
            bool isMainGUIEnable = !Application.isPlaying && EditorVibrationTask.State == EditorVibrationTask.VibrationPlaybackState.Stoped;

            GamepadVibrationEffectAssetEditor._isApplicationPlaying = Application.isPlaying;

            this.serializedObject.Update();
            {
                GUI.enabled = !GamepadVibrationEffectAssetEditor._isApplicationPlaying;

                this.DrawPlaybackControls();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    this.DrawProgressBar();
                    EditorGUILayout.Space();

                    GUI.enabled = isMainGUIEnable;

                    EditorGUILayout.PropertyField(this._type, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_POPUP_TYPE));
                    EditorGUILayout.PropertyField(this._useCurves, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_TOGGLE_USE_CURVES));
                    EditorGUILayout.PropertyField(this._loop, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_TOGGLE_LOOP));

                    GUI.enabled = (this._target._useCurves || !this._target._loop) && isMainGUIEnable;
                    {
                        EditorGUILayout.PropertyField(this._duration, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_DURATION_VALUE));
                        if (this._target._duration < GamepadVibrationEffectAssetEditor.MIN_DURATION)
                        {
                            this._target._duration = GamepadVibrationEffectAssetEditor.MIN_DURATION;
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

                    if (EditorVibrationTask.State != EditorVibrationTask.VibrationPlaybackState.Stoped)
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
        void DrawPlaybackControls()
        {
            switch (GUILayout.Toolbar(-1, new string[] { (EditorVibrationTask.State == EditorVibrationTask.VibrationPlaybackState.Playing ? 
                                                                                 GamepadVibrationEffectAssetEditor.LABEL_CONTROL_PAUSE : 
                                                                                 GamepadVibrationEffectAssetEditor.LABEL_CONTROL_PLAY),
                                                         GamepadVibrationEffectAssetEditor.LABEL_CONTROL_RESTART,
                                                         GamepadVibrationEffectAssetEditor.LABEL_CONTROL_STOP }))
            {
                case 0: // Play/Pause:

                    switch (EditorVibrationTask.State)
                    {
                        case EditorVibrationTask.VibrationPlaybackState.Playing:

                            EditorVibrationTask.Pause();
                            break;

                        case EditorVibrationTask.VibrationPlaybackState.Paused:

                            EditorVibrationTask.Resume();
                            break;

                        case EditorVibrationTask.VibrationPlaybackState.Stoped:

                            EditorVibrationTask.Asset = this._target;
                            EditorVibrationTask.Play();
                            break;
                    }
                    break;

                case 1: // Restart:

                    EditorVibrationTask.Play();
                    break;

                case 2: // Stop:

                    EditorVibrationTask.Stop();
                    break;
            }
        }

        void DrawProgressBar()
        {
            bool condition = this._target._loop && !this._target._useCurves;
            float currentTime = condition ? 
                                (EditorVibrationTask.State == EditorVibrationTask.VibrationPlaybackState.Stoped ? 0f : 1f) : 
                                (EditorVibrationTask.CurrentTime / 1000f);

            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), currentTime / this._target._duration, $"Playback time: {(condition ? "∞" : currentTime.ToString("0.00"))} / {this._target._duration.ToString("0.00")}");
        }
        #endregion
    }
#endif
}