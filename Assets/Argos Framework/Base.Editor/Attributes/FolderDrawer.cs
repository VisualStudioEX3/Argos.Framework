using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(FolderAttribute))]
    public class FolderDrawer : TextFieldWithButtonDrawerBase
    {
        #region Event listeners
        public override void OnButtonClick(SerializedProperty property)
        {
            string newPath = string.Empty;
            var target = (FolderAttribute)this.attribute;

            switch (target.dialogType)
            {
                case FolderDialogTypes.OpenFolder:
                    newPath = EditorUtility.OpenFolderPanel(target.dialogTitle, target.folder, target.defaultName);
                    break;
                case FolderDialogTypes.SaveFolder:
                    newPath = EditorUtility.SaveFolderPanel(target.dialogTitle, target.folder, target.defaultName);
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
