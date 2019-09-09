using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Utils
{
    /// <summary>
    /// Help functions to manage Unity editor skin theme.
    /// </summary>
    public static class EditorSkinUtility
    {
        #region Structs
        /// <summary>
        /// Custom editor colors.
        /// </summary>
        public readonly struct Colors
        {
            #region Constants
            public static readonly Color bright = EditorGUIUtility.isProSkin ? (Color)(new Color32(98, 98, 98, 255)) : new Color(0.4823529f, 0.4862745f, 0.4823529f, 1f);
            public static readonly Color dark = new Color(0.9333333f, 0.9372549f, 0.9333333f, 1f);
            #endregion
        }
        #endregion

        #region Classes
        public sealed class GUIStyleProperty
        {
            #region Internal vars
            string _name;
            GUIStyle _style;
            Func<GUIStyle, GUIStyle> _customInitialization;
            #endregion

            #region Constructors
            public GUIStyleProperty(string name, Func<GUIStyle, GUIStyle> customInitialization = null)
            {
                this._name = name;
                this._customInitialization = customInitialization;
                this.GetStyle();
            }
            #endregion

            #region Operators
            public static implicit operator GUIStyle(GUIStyleProperty item)
            {
                return item.GetStyle();
            }
            #endregion

            #region Methods & Functions
            GUIStyle GetStyle()
            {
                if (this._style == null)
                {
                    this._style = EditorSkinUtility.Skin.GetStyle(this._name).Copy();

                    if (this._customInitialization != null)
                    {
                        this._style = this._customInitialization(this._style); 
                    }
                }

                return this._style;
            } 
            #endregion
        }

        /// <summary>
        /// Editor styles.
        /// </summary>
        /// <remarks>This class exposed some cached built-in skin editor styles and Argos.Framework custom style variations.</remarks>
        public static class Styles
        {
            #region Constants
            /// <summary>
            /// Unity built-in "box" style.
            /// </summary>
            public readonly static GUIStyle box = new GUIStyleProperty("box");

            /// <summary>
            /// Unity built-in "window" style.
            /// </summary>
            public readonly static GUIStyle window = new GUIStyleProperty("window");
            #endregion

            #region Classes
            /// <summary>
            /// Unity built-in custom styles.
            /// </summary>
            public static class Custom
            {
                #region Constants
                /// <summary>
                /// "InvisibleButton" custom style.
                /// </summary>
                public readonly static GUIStyle invisibleButton = new GUIStyleProperty("invisibleButton");

                /// <summary>
                /// "radio" custom style.
                /// </summary>
                public readonly static GUIStyle radio = new GUIStyleProperty("radio");
                #endregion

                #region Classes
                /// <summary>
                /// ReorderableList styles.
                /// </summary>
                public static class ReorderableList
                {
                    #region Constants
                    /// <summary>
                    /// "RL DragHandle" custom style.
                    /// </summary>
                    public readonly static GUIStyle dragHandle = new GUIStyleProperty("RL DragHandle"); 
                    #endregion
                }

                /// <summary>
                /// Toolbar Search styles.
                /// </summary>
                public static class ToolbarSearch
                {
                    #region Constants
                    /// <summary>
                    /// "ToolbarSeachTextFieldPopup" custom style.
                    /// </summary>
                    public readonly static GUIStyle textFieldPopup = new GUIStyleProperty("ToolbarSeachTextFieldPopup");

                    /// <summary>
                    /// "ToolbarSeachCancelButton" custom style.
                    /// </summary>
                    public readonly static GUIStyle cancelButton = new GUIStyleProperty("ToolbarSeachCancelButton");

                    /// <summary>
                    /// "ToolbarSeachCancelButtonEmpty" custom style.
                    /// </summary>
                    public readonly static GUIStyle cancelButtonEmpty = new GUIStyleProperty("ToolbarSeachCancelButtonEmpty");
                    #endregion
                }
                #endregion
            }

            /// <summary>
            /// Argos.Framework custom styles based or derived from Unity built-in styles.
            /// </summary>
            public static class ArgosCustomVariants
            {
                #region Constants
                /// <summary>
                /// "invisibleButton" custom style with transparent color on normal.textColor property .
                /// </summary>
                public readonly static GUIStyle invisibleButtonWithTransparentText = new GUIStyleProperty("invisibleButton", (style) =>
                {
                    style.normal.textColor = Color.clear;
                    return style;
                });

                /// <summary>
                /// "miniLabel" custom style with disable color on normal.textColor property.
                /// </summary>
                public readonly static GUIStyle disabledMiniLabel = new GUIStyleProperty("miniLabel", (style) =>
                {
                    Color color = style.normal.textColor;
                    color.a = 0.5f;
                    style.normal.textColor = color;
                    return style;
                });

                /// <summary>
                /// "miniLabel" custom style with red color on normal.textColor property.
                /// </summary>
                public readonly static GUIStyle errorMiniLabel = new GUIStyleProperty("miniLabel", (style) =>
                {
                    style.normal.textColor = Color.red;
                    return style;
                });
                #endregion
            }
            #endregion
        }

        // TODO: Create GUIContent class to access built-in icons.
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current Unity editor skin.
        /// </summary>
        public static GUISkin Skin
        {
            get
            {
                return EditorGUIUtility.GetBuiltinSkin(EditorGUIUtility.isProSkin ? EditorSkin.Scene : EditorSkin.Inspector);
            }
        }
        #endregion
    }
}
