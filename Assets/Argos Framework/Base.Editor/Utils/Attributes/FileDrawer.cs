using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(FileAttribute))]
    public class FileDrawer : TextFieldWithButtonDrawerBase
    {
        public override void OnButtonClick(SerializedProperty property)
        {
            string newPath = string.Empty;
            var target = (FileAttribute)this.attribute;

            switch (target.DialogType)
            {
                case FileDialogTypes.OpenFile:
                    newPath = EditorUtility.OpenFilePanel(target.DialogTitle, target.Directory, target.FileExtension);
                    break;
                case FileDialogTypes.SaveFile:
                    newPath = EditorUtility.SaveFilePanel(target.DialogTitle, target.Directory, target.DefaultName, target.FileExtension);
                    break;
                case FileDialogTypes.SaveFileInProject:
                    newPath = EditorUtility.SaveFilePanelInProject(target.DialogTitle, target.Directory, target.FileExtension, target.Message);
                    break;
            }

            if (!string.IsNullOrEmpty(newPath))
            {
                property.stringValue = newPath;
            }
        }
    } 
}
