using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomEditor(typeof(RotateSkybox))]
    public class RotateSkyboxEditor : Editor
    {
        #region Event listeners
        private void OnEnable()
        {
            this.StartCoroutine(this.EditorUpdateCoroutine());
        }

        private void OnDisable()
        {
            this.StopAllCoroutines();
        }

        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspectorWithoutScriptField();
        }
        #endregion

        #region Coroutines
        IEnumerator EditorUpdateCoroutine()
        {
            var instance = this.target as RotateSkybox;

            instance.Rotation = instance.initialAngle;

            while (true)
            {
                if (instance.rotateInEditMode)
                {
                    instance.Rotation = Time.realtimeSinceStartup * instance.speed;
                }
                yield return null; 
            }
        } 
        #endregion
    } 
}
