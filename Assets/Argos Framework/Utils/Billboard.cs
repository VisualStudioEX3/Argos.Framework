using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

namespace Argos.Framework.Utils
{
    /// <summary>
    /// Billboard effect.
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("Argos.Framework/Utils/Billboard")]
    public class Billboard : MonoBehaviour
    {
        #region Inspector fields
        [SerializeField]
        bool InvertDirection = false;
        #endregion

        #region Update logic
        void Update()
        {
            if (this.InvertDirection)
            {
                this.transform.forward = Camera.main.transform.forward;
            }
            else
            {
                this.transform.LookAt(Camera.main.transform);
            }
        } 
        #endregion
    }

}