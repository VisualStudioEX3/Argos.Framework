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
        string _assetPath;
        [SerializeField]
        string _scenePath;
        [SerializeField]
        int _sceneIndex;
        #endregion

        #region Properties
        public string AssetPath { get { return this._assetPath; } }
        public string ScenePath { get { return this._scenePath; } }
        public int SceneIndex { get { return this._sceneIndex; } } 
        #endregion

        #region Operators
        public static implicit operator string(SceneAsset scene)
        {
            return scene._scenePath;
        }
        #endregion
    }
}