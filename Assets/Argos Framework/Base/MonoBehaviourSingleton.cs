using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Base class to implement MonoBehaviour derived classes as singleton instances.
    /// </summary>
    /// <typeparam name="T">Type of the MonoBehaviour derived class.</typeparam>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Properties
        public T Instance { get; private set; } 
        #endregion

        #region Initializers
        public virtual void Awake()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException($"MonoBehaviourSingleton<{this.GetClassName()}>: Error to initialize singleton instance. A previous instance is created!");
            }

            Instance = this as T;
        }

        public virtual void OnDestroy()
        {
            Instance = null;
        } 
        #endregion
    } 
}
