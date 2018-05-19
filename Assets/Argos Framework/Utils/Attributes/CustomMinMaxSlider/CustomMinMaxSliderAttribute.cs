using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    public class CustomMinMaxSliderAttribute : PropertyAttribute
    {
        #region Public vars
        public float Min, Max;
        #endregion

        #region Constructor
        public CustomMinMaxSliderAttribute(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }
        #endregion
    }

    [Serializable]
    public struct CustomMinMaxSlider
    {
        #region Public vars
        public float Min, Max;
        #endregion

        #region Constructor
        public CustomMinMaxSlider(float min, float max)
        {
            Min = min;
            Max = max;
        }
        #endregion
    } 
}