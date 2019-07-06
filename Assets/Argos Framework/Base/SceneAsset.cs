using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Serializable Scene asset structure.
    /// </summary>
    /// <remarks>This structure allow to show <see cref="UnityEngine.SceneManagement.Scene"/> fields on inspectors.</remarks>
    [Serializable]
    public struct SceneAsset
    {
        #region Inspector fields
#pragma warning disable 649
        [SerializeField]
        UnityEngine.Object _asset;
        [SerializeField]
        string _assetPath;
        [SerializeField]
        string _scenePath;
        [SerializeField]
        int _sceneIndex;
#pragma warning restore
        #endregion

        #region Properties
        /// <summary>
        /// Path to asset file in project.
        /// </summary>
        public string AssetPath { get { return this._assetPath; } }

        /// <summary>
        /// Scene path on Unity project (the path use to load the scene using the <see cref="UnityEngine.SceneManagement.SceneManager"/>).
        /// </summary>
        public string ScenePath { get { return this._scenePath; } }

        /// <summary>
        /// Scene index on project (the index use to load the scene using the <see cref="UnityEngine.SceneManagement.SceneManager"/>).
        /// </summary>
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