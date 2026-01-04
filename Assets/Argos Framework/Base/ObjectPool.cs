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
        [SerializeField, HelpBox("You can setup different prefabs to randomly instantiate in the object pool.", HelpBoxMessageType.Info)]
        T[] _prefabs;
        [SerializeField]
        int _maxInstances;
        [SerializeField, HelpBox("The parent for all instances. If the parent is null, this game object is the parent.", HelpBoxMessageType.Info)]
        Transform _parent;
#pragma warning restore
        #endregion

        #region Properties
        /// <summary>
        /// Total instances in this object pool.
        /// </summary>
        public int Total { get { return this._instances.Length; } }

        /// <summary>
        /// Active instances in this object pool.
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
        /// Available instances in this object pool.
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
            StopAllCoroutines();

            foreach (var instance in this)
            {
                Destroy(instance);
            }

            this._instances = null;
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Get a new available instance.
        /// </summary>
        /// <returns>Returns the first available instance. If not has available instance, returns null.</returns>
        public T GetNewInstance()
        {
            if (this.Availables > 0)
            {
                for (int i = 0; i < this.Total; i++)
                {
                    if (!this[i].gameObject.activeSelf)
                    {
                        this[i].gameObject.SetActive(true);
                        return this[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get a new available instance with life time to disable it.
        /// </summary>
        /// <param name="lifeTime">The life time of this instance.</param>
        /// <returns>Returns the first available instance. If not has available instance, returns null.</returns>
        public T GetNewInstance(float lifeTime)
        {
            return this.GetNewInstance(lifeTime, null);
        }

        /// <summary>
        /// Get a new available instance with condition to disable it.
        /// </summary>
        /// <param name="condition">Condition to disable the instance after life time delay passed.</param>
        /// <returns>Returns the first available instance. If not has available instance, returns null.</returns>
        public T GetNewInstance(Func<bool> condition)
        {
            return this.GetNewInstance(-1, condition);
        }

        /// <summary>
        /// Get a new available instance with life time and condition to disable it.
        /// </summary>
        /// <param name="lifeTime">The life time of this instance.</param>
        /// <param name="condition">Condition to disable the instance after life time delay passed.</param>
        /// <returns>Returns the first available instance. If not has available instance, returns null.</returns>
        public T GetNewInstance(float lifeTime, Func<bool> condition)
        {
            T instance = this.GetNewInstance();
            if (instance)
            {
                if (lifeTime > 0f && condition == null)
                {
                    StartCoroutine(this.DisableInstanceCoroutine(instance, lifeTime));
                }
                else if (lifeTime < 0f && condition != null)
                {
                    StartCoroutine(this.DisableInstanceCoroutine(instance, condition));
                }
                else if (lifeTime > 0f && condition != null)
                {
                    StartCoroutine(this.DisableInstanceCoroutine(instance, lifeTime, condition));
                }
            }
            return instance;
        }

        /// <summary>
        /// Get a new available instance with custom coroutine to disable it.
        /// </summary>
        /// <param name="conditionRoutine">Coroutine to disable instance.</param>
        /// <returns>Returns the first available instance. If not has available instance, returns null.</returns>
        public T GetNewInstance(IEnumerator conditionRoutine)
        {
            T instance = this.GetNewInstance();
            if (instance)
            {
                StartCoroutine(conditionRoutine); 
            }
            return instance;
        }

        /// <summary>
        /// Reset all instances and make them availables.
        /// </summary>
        public void RestoreAllInstances()
        {
            StopAllCoroutines();

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
        IEnumerator DisableInstanceCoroutine(T instance, float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            instance.gameObject.SetActive(false);
        }

        IEnumerator DisableInstanceCoroutine(T instance, Func<bool> condition)
        {
            yield return new WaitUntil(condition);
            instance.gameObject.SetActive(false);
        }

        IEnumerator DisableInstanceCoroutine(T instance, float lifeTime, Func<bool> condition)
        {
            var timer = new Timer();

            yield return new WaitWhile(() => { return timer.Value >= lifeTime || condition.Invoke(); });

            instance.gameObject.SetActive(false);
        }
        #endregion
    }
}
