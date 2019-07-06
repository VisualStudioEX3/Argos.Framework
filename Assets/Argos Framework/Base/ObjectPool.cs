using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Object Pool base class.
    /// </summary>
    /// <typeparam name="T">Type of the object to instantiate.</typeparam>
    public abstract class ObjectPool<T> : MonoBehaviour, IEnumerable<T> where T : Component
    {
        #region Internal vars
        T[] _instances;
        #endregion

        #region Inspector fields
#pragma warning disable 649
        [SerializeField]
        T[] _prefabs;
        [SerializeField]
        int _maxInstances;
        [SerializeField, Tooltip("The parent for all instances. If the parent is null, this game object is the parent.")]
        Transform _parent;
#pragma warning restore
        #endregion

        #region Properties
        /// <summary>
        /// Total instances of this object pool.
        /// </summary>
        public int Total { get { return this._instances.Length; } }

        /// <summary>
        /// Active instances.
        /// </summary>
        public int Actives
        {
            get
            {
                int actives = 0;

                for (int i = 0; i < this.Total; i++)
                {
                    if (this[i].gameObject.activeSelf)
                    {
                        actives++;
                    }
                }

                return actives;
            }
        }

        /// <summary>
        /// Available instances.
        /// </summary>
        public int Availables { get { return this.Total - this.Actives; } }

        /// <summary>
        /// Access an instance by index.
        /// </summary>
        /// <param name="index">Index of the instance.</param>
        /// <returns>Return the reference to the instance.</returns>
        public T this[int index] { get { return this._instances[index]; } private set { this._instances[index] = value; } }
        #endregion

        #region Initializers
        public virtual void Awake()
        {
            this._instances = new T[this._maxInstances];

            for (int i = 0; i < this.Total; i++)
            {
                this[i] = Instantiate(this._prefabs[UnityEngine.Random.Range(0, this._prefabs.Length)], Vector3.zero, Quaternion.identity, this._parent ?? this.transform);
                this[i].gameObject.SetActive(false);
            }
        }

        public virtual void OnDestroy()
        {
            foreach (var instance in this)
            {
                Destroy(instance);
            }

            this._instances = null;
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Get a new available instance and throw the <see cref="OnNewInstance(T)"/> event.
        /// </summary>
        /// <returns>Return the first available instance. If not has available instance, return null.</returns>
        public T GetNewInstance()
        {
            if (this.Availables > 0)
            {
                for (int i = 0; i < this.Total; i++)
                {
                    if (!this[i].gameObject.activeSelf)
                    {
                        this[i].gameObject.SetActive(true);

                        this.OnNewInstance(this[i]);
                        StartCoroutine(this.TerminateInstance(this[i]));

                        return this[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Reset all instances and make them availables.
        /// </summary>
        public void RestoreAllInstances()
        {
            foreach (var instance in this)
            {
                instance.gameObject.SetActive(false);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_instances).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)_instances).GetEnumerator();
        }
        #endregion

        #region Coroutines
        public abstract IEnumerator TerminateInstance(T instance); 
        #endregion

        #region Event listeners
        public abstract void OnNewInstance(T instance); 
        #endregion
    }
}
