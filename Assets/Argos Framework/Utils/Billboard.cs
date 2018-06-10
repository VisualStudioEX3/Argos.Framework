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
    [AddComponentMenu("Argos.Framework/Utils/Billboard"), DisallowMultipleComponent]
    public class Billboard : MonoBehaviour
    {
        #region Internal vars
        Camera _mainCamera; 
        #endregion

        #region Inspector fields
        [SerializeField]
        bool InvertDirection = false;
        #endregion

        #region Initializers
        void Start()
        {
            this._mainCamera = Camera.main;
        } 
        #endregion

        #region Update logic
        void Update()
        {
            if (this.InvertDirection)
            {
                this.transform.forward = this._mainCamera.transform.forward;
            }
            else
            {
                this.transform.LookAt(this._mainCamera.transform);
            }
        } 
        #endregion
    }

}