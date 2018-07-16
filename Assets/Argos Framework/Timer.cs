using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Simple timer for manage time intervals in scaled or unscaled time and in editor mode.
    /// </summary>
    public sealed class Timer
    {
        #region Enums
        /// <summary>
        /// Timer behaviour modes.
        /// </summary>
        public enum TimerModes
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
            /// Use Time.realtimeSinceStartup. This is not affected by Time.timeScale.
            /// </summary>
            EditorMode
        }

        /// <summary>
        /// Timer behaviour states.
        /// </summary>
        public enum TimerStates
        {
            Running,
            Paused,
            Stopped
        }
        #endregion

        #region Internal vars
        float _startTime;
        float _pauseDelta;
        #endregion

        #region Properties
        /// <summary>
        /// Timer behaviour mode.
        /// </summary>
        public TimerModes BehaviourMode { get; private set; }

        /// <summary>
        /// Current behaviour state.
        /// </summary>
        public TimerStates CurrentState { get; private set; }

        /// <summary>
        /// Return the current time value.
        /// </summary>
        public float Value
        {
            get
            {
                switch (this.CurrentState)
                {
                    case TimerStates.Running:

                        return this.GetCurrentTime() - this._startTime;

                    case TimerStates.Paused:

                        return this._pauseDelta - this._startTime;

                    default:

                        return 0f;
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create new Timer instance.
        /// </summary>
        /// <param name="behaviourMode">Timer behaviour mode. ScaledTime by default.</param>
        /// <param name="autoStart">Auto start the timer after instantiate it. False by default.</param>
        public Timer(TimerModes behaviourMode = TimerModes.ScaledTime, bool autoStart = false)
        {
            this.BehaviourMode = behaviourMode;
            this.Reset(autoStart);
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Start()
        {
            if (this.CurrentState == TimerStates.Stopped)
            {
                this.Reset(true); 
            }
        }

        /// <summary>
        /// Reset the timer.
        /// </summary>
        /// <param name="autoStart">Auto start the timer after reset.</param>
        public void Reset(bool autoStart = false)
        {
            this.CurrentState = autoStart ? TimerStates.Running : TimerStates.Stopped;
            this._pauseDelta = 0f;
            this._startTime = this.GetCurrentTime();
        }

        /// <summary>
        /// Pause the timer.
        /// </summary>
        public void Pause()
        {
            if (this.CurrentState == TimerStates.Running)
            {
                this.CurrentState = TimerStates.Paused;
                this._pauseDelta = this.GetCurrentTime();
            }
        }

        /// <summary>
        /// Resume the timer.
        /// </summary>
        public void Resume()
        {
            if (this.CurrentState == TimerStates.Paused)
            {
                this.CurrentState = TimerStates.Running;
                this._startTime += this.GetCurrentTime() - this._pauseDelta;
            }
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void Stop()
        {
            if (this.CurrentState != TimerStates.Stopped)
            {
                this.Reset(false);
            }
        }

        /// <summary>
        /// Get the right time value for the current mode.
        /// </summary>
        /// <returns>Return the time current value for the current behaviour mode.</returns>
        float GetCurrentTime()
        {
            switch (this.BehaviourMode)
            {
                case TimerModes.UnScaledTime:

                    return Time.unscaledTime;

                case TimerModes.EditorMode:

                    return Time.realtimeSinceStartup;

                default:

                    return Time.time;
            }
        }
#endregion
    }
}
