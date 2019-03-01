using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    [Serializable]
    public struct SceneAsset
    {
        #region Inspector fields
        [SerializeField]
        UnityEngine.Object _asset;
        [SerializeField]
        string _path;
        #endregion

        #region Operators
        public static implicit operator string(SceneAsset scene)
        {
            return scene._path;
        } 
        #endregion
    }
}