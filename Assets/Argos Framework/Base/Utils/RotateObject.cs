using UnityEngine;
using Argos.Framework;

namespace Argos.Framework.Utils
{
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
            this.axis = new Vector3()
            {
                x = Mathf.Clamp(this.axis.x, -1f, 1f),
                y = Mathf.Clamp(this.axis.y, -1f, 1f),
                z = Mathf.Clamp(this.axis.z, -1f, 1f)
            };

            this.transform.Rotate((this.axis * this.step) * (Time.deltaTime * this.speed), this.space);
        }
        #endregion
    }
}
