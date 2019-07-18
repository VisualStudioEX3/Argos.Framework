using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Component to rotate the current scene skybox.
    /// </summary>
    [AddComponentMenu("Argos.Framework/Utils/Rotate Skybox"), DisallowMultipleComponent, ExecuteInEditMode]
    public class RotateSkybox : MonoBehaviour
    {
        #region Constants
        const string ROTATION_PROPERTY = "_Rotation";
        #endregion

        #region Internal vars
#if UNITY_EDITOR
        [SerializeField, Range(0f, 360f)]
        float _testAngle;
#endif
        #endregion

        #region Public vars
        /// <summary>
        /// Rotation speed.
        /// </summary>
        public float speed = 0.5f;

        /// <summary>
        /// Initial rotation angle.
        /// </summary>
        [Range(0f, 360f)]
        public float initialAngle = 0f;
        #endregion

        #region Properties
        /// <summary>
        /// Get or set the rotation angle of the skybox.
        /// </summary>
        public float Rotation
        {
            get
            {
                return RenderSettings.skybox.GetFloat(RotateSkybox.ROTATION_PROPERTY);
            }

            set
            {
                RenderSettings.skybox.SetFloat(RotateSkybox.ROTATION_PROPERTY, value);
            }
        }
        #endregion

        #region Initializers
        private void Start()
        {
            this.Rotation = this.initialAngle;
        }

        private void OnDestroy()
        {
            this.Rotation = 0f;
        }
        #endregion

        #region Update logic
        void Update()
        {
            this.Rotation = Time.time * this.speed;
        }
        #endregion
    }

}