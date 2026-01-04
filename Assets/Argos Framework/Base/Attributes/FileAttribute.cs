using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    #region Enums
    public enum FileDialogTypes
    {
        OpenFile,
        SaveFile,
        SaveFileInProject
    } 
    #endregion

    /// <summary>
    /// Attribute used to make a string variable in a script be a text field with button to invoke a file browser.
    /// </summary>
    public class FileAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly string dialogTitle;
        public readonly FileDialogTypes dialogType;
        public readonly string directory;
        public readonly string defaultName;
        public readonly string fileExtension;
        public readonly string message;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dialogTitle">Title for the open file dialog.</param>
        /// <param name="dialogType">Dialog behaviour type.</param>
        /// <param name="fileExtension">File extension for the open file dialog.</param>
        /// <param name="directory">Initial directory to target the open file dialog. By default is empty.</param>
        /// <param name="defaultName">Default filename. Use only for SaveFile type. By default is empty.</param>
        /// <param name="message">Message displayed in dialog. Only for SaveFileInProject (and only for OSX dialogs). By default is empty.</param>
        public FileAttribute(string dialogTitle, FileDialogTypes dialogType, string fileExtension, string directory = "", string defaultName = "", string message = "")
        {
            this.dialogTitle = dialogTitle;
            this.dialogType = dialogType;
            this.fileExtension = fileExtension;
            this.directory = directory;
            this.defaultName = defaultName;
            this.message = message;
        } 
        #endregion
    }
}