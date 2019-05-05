using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 
#endif

namespace Argos.Framework.Input
{
    /// <summary>
    /// Interface class to access and modify the Unity Input Manager asset properties.
    /// </summary>
    /// <remarks>Code implementation based on this article: http://plyoung.appspot.com/blog/manipulating-input-manager-in-script.html </remarks>
    public static class UnityInputManagerAsset
    {
#if UNITY_EDITOR
        #region Enums
        enum UnityAxisType
        {
            KeyOrMouseButton = 0,
            MouseMovement = 1,
            JoystickAxis = 2
        };
        #endregion

        #region Structs
#pragma warning disable 0649
        struct UnityInputAxis
        {
            public string name;
            public string descriptiveName;
            public string descriptiveNegativeName;
            public string negativeButton;
            public string positiveButton;
            public string altNegativeButton;
            public string altPositiveButton;

            public float gravity;
            public float dead;
            public float sensitivity;

            public bool snap;
            public bool invert;

            public UnityAxisType type;

            public int axis;
            public int joyNum;
        }
#pragma warning restore
        #endregion

        #region Internal vars
        static SerializedObject _serializedObject;
        static SerializedProperty _axesProperty;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Setup the input axes used by Argos Input system.
        /// </summary>
        /// <remarks>Access the Unity Input Manager asset, from Project Settings folder, clear the existing axes definitions, and add the Argos axes definitions.</remarks>
        public static void SetupInputAxes()
        {
            Debug.Log("Access /ProjectSettings/InputManager.asset...");
            UnityInputManagerAsset._serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            UnityInputManagerAsset._axesProperty = UnityInputManagerAsset._serializedObject.FindProperty("m_Axes");

            Debug.Log("Remove old axes definitions...");
            UnityInputManagerAsset._axesProperty.ClearArray();

            Debug.Log("Add custom axes definitions...");
            UnityInputManagerAsset.SetupAxes();

            UnityInputManagerAsset._serializedObject.ApplyModifiedProperties();

            Debug.Log("/ProjectSettings/InputManager.asset is updated!");
        }

        static void AddAxis(UnityInputAxis axis)
        {
            UnityInputManagerAsset._axesProperty.arraySize++;
            UnityInputManagerAsset._serializedObject.ApplyModifiedProperties();

            SerializedProperty axisProperty = UnityInputManagerAsset._axesProperty.GetArrayElementAtIndex(UnityInputManagerAsset._axesProperty.arraySize - 1);

            UnityInputManagerAsset.GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
            UnityInputManagerAsset.GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

            UnityInputManagerAsset._serializedObject.ApplyModifiedProperties();
        }

        static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do
            {
                if (child.name == name) return child;
            }
            while (child.Next(false));
            return null;
        }

        static void SetupAxes()
        {
            // Add mouse definitions:
            UnityInputManagerAsset.AddAxis(new UnityInputAxis() { name = "Mouse X", sensitivity = 0.1f, type = UnityAxisType.MouseMovement, axis = 1 });
            UnityInputManagerAsset.AddAxis(new UnityInputAxis() { name = "Mouse Y", sensitivity = 0.1f, type = UnityAxisType.MouseMovement, axis = 2 });
            UnityInputManagerAsset.AddAxis(new UnityInputAxis() { name = "Mouse ScrollWheel", sensitivity = 0.1f, type = UnityAxisType.MouseMovement, axis = 3 });

            // Add gamepad definitions:
            for (int i = 0; i < 10; i++)
            {
                UnityInputManagerAsset.AddAxis(new UnityInputAxis()
                {
                    name = $"Gamepad {i + 1} Axis",
                    dead = 0.2f,
                    sensitivity = 1f,
                    type = UnityAxisType.JoystickAxis,
                    axis = (i + 1)
                });
            }
        }
        #endregion
#endif
    }
}