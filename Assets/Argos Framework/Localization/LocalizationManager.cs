using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;
using Argos.Framework.Utils;

namespace Argos.Framework.Localization
{
    /// <summary>
    /// Localization text string manager.
    /// </summary>
    [AddComponentMenu("Argos.Framework/Localization/Localization Manager")]
    public sealed class LocalizationManager : MonoBehaviour
    {
        [Serializable]
        public struct TestData
        {
            public string key;
            public string text;
            public KeyCode keyCode;
            public bool enable;
            [Range(0, 10)]
            public int value;
            [ColorUsage(true, true)]
            public Color color;
            public AnimationCurve curve;
            public float threshold;
        }

        [SerializeField]
        TestData[] _test;

        //[TexturePreview]
        //public Texture texture;
    }
}