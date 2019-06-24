using UnityEngine;
using Argos.Framework;

namespace Argos.Framework
{
    /// <summary>
    /// Component to rotate a game object.
    /// </summary>
    [AddComponentMenu("Argos.Framework/Utils/Rotate Object"), DisallowMultipleComponent]
    public sealed class RotateObject : MonoBehaviour
    {
        #region Public vars
        public float speed = 1f;
        [Tooltip("Step defined in degrees.")]
        public float step = 1f;
        public Vector3 axis = Vector3.zero;
        public Space space = Space.Self;
        #endregion

        #region Update logic
        void Update()
        {
            this.axis.Clamp(Vector3.one * -1f, Vector3.one);
            this.transform.Rotate((this.axis * this.step) * (Time.deltaTime * this.speed), this.space);
        }
        #endregion
    }
}
