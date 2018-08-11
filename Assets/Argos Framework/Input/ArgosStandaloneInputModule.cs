using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor; 
#endif
using Argos.Framework;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Custom Standalone Input Module for working with Argos.Input system.
    /// </summary>
    /// <remarks>
    /// Based on Unity StandaloneInputModule.cs, version 5.3:
    /// https://bitbucket.org/Unity-Technologies/ui/src/b5f9aae6ff7c2c63a521a1cb8b3e3da6939b191b/UnityEngine.UI/EventSystem/InputModules?at=5.3
    /// </remarks>
    [AddComponentMenu("Argos.Framework/Input/Standalone Input Module"), DisallowMultipleComponent]
    public class ArgosStandaloneInputModule : PointerInputModule
    {
        #region Internal vars
        private float m_PrevActionTime;
        private Vector2 m_LastMoveVector;
        private int m_ConsecutiveMoveCount = 0;

        private Vector2 m_LastMousePosition;
        private Vector2 m_MousePosition;

        private float m_InputActionsPerSecond = 10;
        private float m_RepeatDelay = 0.5f;
        [FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
        private bool m_ForceModuleActive;
        #endregion

        #region Inspector fields
#pragma warning disable 0649
        [SerializeField]
        string _inputMap;
        [SerializeField]
        string _navigation;
        [SerializeField]
        string _submit;
        [SerializeField]
        string _cancel;
        [SerializeField]
        string _default;
        [SerializeField]
        string _delete;
        [SerializeField]
        UnityEvent _onSubmit;
        [SerializeField]
        UnityEvent _onCancel;
        [SerializeField]
        UnityEvent _onDelete;
        [SerializeField]
        UnityEvent _onSetToDefault;
#pragma warning restore
        #endregion

        #region Properties
        [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
        public enum InputMode
        {
            Mouse,
            Buttons
        }

        [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
        public InputMode inputMode
        {
            get { return InputMode.Mouse; }
        }

        [Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
        public bool allowActivationOnMobileDevice
        {
            get { return m_ForceModuleActive; }
            set { m_ForceModuleActive = value; }
        }

        public bool forceModuleActive
        {
            get { return m_ForceModuleActive; }
            set { m_ForceModuleActive = value; }
        }

        public float inputActionsPerSecond
        {
            get { return m_InputActionsPerSecond; }
            set { m_InputActionsPerSecond = value; }
        }

        public float repeatDelay
        {
            get { return m_RepeatDelay; }
            set { m_RepeatDelay = value; }
        }
        #endregion

        #region Constructors
        protected ArgosStandaloneInputModule()
        {
        }
        #endregion

        #region Methods & Functions
        public override void UpdateModule()
        {
            m_LastMousePosition = m_MousePosition;
            m_MousePosition = UnityEngine.Input.mousePosition;
        }

        public override bool IsModuleSupported()
        {
            return m_ForceModuleActive || UnityEngine.Input.mousePresent || UnityEngine.Input.touchSupported;
        }

        public override bool ShouldActivateModule()
        {
            if (!base.ShouldActivateModule())
                return false;

            var shouldActivate = m_ForceModuleActive;

            shouldActivate |= InputManager.Instance.GetAction(this._inputMap, this._submit);
            shouldActivate |= InputManager.Instance.GetAction(this._inputMap, this._cancel);
            shouldActivate |= !Mathf.Approximately(InputManager.Instance.GetAxis(this._inputMap, this._navigation).x, 0f);
            shouldActivate |= !Mathf.Approximately(InputManager.Instance.GetAxis(this._inputMap, this._navigation).y, 0f);
            shouldActivate |= (m_MousePosition - m_LastMousePosition).sqrMagnitude > 0.0f;
            shouldActivate |= UnityEngine.Input.GetMouseButtonDown(0);


            for (int i = 0; i < UnityEngine.Input.touchCount; ++i)
            {
                Touch input = UnityEngine.Input.GetTouch(i);

                shouldActivate |= input.phase == TouchPhase.Began
                    || input.phase == TouchPhase.Moved
                    || input.phase == TouchPhase.Stationary;
            }
            return shouldActivate;
        }

        public override void ActivateModule()
        {
            base.ActivateModule();
            m_MousePosition = UnityEngine.Input.mousePosition;
            m_LastMousePosition = UnityEngine.Input.mousePosition;

            var toSelect = eventSystem.currentSelectedGameObject;
            if (toSelect == null)
                toSelect = eventSystem.firstSelectedGameObject;

            eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
        }

        public override void DeactivateModule()
        {
            base.DeactivateModule();
            ClearSelection();
        }

        public override void Process()
        {
            bool usedEvent = SendUpdateEventToSelectedObject();

            if (eventSystem.sendNavigationEvents)
            {
                if (!usedEvent)
                    usedEvent |= SendMoveEventToSelectedObject();

                if (!usedEvent)
                    SendSubmitEventToSelectedObject();
            }

            // touch needs to take precedence because of the mouse emulation layer
            if (!ProcessTouchEvents())
                ProcessMouseEvent();

            // If the player push Submit input action, run the OnSubmit event:
            if (InputManager.Instance.GetAction(this._inputMap, this._submit))
            {
                this._onSubmit.Invoke();
            }

            // If the player push Cancel input action, run the OnCancel event:
            if (InputManager.Instance.GetAction(this._inputMap, this._cancel))
            {
                this._onCancel.Invoke();
            }

            // If the player push Delete input action, run the OnDelete event:
            if (InputManager.Instance.GetAction(this._inputMap, this._delete))
            {
                this._onDelete.Invoke();
            }

            // If the player push SetDefaults input action, run the OnSetDefaults event:
            if (InputManager.Instance.GetAction(this._inputMap, this._default))
            {
                this._onSetToDefault.Invoke();
            }
        }

        private bool ProcessTouchEvents()
        {
            for (int i = 0; i < UnityEngine.Input.touchCount; ++i)
            {
                Touch input = UnityEngine.Input.GetTouch(i);

                if (input.type == TouchType.Indirect)
                    continue;

                bool released;
                bool pressed;
                var pointer = GetTouchPointerEventData(input, out pressed, out released);

                ProcessTouchPress(pointer, pressed, released);

                if (!released)
                {
                    ProcessMove(pointer);
                    ProcessDrag(pointer);
                }
                else
                    RemovePointerData(pointer);
            }
            return UnityEngine.Input.touchCount > 0;
        }

        private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
        {
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

            // PointerDown notification
            if (pressed)
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

                DeselectIfSelectionChanged(currentOverGo, pointerEvent);

                if (pointerEvent.pointerEnter != currentOverGo)
                {
                    // send a pointer enter to the touched element if it isn't the one to select...
                    HandlePointerExitAndEnter(pointerEvent, currentOverGo);
                    pointerEvent.pointerEnter = currentOverGo;
                }

                // search for the control that will receive the press
                // if we can't find a press handler set the press
                // handler to be what would receive a click.
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // Debug.Log("Pressed: " + newPressed);

                float time = Time.unscaledTime;

                if (newPressed == pointerEvent.lastPress)
                {
                    var diffTime = time - pointerEvent.clickTime;
                    if (diffTime < 0.3f)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;

                    pointerEvent.clickTime = time;
                }
                else
                {
                    pointerEvent.clickCount = 1;
                }

                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentOverGo;

                pointerEvent.clickTime = time;

                // Save the drag handler as well
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

                if (pointerEvent.pointerDrag != null)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
            }

            // PointerUp notification
            if (released)
            {
                // Debug.Log("Executing pressup on: " + pointer.pointerPress);
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

                // see if we mouse up on the same element that we clicked on...
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // PointerClick and Drop events
                if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
                }

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;

                if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;

                if (pointerEvent.pointerDrag != null)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

                pointerEvent.pointerDrag = null;

                // send exit events as we need to simulate this on touch up on touch device
                ExecuteEvents.ExecuteHierarchy(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
                pointerEvent.pointerEnter = null;
            }
        }

        /// <summary>
        /// Process submit keys.
        /// </summary>
        protected bool SendSubmitEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;

            var data = GetBaseEventData();

            if (InputManager.Instance.GetAction(this._inputMap, this._submit))
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

            if (InputManager.Instance.GetAction(this._inputMap, this._cancel))
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);

            return data.used;
        }

        private Vector2 GetRawMoveVector()
        {
            var navigationAxis = InputManager.Instance.GetInputMap(this._inputMap).GetAxis(this._navigation);

            Vector2 move = (Vector2)navigationAxis;

            if (navigationAxis.AxisKeyDown.x == 1f)   // Only for keyboard input:
            {
                if (move.x < 0)
                    move.x = -1f;
                if (move.x > 0)
                    move.x = 1f;
            }

            if (navigationAxis.AxisKeyDown.y == 1f)   // Only for keyboard input:
            {
                if (move.y < 0)
                    move.y = -1f;
                if (move.y > 0)
                    move.y = 1f;
            }

            return move;
        }

        /// <summary>
        /// Process keyboard events.
        /// </summary>
        protected bool SendMoveEventToSelectedObject()
        {
            float time = Time.unscaledTime;

            Vector2 movement = GetRawMoveVector();
            if (Mathf.Approximately(movement.x, 0f) && Mathf.Approximately(movement.y, 0f))
            {
                m_ConsecutiveMoveCount = 0;
                return false;
            }

            // If user pressed key again, always allow event
            bool allow = InputManager.Instance.GetInputMap(this._inputMap).GetAxis(this._navigation).AxisKeyDown != Vector2.zero;
            bool similarDir = (Vector2.Dot(movement, m_LastMoveVector) > 0);
            if (!allow)
            {
                // Otherwise, user held down key or axis.
                // If direction didn't change at least 90 degrees, wait for delay before allowing consequtive event.
                if (similarDir && m_ConsecutiveMoveCount == 1)
                    allow = (time > m_PrevActionTime + m_RepeatDelay);
                // If direction changed at least 90 degree, or we already had the delay, repeat at repeat rate.
                else
                    allow = (time > m_PrevActionTime + 1f / m_InputActionsPerSecond);
            }
            if (!allow)
                return false;

            // Debug.Log(m_ProcessingEvent.rawType + " axis:" + m_AllowAxisEvents + " value:" + "(" + x + "," + y + ")");
            var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);

            if (axisEventData.moveDir != MoveDirection.None)
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
                if (!similarDir)
                    m_ConsecutiveMoveCount = 0;
                m_ConsecutiveMoveCount++;
                m_PrevActionTime = time;
                m_LastMoveVector = movement;
            }
            else
            {
                m_ConsecutiveMoveCount = 0;
            }

            return axisEventData.used;
        }

        protected void ProcessMouseEvent()
        {
            if (InputManager.Instance.CurrentInputType == InputManager.InputType.KeyboardAndMouse)
            {
                ProcessMouseEvent(0); 
            }
        }

        /// <summary>
        /// Process all mouse events.
        /// </summary>
        protected void ProcessMouseEvent(int id)
        {
            var mouseData = GetMousePointerEventData(id);
            var leftButtonData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;

            // Process the first mouse button fully
            ProcessMousePress(leftButtonData);
            ProcessMove(leftButtonData.buttonData);
            ProcessDrag(leftButtonData.buttonData);

            // Now process right / middle clicks
            ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData);
            ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
            ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
            ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);

            if (!Mathf.Approximately(leftButtonData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
            {
                var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
                ExecuteEvents.ExecuteHierarchy(scrollHandler, leftButtonData.buttonData, ExecuteEvents.scrollHandler);
            }
        }

        protected bool SendUpdateEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;

            var data = GetBaseEventData();
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
            return data.used;
        }

        /// <summary>
        /// Process the current mouse press.
        /// </summary>
        protected void ProcessMousePress(MouseButtonEventData data)
        {
            var pointerEvent = data.buttonData;
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

            // PointerDown notification
            if (data.PressedThisFrame())
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

                // Avoid to select a null element:
                if (currentOverGo)
                {
                    Selectable selectable = currentOverGo.GetComponent<Selectable>();

                    // Avoid to select a disabled element:
                    if (selectable && selectable.interactable)
                    {
                        DeselectIfSelectionChanged(currentOverGo, pointerEvent);
                    }
                }

                // search for the control that will receive the press
                // if we can't find a press handler set the press
                // handler to be what would receive a click.
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // Debug.Log("Pressed: " + newPressed);

                float time = Time.unscaledTime;

                if (newPressed == pointerEvent.lastPress)
                {
                    var diffTime = time - pointerEvent.clickTime;
                    if (diffTime < 0.3f)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;

                    pointerEvent.clickTime = time;
                }
                else
                {
                    pointerEvent.clickCount = 1;
                }

                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentOverGo;

                pointerEvent.clickTime = time;

                // Save the drag handler as well
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

                if (pointerEvent.pointerDrag != null)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
            }

            // PointerUp notification
            if (data.ReleasedThisFrame())
            {
                // Debug.Log("Executing pressup on: " + pointer.pointerPress);
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

                // see if we mouse up on the same element that we clicked on...
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // PointerClick and Drop events
                if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
                }

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;

                if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;

                // redo pointer enter / exit to refresh state
                // so that if we moused over somethign that ignored it before
                // due to having pressed on something else
                // it now gets it.
                if (currentOverGo != pointerEvent.pointerEnter)
                {
                    HandlePointerExitAndEnter(pointerEvent, null);
                    HandlePointerExitAndEnter(pointerEvent, currentOverGo);
                }
            }
        } 
        #endregion
    }

#if UNITY_EDITOR
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
#endif
}
