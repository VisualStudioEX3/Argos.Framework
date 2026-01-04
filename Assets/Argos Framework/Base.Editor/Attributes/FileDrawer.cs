using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(FileAttribute))]
    public class FileDrawer : TextFieldWithButtonDrawerBase
    {
        #region Event listeners
        public override void OnButtonClick(SerializedProperty property)
        {
            string newPath = string.Empty;
            var target = (FileAttribute)this.attribute;

            switch (target.dialogType)
            {
                case FileDialogTypes.OpenFile:
                    newPath = EditorUtility.OpenFilePanel(target.dialogTitle, target.directory, target.fileExtension);
                    break;
                case FileDialogTypes.SaveFile:
                    newPath = EditorUtility.SaveFilePanel(target.dialogTitle, target.directory, target.defaultName, target.fileExtension);
                    break;
                case FileDialogTypes.SaveFileInProject:
                    newPath = EditorUtility.SaveFilePanelInProject(target.dialogTitle, target.directory, target.fileExtension, target.message);
                    break;
            }

            if (!string.IsNullOrEmpty(newPath))
            {
                property.stringValue = newPath;
            }
        } 
        #endregion
    } 
}
