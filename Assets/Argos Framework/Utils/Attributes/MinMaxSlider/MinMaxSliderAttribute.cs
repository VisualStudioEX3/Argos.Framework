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
        public MinMaxSliderAttribute(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }
        #endregion
    }

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