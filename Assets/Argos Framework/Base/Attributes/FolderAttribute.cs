using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    #region Enums
    public enum FolderDialogTypes
    {
        OpenFolder,
        SaveFolder
    } 
    #endregion

    /// <summary>
    /// Attribute used to make a string variable in a script be a text field with button to invoke a folder browser.
    /// </summary>
    public class FolderAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly string dialogTitle;
        public readonly FolderDialogTypes dialogType;
        public readonly string folder;
        public readonly string defaultName;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dialogTitle">Title for the open folder dialog.</param>
        /// <param name="dialogType">Dialog behaviour type.</param>
        /// <param name="folder">Initial directory to target the open folder dialog. By default is empty.</param>
        /// <param name="defaultName">Default folder name. By default empty.</param>
        public FolderAttribute(string dialogTitle, FolderDialogTypes dialogType, string folder = "", string defaultName = "")
        {
            this.dialogTitle = dialogTitle;
            this.dialogType = dialogType;
            this.folder = folder;
            this.defaultName = defaultName;
        } 
        #endregion
    }
}
