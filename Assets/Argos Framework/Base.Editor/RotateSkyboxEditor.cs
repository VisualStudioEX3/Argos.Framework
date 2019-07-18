using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomEditor(typeof(RotateSkybox))]
    public class RotateSkyboxEditor : Editor
    {
        #region Internal vars
        SerializedProperty _speed;
        SerializedProperty _initialAngle;
        SerializedProperty _testAngle;
        #endregion

        #region Event listeners
        private void OnEnable()
        {
            this._speed = this.serializedObject.FindProperty("speed");
            this._initialAngle = this.serializedObject.FindProperty("initialAngle");
            this._testAngle = this.serializedObject.FindProperty("_testAngle");

            this.StartCoroutine(this.TestAngleCoroutine());
        }

        private void OnDisable()
        {
            this.StopAllCoroutines();
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            {
                EditorGUILayout.PropertyField(this._speed);
                EditorGUILayout.PropertyField(this._initialAngle);

                GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("You can manually test the rotation skybox in edit mode.", MessageType.Info);
                    EditorGUILayout.PropertyField(this._testAngle, new GUIContent("Angle"));
                }
                GUI.enabled = true;
            }
            this.serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Coroutines
        IEnumerator TestAngleCoroutine()
        {
            var instance = (this.target as RotateSkybox);
            while (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                instance.Rotation = this._testAngle.floatValue;
                yield return null;
            }
        } 
        #endregion
    }
}