using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Input;
using Argos.Framework.Input.Extensions;
using System;
using Argos.Framework.FileSystem;
using Argos.Framework.Helpers;

public class Test : MonoBehaviour
{
    [Serializable]
    public struct TestStruct
    {
        [HelpBox("Struct element helpbox", HelpBoxMessageType.Info)]
        public int Field1;

        [MinMaxSlider(0f, 5f)]
        public Vector2Int Field2;

        public SceneAsset Field3;

        [ProgressBar("Field4", true)]
        public float Field4;

        [TexturePreview]
        public Texture Texture;

        [TexturePreview]
        public Texture2D Texture2D;

        [TexturePreview]
        public RenderTexture RenderTexture;

        [TexturePreview]
        public Cubemap CubeMap;

        [TexturePreview]
        public Sprite Sprite;

        [Multiline(10)]
        public string Multiline;

        [OptionList]
        public PrimitiveType OptionList;

        [OptionList(true)]
        public RuntimePlatform SplittedOptionList;

        [Button]
        public string ButtonAttribute;

        [Button("Large Button", GUIButtonSize.Large)]
        public string LargeButtonAttribute;

        [Button("Mini Button", GUIButtonSize.Mini)]
        public string MiniButtonAttribute;

        [File("Open file", FileDialogTypes.OpenFile, "PNG")]
        public string OpenFileField;

        [File("Save file", FileDialogTypes.SaveFile, "PNG")]
        public string SaveFileField;

        [File("Save file", FileDialogTypes.SaveFileInProject, "PNG", "", "New image.png", "Save image in project.")]
        public string SaveFileInProjectField;

        [Folder("Open folder", FolderDialogTypes.OpenFolder)]
        public string OpenFolderField;

        [Folder("Save folder", FolderDialogTypes.SaveFolder)]
        public string SaveFolderField;
    }

    public FileSlotAsset File;

    [HelpBox("Argos Helpbox", HelpBoxMessageType.Warning)]
    [MinMaxSlider(0f, 5f, "Test")]
    public Vector2 Vector2MinMaxSliderField;

    [MinMaxSlider(0f, 5f, "Test")]
    public Vector2Int Vector2IntMinMaxSliderField;

    [Tooltip("Test")] // TooltipAttribute is not detected in PropertyDrawers ¿?
    public SceneAsset SceneField;

    [Tag("Test")]
    public string TagField;

    [Layer("Test")]
    public int LayerField;

    public SortingLayer SortingLayer;

    [CustomVector(new string[] {"L", "R"}, "Test")]
    public Vector2 CustomVector2;

    [Label("Label field.")]
    [CustomVector("W", "H")]
    public Vector2Int CustomVector2Int;

    [Label("Mini label field.", true)]
    [CustomVector("P", "R", "Y")]
    public Vector3 CustomVector3;

    [Label("Selectable label field.", false, true)]
    [CustomVector("R", "G", "B")]
    public Vector3Int CustomVector3Int;

    [Label("Selectable mini label field.", true, true)]
    [CustomVector("R", "G", "B", "A")]
    public Vector4 CustomVector4;

    [ProgressBar("Test")]
    public float ProgressBarField;

    [ProgressBar("This is a full width ProgressBar field with message")]
    public float ProgressBarFieldWithMessage;

    [ProgressBar("", true)]
    public float LabeledProgressBarField;

    [ProgressBar("ProgressBar message", true)]
    public float LabeledProgressBarFieldWithMessage;

    [DinamicLabel]
    public string DinamicLabel = "Dinamic label.";

    [DinamicLabel(true)]
    public string DinamicMiniLabel = "Dinamic mini label.";

    [DinamicLabel(false, true)]
    public string DinamicSelectableLabel = "Dinamic selectable label.";

    [DinamicLabel]
    public string DinamicSelectableMiniLabel = "Dinamic selectable mini label.";

    [Multiline(10), Tooltip("Test")]
    public string Multiline;

    [Button("", GUIButtonSize.Normal, GUIButtonDisableEvents.Never, "Test")]
    public string DefaultButton = "ButtonTest";

    [Button("Large Button", GUIButtonSize.Large)]
    public string LargeButton = "ButtonTest";

    [Button("Mini Button", GUIButtonSize.Mini)]
    public string MiniButton = "ButtonTest";

    [File("Open file", FileDialogTypes.OpenFile, "PNG", "", "", "", "Test")]
    public string OpenFileField;

    [File("Save file", FileDialogTypes.SaveFile, "PNG")]
    public string SaveFileField;

    [File("Save file", FileDialogTypes.SaveFileInProject, "PNG", "" , "New image.png", "Save image in project.")]
    public string SaveFileInProjectField;

    [Folder("Open folder", FolderDialogTypes.OpenFolder, "", "", "Test")]
    public string OpenFolderField;

    [Folder("Save folder", FolderDialogTypes.SaveFolder)]
    public string SaveFolderField;

    [OptionList]
    public PrimitiveType OptionList;

    [OptionList(true, "Test")]
    public RuntimePlatform SplittedOptionList;

    [TexturePreview(false, "Test")]
    public Texture Texture;

    [TexturePreview]
    public Texture2D Texture2D;

    [TexturePreview]
    public RenderTexture RenderTexture;

    [TexturePreview]
    public Cubemap CubeMap;

    [TexturePreview]
    public Sprite Sprite;

    public TestStruct Test2;

    IEnumerator TestProgressBarCoroutine()
    {
        this.ProgressBarField = this.ProgressBarFieldWithMessage = this.LabeledProgressBarField = this.LabeledProgressBarFieldWithMessage = 0f;

        while (this.ProgressBarField != 1f)
        {
            this.ProgressBarField = this.ProgressBarFieldWithMessage = this.LabeledProgressBarField = this.LabeledProgressBarFieldWithMessage = Mathf.MoveTowards(this.ProgressBarField, 1f, Time.deltaTime);
            this.DinamicLabel = this.DinamicMiniLabel = this.DinamicSelectableLabel = this.DinamicSelectableMiniLabel = $"Progress to {this.ProgressBarField * 100f}%";
            yield return null;
        }
    }

    void Start()
    {
        var ui = InputManager.Instance.GetInputMap("UI");

        ui.GetAction("Submit").OnKeyDown += this.OnEventSubmitTest;
        ui.GetAction("Cancel").OnKeyDown += this.OnEventCancelTest;
    }

    void Update()
    {
        var player = InputManager.Instance.GetInputMap("Player");

        InputManager.Instance.SetGamepadVibration(player.GetAxis("Movement"));

        if (player.GetAction("NextWeapon"))
        {
            print("Next weapon.");
        }

        if (player.GetAction("PreviousWeapon"))
        {
            print("Previous weapon.");
        }
    }

    void OnEventSubmitTest()
    {
        print("Submit!");
    }

    void OnEventCancelTest()
    {
        print("Cancel!");
    }

    public void ButtonTest()
    {
        print("Button test!");
    }
}