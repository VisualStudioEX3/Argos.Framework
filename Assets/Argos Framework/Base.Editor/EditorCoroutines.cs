using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

namespace Argos.Framework
{
    #region Interfaces
    public interface IEditorCoroutineYield
    {
        #region Methods & Functions
        bool IsDone(float deltaTime); 
        #endregion
    }
    #endregion

    #region Structs
    struct EditorYieldDefault : IEditorCoroutineYield
    {
        #region Methods & Functions
        public bool IsDone(float deltaTime)
        {
            return true;
        } 
        #endregion
    }

    struct EditorYieldWaitForSeconds : IEditorCoroutineYield
    {
        #region Public vars
        public float timeLeft;
        #endregion

        #region Methods & Functions
        public bool IsDone(float deltaTime)
        {
            timeLeft -= deltaTime;
            return timeLeft < 0;
        } 
        #endregion
    }

    struct EditorYieldWWW : IEditorCoroutineYield
    {
        #region Public vars
        public UnityWebRequest Www;
        #endregion

        #region Methods & Functions
        public bool IsDone(float deltaTime)
        {
            return Www.isDone;
        } 
        #endregion
    }

    struct EditorYieldAsync : IEditorCoroutineYield
    {
        #region Public vars
        public AsyncOperation asyncOperation;
        #endregion

        #region Methods & Functions
        public bool IsDone(float deltaTime)
        {
            return asyncOperation.isDone;
        } 
        #endregion
    }

    struct EditorYieldNestedCoroutine : IEditorCoroutineYield
    {
        #region Public vars
        public EditorCoroutine coroutine;
        #endregion

        #region Methods & Functions
        public bool IsDone(float deltaTime)
        {
            return coroutine.finished;
        } 
        #endregion
    }
    #endregion

    #region Classes
    public sealed class EditorCoroutine
    {
        #region Public vars
        public IEditorCoroutineYield currentYield = new EditorYieldDefault();
        public IEnumerator routine;
        public string routineUniqueHash;
        public string ownerUniqueHash;
        public string methodName;

        public int ownerHash;
        public string ownerType;

        public bool finished = false;
        #endregion

        #region Constructors
        public EditorCoroutine(IEnumerator routine, int ownerHash, string ownerType)
        {
            this.routine = routine;
            this.ownerHash = ownerHash;
            this.ownerType = ownerType;
            this.ownerUniqueHash = $"{ownerHash}_{ownerType}";

            if (routine != null)
            {
                string[] split = routine.ToString().Split('<', '>');
                if (split.Length == 3)
                {
                    this.methodName = split[1];
                }
            }

            this.routineUniqueHash = $"{ownerHash}_{ownerType}_{methodName}";
        }

        public EditorCoroutine(string methodName, int ownerHash, string ownerType)
        {
            this.methodName = methodName;
            this.ownerHash = ownerHash;
            this.ownerType = ownerType;
            this.ownerUniqueHash = $"{ownerHash}_{ownerType}";
            this.routineUniqueHash = $"{ownerHash}_{ownerType}_{methodName}";
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// Coroutines for Editor scripts, just like regular coroutines.
    /// </summary>
    /// <remarks>Original source: https://github.com/marijnz/unity-editor-coroutines </remarks>
    public sealed class EditorCoroutines
    {
        #region Internal vars
        Dictionary<string, List<EditorCoroutine>> _coroutineDict = new Dictionary<string, List<EditorCoroutine>>();
        List<List<EditorCoroutine>> _tempCoroutineList = new List<List<EditorCoroutine>>();

        Dictionary<string, Dictionary<string, EditorCoroutine>> _coroutineOwnerDict = new Dictionary<string, Dictionary<string, EditorCoroutine>>();

        DateTime _previousTimeSinceStartup;
        #endregion

        #region Singleton
        static EditorCoroutines _instance = null;
        #endregion

        #region Static Methods & Functions
        /// <summary>
        /// Starts a coroutine.
        /// </summary>
        /// <param name="routine">The coroutine to start.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static EditorCoroutine StartCoroutine(IEnumerator routine, object thisReference)
        {
            EditorCoroutines.CreateInstanceIfNeeded();
            return _instance.GoStartCoroutine(routine, thisReference);
        }

        /// <summary>
        /// Starts a coroutine.
        /// </summary>
        /// <param name="methodName">The name of the coroutine method to start.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static EditorCoroutine StartCoroutine(string methodName, object thisReference)
        {
            return StartCoroutine(methodName, null, thisReference);
        }

        /// <summary>
        /// Starts a coroutine.
        /// </summary>
        /// <param name="methodName">The name of the coroutine method to start.</param>
        /// <param name="value">The parameter to pass to the coroutine.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static EditorCoroutine StartCoroutine(string methodName, object value, object thisReference)
        {
            MethodInfo methodInfo = thisReference.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            object returnValue;

            if (methodInfo == null)
            {
                UnityEngine.Debug.LogErrorFormat("Coroutine '{0}' couldn't be started, the method doesn't exist!", methodName);
            }

            if (value == null)
            {
                returnValue = methodInfo.Invoke(thisReference, null);
            }
            else
            {
                returnValue = methodInfo.Invoke(thisReference, new object[] { value });
            }

            if (returnValue is IEnumerator)
            {
                EditorCoroutines.CreateInstanceIfNeeded();
                return EditorCoroutines._instance.GoStartCoroutine((IEnumerator)returnValue, thisReference);
            }
            else
            {
                UnityEngine.Debug.LogErrorFormat("Coroutine '{0}' couldn't be started, the method doesn't return an IEnumerator!", methodName);
            }

            return null;
        }

        /// <summary>
        /// Stops all coroutines being the routine running on the passed instance.
        /// </summary>
        /// <param name="routine"> The coroutine to stop.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static void StopCoroutine(IEnumerator routine, object thisReference)
        {
            EditorCoroutines.CreateInstanceIfNeeded();
            EditorCoroutines._instance.GoStopCoroutine(routine, thisReference);
        }

        /// <summary>
        /// Stops all coroutines named methodName running on the passed instance.
        /// </summary>
        /// <param name="methodName"> The name of the coroutine method to stop.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static void StopCoroutine(string methodName, object thisReference)
        {
            EditorCoroutines.CreateInstanceIfNeeded();
            EditorCoroutines._instance.GoStopCoroutine(methodName, thisReference);
        }

        /// <summary>
        /// Stops all coroutines running on the passed instance.
        /// </summary>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static void StopAllCoroutines(object thisReference)
        {
            EditorCoroutines.CreateInstanceIfNeeded();
            EditorCoroutines._instance.GoStopAllCoroutines(thisReference);
        }

        static void CreateInstanceIfNeeded()
        {
            if (EditorCoroutines._instance == null)
            {
                EditorCoroutines._instance = new EditorCoroutines();
                EditorCoroutines._instance.Initialize();
            }
        }

        static bool MoveNext(EditorCoroutine coroutine)
        {
            if (coroutine.routine.MoveNext())
            {
                return EditorCoroutines.Process(coroutine);
            }

            return false;
        }

        // Returns false if no next, returns true if OK.
        static bool Process(EditorCoroutine coroutine)
        {
            object current = coroutine.routine.Current;

            if (current == null)
            {
                coroutine.currentYield = new EditorYieldDefault();
            }
            else if (current is WaitForSeconds)
            {
                float seconds = float.Parse(EditorCoroutines.GetInstanceField(typeof(WaitForSeconds), current, "m_Seconds").ToString());
                coroutine.currentYield = new EditorYieldWaitForSeconds() { timeLeft = (float)seconds };
            }
            else if (current is UnityWebRequest)
            {
                coroutine.currentYield = new EditorYieldWWW { Www = (UnityWebRequest)current };
            }
            else if (current is WaitForFixedUpdate)
            {
                coroutine.currentYield = new EditorYieldDefault();
            }
            else if (current is AsyncOperation)
            {
                coroutine.currentYield = new EditorYieldAsync { asyncOperation = (AsyncOperation)current };
            }
            else if (current is EditorCoroutine)
            {
                coroutine.currentYield = new EditorYieldNestedCoroutine { coroutine = (EditorCoroutine)current };
            }
            else
            {
                UnityEngine.Debug.LogException(new Exception($"<{coroutine.methodName}> yielded an unknown or unsupported type! ({current.GetType()})"), null);
                coroutine.currentYield = new EditorYieldDefault();
            }

            return true;
        }

        static object GetInstanceField(Type type, object instance, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            return field.GetValue(instance);
        }
        #endregion

        #region Methods & Functions
        void Initialize()
        {
            this._previousTimeSinceStartup = DateTime.Now;
            EditorApplication.update += OnUpdate;
        }

        void GoStopCoroutine(IEnumerator routine, object thisReference)
        {
            this.GoStopActualRoutine(this.CreateCoroutine(routine, thisReference));
        }

        void GoStopCoroutine(string methodName, object thisReference)
        {
            this.GoStopActualRoutine(this.CreateCoroutineFromString(methodName, thisReference));
        }

        void GoStopActualRoutine(EditorCoroutine routine)
        {
            if (this._coroutineDict.ContainsKey(routine.routineUniqueHash))
            {
                this._coroutineOwnerDict[routine.ownerUniqueHash].Remove(routine.routineUniqueHash);
                this._coroutineDict.Remove(routine.routineUniqueHash);
            }
        }

        void GoStopAllCoroutines(object thisReference)
        {
            EditorCoroutine coroutine = this.CreateCoroutine(null, thisReference);

            if (this._coroutineOwnerDict.ContainsKey(coroutine.ownerUniqueHash))
            {
                foreach (var couple in this._coroutineOwnerDict[coroutine.ownerUniqueHash])
                {
                    this._coroutineDict.Remove(couple.Value.routineUniqueHash);
                }
                this._coroutineOwnerDict.Remove(coroutine.ownerUniqueHash);
            }
        }

        EditorCoroutine GoStartCoroutine(IEnumerator routine, object thisReference)
        {
            if (routine == null)
            {
                UnityEngine.Debug.LogException(new Exception("IEnumerator is null!"), null);
            }

            EditorCoroutine coroutine = this.CreateCoroutine(routine, thisReference);
            this.GoStartCoroutine(this.CreateCoroutine(routine, thisReference));

            return coroutine;
        }

        void GoStartCoroutine(EditorCoroutine coroutine)
        {
            if (!_coroutineDict.ContainsKey(coroutine.routineUniqueHash))
            {
                this._coroutineDict.Add(coroutine.routineUniqueHash, new List<EditorCoroutine>());
            }
            this._coroutineDict[coroutine.routineUniqueHash].Add(coroutine);

            if (!this._coroutineOwnerDict.ContainsKey(coroutine.ownerUniqueHash))
            {
                this._coroutineOwnerDict.Add(coroutine.ownerUniqueHash, new Dictionary<string, EditorCoroutine>());
            }

            // If the method from the same owner has been stored before, it doesn't have to be stored anymore,
            // One reference is enough in order for "StopAllCoroutines" to work
            if (!this._coroutineOwnerDict[coroutine.ownerUniqueHash].ContainsKey(coroutine.routineUniqueHash))
            {
                this._coroutineOwnerDict[coroutine.ownerUniqueHash].Add(coroutine.routineUniqueHash, coroutine);
            }

            EditorCoroutines.MoveNext(coroutine);
        }

        EditorCoroutine CreateCoroutine(IEnumerator routine, object thisReference)
        {
            return new EditorCoroutine(routine, thisReference.GetHashCode(), thisReference.GetType().ToString());
        }

        EditorCoroutine CreateCoroutineFromString(string methodName, object thisReference)
        {
            return new EditorCoroutine(methodName, thisReference.GetHashCode(), thisReference.GetType().ToString());
        }
        #endregion

        #region Event listeners
        void OnUpdate()
        {
            float deltaTime = (float)(DateTime.Now.Subtract(_previousTimeSinceStartup).TotalMilliseconds / 1000.0f);

            this._previousTimeSinceStartup = DateTime.Now;
            if (this._coroutineDict.Count == 0)
            {
                return;
            }

            this._tempCoroutineList.Clear();
            foreach (var pair in this._coroutineDict)
            {
                this._tempCoroutineList.Add(pair.Value);
            }

            for (var i = this._tempCoroutineList.Count - 1; i >= 0; i--)
            {
                List<EditorCoroutine> coroutines = this._tempCoroutineList[i];

                for (int j = coroutines.Count - 1; j >= 0; j--)
                {
                    EditorCoroutine coroutine = coroutines[j];

                    if (!coroutine.currentYield.IsDone(deltaTime))
                    {
                        continue;
                    }

                    if (!EditorCoroutines.MoveNext(coroutine))
                    {
                        coroutines.RemoveAt(j);
                        coroutine.currentYield = null;
                        coroutine.finished = true;
                    }

                    if (coroutines.Count == 0)
                    {
                        this._coroutineDict.Remove(coroutine.ownerUniqueHash);
                    }
                }
            }
        } 
        #endregion
    }
}