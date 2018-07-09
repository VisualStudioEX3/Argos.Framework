#if UNITY_EDITOR
using System.Collections;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// Coroutines for Editor scripts, just like regular coroutines.
    /// </summary>
    /// <remarks>Original source: https://github.com/marijnz/unity-editor-coroutines </remarks>
    public static class EditorCoroutineExtensions
    {
        #region Methods & Functions
        public static EditorCoroutine StartCoroutine(this Editor thisRef, IEnumerator coroutine)
        {
            return EditorCoroutines.StartCoroutine(coroutine, thisRef);
        }

        public static EditorCoroutine StartCoroutine(this EditorWindow thisRef, IEnumerator coroutine)
        {
            return EditorCoroutines.StartCoroutine(coroutine, thisRef);
        }

        public static EditorCoroutine StartCoroutine(this Editor thisRef, string methodName)
        {
            return EditorCoroutines.StartCoroutine(methodName, thisRef);
        }

        public static EditorCoroutine StartCoroutine(this EditorWindow thisRef, string methodName)
        {
            return EditorCoroutines.StartCoroutine(methodName, thisRef);
        }

        public static EditorCoroutine StartCoroutine(this Editor thisRef, string methodName, object value)
        {
            return EditorCoroutines.StartCoroutine(methodName, value, thisRef);
        }

        public static EditorCoroutine StartCoroutine(this EditorWindow thisRef, string methodName, object value)
        {
            return EditorCoroutines.StartCoroutine(methodName, value, thisRef);
        }

        public static void StopCoroutine(this Editor thisRef, IEnumerator coroutine)
        {
            EditorCoroutines.StopCoroutine(coroutine, thisRef);
        }

        public static void StopCoroutine(this EditorWindow thisRef, IEnumerator coroutine)
        {
            EditorCoroutines.StopCoroutine(coroutine, thisRef);
        }

        public static void StopCoroutine(this Editor thisRef, EditorCoroutine coroutine)
        {
            EditorCoroutines.StopCoroutine(coroutine.routine, thisRef);
        }

        public static void StopCoroutine(this EditorWindow thisRef, EditorCoroutine coroutine)
        {
            EditorCoroutines.StopCoroutine(coroutine.routine, thisRef);
        }

        public static void StopCoroutine(this Editor thisRef, string methodName)
        {
            EditorCoroutines.StopCoroutine(methodName, thisRef);
        }

        public static void StopCoroutine(this EditorWindow thisRef, string methodName)
        {
            EditorCoroutines.StopCoroutine(methodName, thisRef);
        }

        public static void StopAllCoroutines(this Editor thisRef)
        {
            EditorCoroutines.StopAllCoroutines(thisRef);
        }

        public static void StopAllCoroutines(this EditorWindow thisRef)
        {
            EditorCoroutines.StopAllCoroutines(thisRef);
        }
        #endregion
    }
}
#endif