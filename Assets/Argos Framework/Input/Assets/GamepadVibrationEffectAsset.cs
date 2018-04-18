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
        [Tooltip("Curves allow to define complex patterns based in time and scale values.")]
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
        #endregion

        #region Internal vars
        GamepadVibrationEffectAsset _target;
        SerializedProperty _type, _useCurves, _loop, _strongForce, _weakForce, _duration, _strongCurve, _weakCurve;
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
            InputManager.Instance?.SetGamepadVibration(Vector2.zero);
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            {
                this.PlaybackControls();

                EditorGUILayout.PropertyField(this._type, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_POPUP_TYPE));
                EditorGUILayout.PropertyField(this._useCurves, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_TOGGLE_USE_CURVES));
                EditorGUILayout.PropertyField(this._loop, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_TOGGLE_LOOP));

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

                    if (!this._target._loop)
                    {
                        EditorGUILayout.PropertyField(this._duration, new GUIContent(GamepadVibrationEffectAssetEditor.LABEL_DURATION_VALUE));
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
            }
            this.serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Methods & Functions
        void PlaybackControls()
        {
            switch (GUILayout.Toolbar(-1, new string[] { "Play", "Restart", "Stop" }))
            {
                case 0:
                    Debug.Log("Play");
                    break;
                case 1:
                    Debug.Log("Restart");
                    break;
                case 2:
                    Debug.Log("Stop");
                    break;
            }
        } 
        #endregion
    }
#endif
}