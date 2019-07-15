using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Singleton base class for standard classes.
    /// </summary>
    /// <typeparam name="T">Type of the class.</typeparam>
    public abstract class Singleton<T> where T : new()
    {
        #region Static members
        static T _instance;
        #endregion

        #region Properties
        public static T Instance
        {
            get
            {
                if (Singleton<T>._instance == null)
                {
                    Singleton<T>._instance = new T();
                }

                return Singleton<T>._instance;
            }
        }
        #endregion
    }

    /// <summary>
    /// Disposable singleton base class for standard classes.
    /// </summary>
    /// <typeparam name="T">Type of the class.</typeparam>
    /// <remarks>This base class requires that <see cref="T"/> implemented the <see cref="IDisposable"/> interface.</remarks>
    public abstract class DisposableSingleton<T> where T : class, IDisposable, new()
    {
        #region Static members
        static T _instance;
        static bool _disposed;
        #endregion

        #region Properties
        public static T Instance
        {
            get
            {
                if (DisposableSingleton<T>._instance == null)
                {
                    DisposableSingleton<T>._instance = new T();
                    DisposableSingleton<T>._disposed = false;
                }

                return DisposableSingleton<T>._instance;
            }
        }
        #endregion

        #region Constructors & Destructors
        ~DisposableSingleton()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Finalize and release the resources of this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">Tell the method to release all managed resources.</param>
        protected void Dispose(bool disposing)
        {
            if (!DisposableSingleton<T>._disposed)
            {
                if (disposing)
                {
                    this.OnDisposingManagedResources(); 
                }

                this.OnDisposingUnmanagedResources();

                DisposableSingleton<T>._disposed = true;
            }

            DisposableSingleton<T>._instance = null;
        }
        #endregion

        #region Event listeners
        /// <summary>
        /// Override this event to release all managed resources when the class call <see cref="Dispose"/> method.
        /// </summary>
        public abstract void OnDisposingManagedResources();

        /// <summary>
        /// Override this event to release all unmanaged resources when the class call <see cref="Dispose"/> method.
        /// </summary>
        public abstract void OnDisposingUnmanagedResources();
        #endregion
    }
}
