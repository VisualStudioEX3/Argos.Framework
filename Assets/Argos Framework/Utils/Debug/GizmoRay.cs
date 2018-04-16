using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

namespace Argos.Framework.Utils.Debug
{
    /// <summary>
    /// Gizmo Ray.
    /// </summary>
    /// <remarks>Draws a gizmo ray, with the selected color, from the current game object position in the local forward direction multiply by the length parameter.</remarks>
    [AddComponentMenu("Argos.Framework/Utils/Debug/Gizmo Ray")]
    public class GizmoRay : MonoBehaviour
    {
        #region Public vars
        public Color Color = Color.red;
        public float Length = 100f;
        #endregion

        #region Update logic
        private void OnDrawGizmos()
        {
            Gizmos.color = this.Color;
            Gizmos.DrawRay(this.transform.position, this.transform.forward * this.Length);
        }
        #endregion
    }

}