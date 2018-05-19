using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Wait for a desired number of frames.
    /// </summary>
    public class WaitForEndFrames : CustomYieldInstruction
    {
        #region Internal vars
        int _frames;
        float _lastDeltaTime;
        #endregion

        #region Properties
        public override bool keepWaiting
        {
            get
            {
                if (this._lastDeltaTime != Time.deltaTime)
                {
                    this._lastDeltaTime = Time.deltaTime;
                    this._frames--;
                }
                return this._frames == 0;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="frames">Frames to wait.</param>
        public WaitForEndFrames(int frames)
        {
            this._frames = frames;
            this._lastDeltaTime = Time.deltaTime;
        } 
        #endregion
    }

}