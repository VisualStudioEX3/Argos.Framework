using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Argos.Framework.Input
{
    [CustomEditor(typeof(InputManager))]
    public class InputManagerEditor : Editor
    {
        #region Constants
        const string HEADER_NAME = "Input Maps";
        const string PROPERTY_NAME = "_inputMaps";
        const string PREFIX_NAME = "Input Map";

        const string HELPBOX_MESSAGE = "For the right behaviour of the Argos Input Manager, the Unity input settings must be setup first with the Argos input axes predefined values.";
        const string BUTTON_LABEL = "Setup Unity input settings";

        const string DIALOG_TITLE = "Warning!";
        const string DIALOG_MESSAGE = "This action will delete the current Unity input settings axes values. Are you sure?";
        const string DIALOG_OK = "Yes, proceed";
        const string DIALOG_CANCEL = "No, cancel";

        public const string INPUT_MAP_ARRAY_KEY = "Argos.Framework.Temp.InputManager.InputMaps";
        #endregion

        #region Internal vars
        ReorderableList _inputMapList;
        #endregion

        #region Event listeners
        private void OnEnable()
        {
            this._inputMapList = ReorderableListHelper.CreateNamedList(this, this._inputMapList, InputManagerEditor.HEADER_NAME, InputManagerEditor.PROPERTY_NAME, InputManagerEditor.PREFIX_NAME, false);
        }

        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspectorWithoutScriptField();

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(InputManagerEditor.HELPBOX_MESSAGE, MessageType.Info);
            GUI.enabled = !Application.isPlaying;
            if (GUILayout.Button(InputManagerEditor.BUTTON_LABEL, GUILayout.Height(32f)))
            {
                if (EditorUtility.DisplayDialog(InputManagerEditor.DIALOG_TITLE, InputManagerEditor.DIALOG_MESSAGE, InputManagerEditor.DIALOG_OK, InputManagerEditor.DIALOG_CANCEL))
                {
                    UnityInputManagerAsset.SetupInputAxes();
                }
            }

            EditorGUILayout.Space();

            this.serializedObject.Update();
            this._inputMapList.DoLayoutList();
            this.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
        }
        #endregion
    } 
}
