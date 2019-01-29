using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(FolderAttribute))]
    public class FolderDrawer : TextFieldWithButtonDrawerBase
    {
        public override void OnButtonClick(SerializedProperty property)
        {
            string newPath = string.Empty;
            var target = (FolderAttribute)this.attribute;

            switch (target.DialogType)
            {
                case FolderDialogTypes.OpenFolder:
                    newPath = EditorUtility.OpenFolderPanel(target.DialogTitle, target.Folder, target.DefaultName);
                    break;
                case FolderDialogTypes.SaveFolder:
                    newPath = EditorUtility.SaveFolderPanel(target.DialogTitle, target.Folder, target.DefaultName);
                    break;
            }

            if (!string.IsNullOrEmpty(newPath))
            {
                property.stringValue = newPath;
            }
        }
    } 
}
