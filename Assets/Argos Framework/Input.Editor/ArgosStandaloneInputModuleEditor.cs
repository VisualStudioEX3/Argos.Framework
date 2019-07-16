using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Input
{
    [CustomEditor(typeof(ArgosStandaloneInputModule))]
    public class ArgosStandaloneInputModuleEditor : Editor
    {
        #region Constants
        const string INPUT_MAP_PROPERTY = "_inputMap";

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
        string[] _inputMapNames = new string[0];
        string[] _axesNames = new string[0];
        string[] _actionsNames = new string[0];

        SerializedProperty _inputMapSelected;
        SerializedProperty _navigation;
        SerializedProperty _submit, _cancel, _setToDefault, _delete;
        SerializedProperty _onSubmit, _onCancel, _onSetToDefault, _onDelete;
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
            if (InputManager.Instance)
            {
                this._inputMapNames = InputManager.Instance.InputMaps.Keys.ToArray();
            }
        }

        void UpdateArrayNames()
        {
            InputManager.EditorInstance.InputMaps.SetDirty();

            if (InputManager.EditorInstance.InputMaps.Count > 0)
            {
                this._inputMapNames = InputManager.EditorInstance.InputMaps.Keys.ToArray();

                if (!string.IsNullOrEmpty(this._inputMapSelected.stringValue))
                {
                    var map = InputManager.EditorInstance.InputMaps[this._inputMapSelected.stringValue];
                    map.SetDirty();

                    this._axesNames = map.Axes.Keys.ToArray();
                    this._actionsNames = map.Actions.Keys.ToArray();
                }
                else
                {
                    this._axesNames = new string[0];
                    this._actionsNames = new string[0];
                }
            }
        }

        bool DrawFieldPopup(string label, SerializedProperty field, string[] values)
        {
            GUI.enabled = values.Length > 0;

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

            GUI.enabled = true;

            return previous != field.stringValue;
        }
        #endregion

        #region Event listeners
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
                if (InputManager.EditorInstance)
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
    }
}