using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Input
{
    [CustomEditor(typeof(GamepadVibrationEffectController))]
    public class GamepadVibrationEffectControllerEditor : Editor
    {
        #region Internal vars
        GamepadVibrationEffectController _target;
        SerializedProperty _effect, _fixedUpdate, _useUnScaledTime, _playOnStart;
        #endregion

        #region Events
        private void OnEnable()
        {
            this._target = (GamepadVibrationEffectController)this.target;

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
}