using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace Argos.Framework
{
    /// <summary>
    /// Simple timer for manage time intervals in scaled or unscaled time and in editor mode.
    /// </summary>
    public sealed class Timer
    {
        #region Enums
        public enum TimerMode
        {
            /// <summary>
            /// Use Time.time value. This is affected by Time.timeScale.
            /// </summary>
            ScaledTime,
            /// <summary>
            /// Use Time.unscaledTime value. This is not affected by Time.timeScale.
            /// </summary>
            UnScaledTime,
            /// <summary>
            /// For script editors. Use System.Diagnostics.Stopwatch value.
            /// </summary>
            EditorMode
        } 
        #endregion

        #region Internal vars
        float _startTime;
        float _pauseDelta;
        bool _isPaused;
        Stopwatch _stopwatch;
        #endregion

        #region Properties
        public TimerMode Mode { get; private set; }

        /// <summary>
        /// Return the current time value.
        /// </summary>
        public float Value
        {
            get
            {
                switch (this.Mode)
                {
                    case TimerMode.UnScaledTime:

                        return this._isPaused ? this._pauseDelta : Time.unscaledTime - _startTime;

                    case TimerMode.EditorMode:

                        return this.GetStopWatchInSeconds();

                    default:

                        return this._isPaused ? this._pauseDelta : Time.time - _startTime;
                }
            }
        }

        /// <summary>
        /// Is the Timer paused?
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return this._isPaused;
            }
        }
        #endregion

        #region Constructor
        public Timer(TimerMode mode = TimerMode.ScaledTime)
        {
            this.Mode = mode;

            if (this.Mode == TimerMode.EditorMode)
            {
                this._stopwatch = new Stopwatch();
            }

            this.Reset();
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Reset the timer value.
        /// </summary>
        public void Reset()
        {
            this._isPaused = false;

            switch (this.Mode)
            {
                case TimerMode.ScaledTime:

                    this._startTime = Time.time;
                    break;

                case TimerMode.UnScaledTime:

                    this._startTime = Time.unscaledTime;
                    break;

                case TimerMode.EditorMode:

                    this._stopwatch.Reset();
                    this._stopwatch.Start();
                    break;
            }
        }

        /// <summary>
        /// Pause the timer.
        /// </summary>
        public void Pause()
        {
            this._isPaused = true;

            switch (this.Mode)
            {
                case TimerMode.ScaledTime:

                    this._pauseDelta = Time.time;
                    break;

                case TimerMode.UnScaledTime:

                    this._pauseDelta = Time.unscaledTime;
                    break;

                case TimerMode.EditorMode:

                    this._stopwatch.Stop();
                    break;
            }
        }

        /// <summary>
        /// Resume the timer.
        /// </summary>
        public void Resume()
        {
            this._isPaused = false;
            switch (this.Mode)
            {
                case TimerMode.ScaledTime:

                    this._startTime += Time.time - this._pauseDelta;
                    break;

                case TimerMode.UnScaledTime:

                    this._startTime += Time.unscaledTime - this._pauseDelta;
                    break;

                case TimerMode.EditorMode:

                    this._stopwatch.Start();
                    break;
            }
        }

        /// <summary>
        /// Reset and pause the timer.
        /// </summary>
        public void Stop()
        {
            this.Reset();
            this.Pause();
        }

        float GetStopWatchInSeconds()
        {
            return (float)(this._stopwatch.ElapsedMilliseconds / 1000f);
        }
        #endregion
    }
}
