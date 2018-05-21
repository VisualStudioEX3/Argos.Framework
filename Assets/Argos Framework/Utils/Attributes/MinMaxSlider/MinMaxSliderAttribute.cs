using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        #region Public vars
        public float Min, Max;
        #endregion

        #region Constructor
        public MinMaxSliderAttribute(float min = 0f, float max = 1f)
        {
            this.Min = min;
            this.Max = max;
        }
        #endregion
    }

    /// <summary>
    /// MinMaxSlider type struct.
    /// </summary>
    /// <remarks>Use this to create MinMaxSlider variables.</remarks>
    [Serializable]
    public struct MinMaxSlider
    {
        #region Public vars
        public float Min, Max;
        #endregion

        #region Constructor
        public MinMaxSlider(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }
        #endregion
    } 
}