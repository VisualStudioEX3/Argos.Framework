using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Base class to implement <see cref="MonoBehaviour"/> derived classes as singleton instances.
    /// </summary>
    /// <typeparam name="T">Type of the MonoBehaviour derived class.</typeparam>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Properties
        public static T Instance { get; private set; } 
        #endregion

        #region Initializers
        public virtual void Awake()
        {
            if (MonoBehaviourSingleton<T>.Instance != null)
            {
                throw new InvalidOperationException($"MonoBehaviourSingleton<{typeof(T)}>: Error to initialize singleton instance. A previous instance is created on \"{this.gameObject.scene.name}\" scene on \"{this.gameObject.name}\" game object!");
            }

            MonoBehaviourSingleton<T>.Instance = this as T;
        }

        public virtual void OnDestroy()
        {
            MonoBehaviourSingleton<T>.Instance = null;
        } 
        #endregion
    } 
}
