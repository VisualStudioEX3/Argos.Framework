using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Input
{
    [CustomEditor(typeof(ArgosStandaloneInputModule))]
    public class ArgosStandaloneInputModuleEditor : Editor
    {
        #region Constants
        const string INPUT_MAP_PROPERTY = "_inputMap";
        const string INPUT_MAP_LIST_PROPERTY = "_inputMaps";
        const string NAME_PROPERTY = "Name";
        const string DATA_PROPERTY = "Data";
        const string INPUT_MAP_AXES_LIST_PROPERTY = "_axes";
        const string INPUT_MAP_ACTIONS_LIST_PROPERTY = "_actions";

        const string NAVIGATION_PROPERTY = "_navigation";

        const string SUBMIT_PROPERTY = "_submit";
        const string CANCEL_PROPERTY = "_cancel";
        const string DELETE_PROPERTY = "_delete";
        const string DEFAULT_PROPERTY = "_default";

        const string ON_SUBMIT_EVENT_PROPERTY = "_onSubmit";
        const string ON_CANCEL_EVENT_PROPERTY = "_onCancel";
        const string ON_DELETE_EVENT_PROPERTY = "_onDelete";
        const string ON_SET_DEFAULTS_EVENT_PROPERTY = "_onSetToDefault";

        const string HELPBOX_MESSAGE = "Argos Input Manager not found on scene.";

        const string NAVIGATION_AXIS_LABEL = "Navigation axis";
        const string SUBMIT_ACTION_LABEL = "Submit action";
        const string CANCEL_ACTION_LABEL = "Cancel action";
        const string DELETE_ACTION_LABEL = "Delete action";
        const string SET_TO_DEFAULT_ACTION_LABEL = "Set to default action";
        #endregion

        #region Internal vars
        InputManager _inputManager;
        SerializedProperty _serializedInputMaps;
        InputManager.InputMapData[] _inputMaps;
        string[] _inputMapNames = new string[0];
        string[] _axesNames = new string[0];
        string[] _actionsNames = new string[0];
        SerializedProperty _inputMapSelected;
        SerializedProperty _navigation;
        SerializedProperty _submit, _cancel, _setToDefault, _delete;
        SerializedProperty _onSubmit, _onCancel, _onSetToDefault, _onDelete;
        #endregion

        #region Events
        private void OnEnable()
        {
            this.GetLocalSerializedProperties();
            this.GetInputMapsFromInputManagerInstance();
            this.UpdateArrayNames();
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            {
                if (this._inputManager)
                {
                    EditorGUILayout.Space();
                    if (this.DrawFieldPopup(string.Empty, this._inputMapSelected, this._inputMapNames))
                    {
                        this.UpdateArrayNames();
                    }

                    EditorGUI.indentLevel++;
                    {
                        this.DrawFieldPopup(ArgosStandaloneInputModuleEditor.NAVIGATION_AXIS_LABEL, this._navigation, this._axesNames);
                        this.DrawFieldPopup(ArgosStandaloneInputModuleEditor.SUBMIT_ACTION_LABEL, this._submit, this._actionsNames);
                        this.DrawFieldPopup(ArgosStandaloneInputModuleEditor.CANCEL_ACTION_LABEL, this._cancel, this._actionsNames);
                        this.DrawFieldPopup(ArgosStandaloneInputModuleEditor.DELETE_ACTION_LABEL, this._delete, this._actionsNames);
                        this.DrawFieldPopup(ArgosStandaloneInputModuleEditor.SET_TO_DEFAULT_ACTION_LABEL, this._setToDefault, this._actionsNames);
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(this._onSubmit);
                    EditorGUILayout.PropertyField(this._onCancel);
                    EditorGUILayout.PropertyField(this._onDelete);
                    EditorGUILayout.PropertyField(this._onSetToDefault);
                }
                else
                {
                    EditorGUILayout.HelpBox(ArgosStandaloneInputModuleEditor.HELPBOX_MESSAGE, MessageType.Error);
                }
            }
            this.serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Methods & Functions
        void GetLocalSerializedProperties()
        {
            this._inputMapSelected = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.INPUT_MAP_PROPERTY);

            this._navigation = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.NAVIGATION_PROPERTY);

            this._submit = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.SUBMIT_PROPERTY);
            this._cancel = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.CANCEL_PROPERTY);
            this._delete = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.DELETE_PROPERTY);
            this._setToDefault = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.DEFAULT_PROPERTY);

            this._onSubmit = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.ON_SUBMIT_EVENT_PROPERTY);
            this._onCancel = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.ON_CANCEL_EVENT_PROPERTY);
            this._onDelete = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.ON_DELETE_EVENT_PROPERTY);
            this._onSetToDefault = this.serializedObject.FindProperty(ArgosStandaloneInputModuleEditor.ON_SET_DEFAULTS_EVENT_PROPERTY);
        }

        void GetInputMapsFromInputManagerInstance()
        {
            this._inputManager = GameObject.FindObjectOfType<InputManager>();

            if (this._inputManager)
            {
                this._serializedInputMaps = new SerializedObject(this._inputManager).FindProperty(ArgosStandaloneInputModuleEditor.INPUT_MAP_LIST_PROPERTY);

                this._inputMaps = new InputManager.InputMapData[this._serializedInputMaps.arraySize];
                for (int i = 0; i < this._inputMaps.Length; i++)
                {
                    this._inputMaps[i].Name = this._serializedInputMaps.GetArrayElementAtIndex(i).FindPropertyRelative(ArgosStandaloneInputModuleEditor.NAME_PROPERTY).stringValue;
                    this._inputMaps[i].Data = (InputMapAsset)this._serializedInputMaps.GetArrayElementAtIndex(i).FindPropertyRelative(ArgosStandaloneInputModuleEditor.DATA_PROPERTY).objectReferenceValue;
                }

                this._inputMapNames = new string[this._inputMaps.Length];
                for (int i = 0; i < this._inputMapNames.Length; i++)
                {
                    this._inputMapNames[i] = this._inputMaps[i].Name;
                }
            }
        }

        void UpdateArrayNames()
        {
            this.FillArrayNamesFromSelectedInputMap(ref this._axesNames, ArgosStandaloneInputModuleEditor.INPUT_MAP_AXES_LIST_PROPERTY);
            this.FillArrayNamesFromSelectedInputMap(ref this._actionsNames, ArgosStandaloneInputModuleEditor.INPUT_MAP_ACTIONS_LIST_PROPERTY);
        }

        void FillArrayNamesFromSelectedInputMap(ref string[] array, string serializedPropertyArrayName)
        {
            if (this._inputManager)
            {
                for (int i = 0; i < this._inputMaps.Length; i++)
                {
                    if (this._inputMaps[i].Name == this._inputMapSelected.stringValue)
                    {
                        var serializedAxes = new SerializedObject(this._inputMaps[i].Data).FindProperty(serializedPropertyArrayName);
                        array = new string[serializedAxes.arraySize];
                        for (int j = 0; j < array.Length; j++)
                        {
                            array[j] = serializedAxes.GetArrayElementAtIndex(j).FindPropertyRelative(ArgosStandaloneInputModuleEditor.NAME_PROPERTY).stringValue;
                        }
                        return;
                    }
                }
            }
        }

        bool DrawFieldPopup(string label, SerializedProperty field, string[] values)
        {
            int index; for (index = 0; index < values.Length; index++)
            {
                if (values[index] == field.stringValue) break;
            }

            string previous = field.stringValue;

            index = EditorGUILayout.Popup(string.IsNullOrEmpty(label) ? field.displayName : label, index, values);

            if (index >= values.Length)
            {
                index = 0;
            }

            if (values.Length > 0)
            {
                field.stringValue = values[index];
            }

            return previous != field.stringValue;
        }
        #endregion
    } 
}