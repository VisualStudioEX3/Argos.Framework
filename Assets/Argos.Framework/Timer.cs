using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Simple timer for manage time scaled intervals.
    /// </summary>
    public class Timer
    {
        #region Internal vars
        private float startTime;
        #endregion

        #region Properties
        /// <summary>
        /// Return the current time value.
        /// </summary>
        public float Value { get { return Time.time - startTime; } }
        #endregion

        #region Constructor
        public Timer()
        {
            this.Reset();
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Reset the timer value.
        /// </summary>
        public void Reset()
        {
            startTime = Time.time;
        } 
        #endregion
    }
}
