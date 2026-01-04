using UnityEngine;
using Argos.Framework.Utils;

namespace Argos.Framework
{
    /// <summary>
    /// Component to rotate a game object.
    /// </summary>
    [AddComponentMenu("Argos.Framework/Utils/Rotate Object"), DisallowMultipleComponent, ExecuteInEditMode]
    public sealed class RotateObject : MonoBehaviour
    {
        #region Public vars
        public float speed = 1f;
        [Tooltip("Step defined in degrees.")]
        public float step = 1f;
        public Vector3 axis = Vector3.zero;
        public Space relativeTo = Space.Self;
        #endregion

        #region Update logic
        void Update()
        {
            this.axis = VectorsUtility.Clamp(this.axis, Vector3.one * -1f, Vector3.one);

            if (Application.isPlaying)
            {
                this.transform.Rotate((this.axis * this.step) * (Time.deltaTime * this.speed), this.relativeTo); 
            }
        }
        #endregion
    }
}
