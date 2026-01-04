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

        #region Event listeners
        private void OnEnable()
        {
            this._target = (GamepadVibrationEffectController)this.target;

            this._effect = this.serializedObject.FindProperty("effect");
            this._fixedUpdate = this.serializedObject.FindProperty("fixedUpdate");
            this._useUnScaledTime = this.serializedObject.FindProperty("useUnScaledTime");
            this._playOnStart = this.serializedObject.FindProperty("playOnStart");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            {
                EditorGUILayout.PropertyField(this._effect);
                EditorGUILayout.PropertyField(this._fixedUpdate);
                this._useUnScaledTime.boolValue = EditorGUILayout.Popup("Update mode", this._useUnScaledTime.boolValue ? 1 : 0, new string[] { "Normal", "Unscaled Time" }) != 0;
                EditorGUILayout.PropertyField(this._playOnStart);

                if (this._target.effect)
                {
                    var effectInfo = new StringBuilder();
                    effectInfo.AppendFormat("Type: {0} ({1}) | ", this._target.effect.Type,
                                                               (this._target.effect.Type == GamepadVibrationEffectAsset.VibratorType.Both ?
                                                               "Left & Right engines" :
                                                               this._target.effect.Type == GamepadVibrationEffectAsset.VibratorType.Strong ?
                                                                   "Left engine" :
                                                                   "Right engine"));

                    effectInfo.AppendFormat("Use curves: {0} | ", this._target.effect.UseCurves ? "Yes" : "No");
                    effectInfo.AppendFormat("Is looped: {0} {1}", this._target.effect.Loop ? "Yes" : "No", !this._target.effect.Loop ? "| " : string.Empty);
                    if (!this._target.effect.Loop)
                    {
                        effectInfo.AppendFormat("Duration: {0:0.00} {1}", this._target.effect.Duration, this._target.effect.Duration == 1f ? "second" : "seconds");
                    }

                    EditorGUILayout.HelpBox(effectInfo.ToString(), MessageType.Info);
                }
            }
            this.serializedObject.ApplyModifiedProperties();
        }
        #endregion
    } 
}