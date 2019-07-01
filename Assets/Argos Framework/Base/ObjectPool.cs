using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    public abstract class ObjectPool<T> : IDisposable, IEnumerable<T> where T : Component
    {
        #region Internal vars
        T[] _prefabs;
        T[] _instances;
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

        #region Events
        event Action<T> onNewInstance;
        #endregion

        #region Constructors & Destructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="prefab">Prefab used as model to create instances.</param>
        /// <param name="maxInstances">Max instances created by this object pool.</param>
        /// <param name="onNewInstance">Event listener where customize the instance initialization.</param>
        /// <param name="parent">Optional. Set the parent of all instances. By default is null (not parent assigned).</param>
        public ObjectPool(T prefab, int maxInstances, Action<T> onNewInstance, Transform parent = null) 
            : this(new T[1] { prefab }, maxInstances, onNewInstance, parent)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="prefabs">Prefabs used as model to create instances.</param>
        /// <param name="maxInstances">Max instances created by this object pool.</param>
        /// <param name="onNewInstance">Event listener where customize the instance initialization.</param>
        /// <param name="parent">Optional. Set the parent of all instances. By default is null (not parent assigned).</param>
        public ObjectPool(T[] prefabs, int maxInstances, Action<T> onNewInstance, Transform parent = null)
        {
            this._prefabs = prefabs;
            this._instances = new T[maxInstances];

            for (int i = 0; i < this.Total; i++)
            {
                this[i] = UnityEngine.Object.Instantiate<T>(this._prefabs[UnityEngine.Random.Range(0, maxInstances)], Vector3.zero, Quaternion.identity, parent);
                this[i].gameObject.SetActive(false);
            }

            this.onNewInstance = onNewInstance;
        }

        public void Dispose()
        {
            foreach (var instance in this)
            {
                UnityEngine.Object.Destroy(instance);
            }

            this._instances = null;
            this.onNewInstance = null;
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Get a new available instance and run the <see cref="onNewInstance"/> event.
        /// </summary>
        /// <returns>Return the first available instance. If not has available instance, return null.</returns>
        public T GetNewInstance(IEnumerator terminationCoroutine = null)
        {
            if (this.Availables > 0)
            {
                for (int i = 0; i < this.Total; i++)
                {
                    if (!this[i].gameObject.activeSelf)
                    {
                        this[i].gameObject.SetActive(true);
                        this.onNewInstance?.Invoke(this[i]);

                        // TODO: Fix how to call a coroutine from here! https://answers.unity.com/questions/1260116/tartcoroutine-cant-be-called-from-a-gameobject-am.html?childToView=1260187#comment-1260187
                        //this[i].GetComponent<T>()

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
    } 
}
