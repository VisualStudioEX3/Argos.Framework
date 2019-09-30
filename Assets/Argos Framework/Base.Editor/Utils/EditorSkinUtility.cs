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
        class GUIStyleProperty
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
            }
            #endregion

            #region Operators
            public static implicit operator GUIStyle(GUIStyleProperty item)
            {
                if (item._style == null)
                {
                    item._style = EditorSkinUtility.Skin.GetStyle(item._name).Copy();

                    if (item._customInitialization != null)
                    {
                        item._style = item._customInitialization(item._style);
                    }
                }

                return item._style;
            }
            #endregion
        }

        class GUIContentProperty
        {
            #region Internal vars
            string _name;
            GUIContent _content;
            Func<GUIContent, GUIContent> _customInitialization;
            #endregion

            #region Constructors
            public GUIContentProperty(string name, Func<GUIContent, GUIContent> customInitialization = null)
            {
                this._name = name;
                this._customInitialization = customInitialization;
            }
            #endregion

            #region Operators
            public static implicit operator GUIContent(GUIContentProperty item) => item.GetContent();
            public static implicit operator Texture(GUIContentProperty item) => item.GetContent().image;
            public static implicit operator string(GUIContentProperty item) => item.GetContent().text;
            #endregion

            GUIContent GetContent()
            {
                if (this._content == null)
                {
                    this._content = EditorGUIUtility.IconContent(this._name).Copy();

                    if (this._customInitialization != null)
                    {
                        this._content = this._customInitialization(this._content);
                    }
                }

                return this._content;
            }
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
                /// "grey_border" custom style.
                /// </summary>
                public readonly static GUIStyle greyBorder = new GUIStyleProperty("grey_border");

                /// <summary>
                /// "InvisibleButton" custom style.
                /// </summary>
                public readonly static GUIStyle invisibleButton = new GUIStyleProperty("invisibleButton");
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
                    /// "ToolbarSearchField" custom style.
                    /// </summary>
                    public readonly static GUIStyle textField = new GUIStyleProperty("ToolbarSeachTextField");

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
                /// Fixed version of built-in label style to fix text aligment in Unity 2019.3 or newer versions.
                /// </summary>
                public readonly static GUIStyle upperLeftAligmentLabel = new GUIStyleProperty("label", (style) =>
                {
                    style.alignment = TextAnchor.UpperLeft;
                    return style;
                });

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

        /// <summary>
        /// Unity built-in icons.
        /// </summary>
        /// <remarks>This class exposed some cached built-in editor icons.</remarks>
        public static class Icons
        {
            #region Classes
            /// <summary>
            /// Debug console icons.
            /// </summary>
            public static class Console
            {
                #region Constants
                /// <summary>
                /// "console.erroricon.sml" icon.
                /// </summary>
                public readonly static GUIContent errorIcon = new GUIContentProperty("console.erroricon.sml");
                #endregion
            } 
            #endregion
        }
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
