using UnityEngine;
using Argos.Framework;

namespace Argos.Framework.Utils
{
    [AddComponentMenu("Argos.Framework/Utils/Rotate Object"), DisallowMultipleComponent, ExecuteInEditMode]
    public class RotateObject : MonoBehaviour
    {
        #region Public vars
        public float Speed = 1f;
        [Tooltip("Step defined in degrees.")]
        public float Step = 1f;
        public Vector3 Axis = Vector3.zero;
        public Space Space = Space.Self;
        #endregion

        #region Update logic
        void Update()
        {
            this.Axis = new Vector3()
            {
                x = Mathf.Clamp(this.Axis.x, -1f, 1f),
                y = Mathf.Clamp(this.Axis.y, -1f, 1f),
                z = Mathf.Clamp(this.Axis.z, -1f, 1f)
            };

            if (Application.isPlaying)
            {
                this.transform.Rotate((this.Axis * this.Step) * (Time.deltaTime * this.Speed), this.Space);
            }
        } 
        #endregion
    } 
}
