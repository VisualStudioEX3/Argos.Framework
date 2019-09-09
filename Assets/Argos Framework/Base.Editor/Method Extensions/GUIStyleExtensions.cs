using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="GUIStyle"/> method extensions.
    /// </summary>
    public static class GUIStyleExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Gets a copy from this style.
        /// </summary>
        /// <param name="style"><see cref="GUIStyle"/> instance.</param>
        /// <returns>Returns a new <see cref="GUIStyle"/> instance as copy of this instance.</returns>
        public static GUIStyle Copy(this GUIStyle style)
        {
            return new GUIStyle(style);
        }

        /// <summary>
        /// Draws this <see cref="GUIStyle"/> only when the current event is a <see cref="EventType.Repaint"/>.
        /// </summary>
        /// <param name="style"><see cref="GUIStyle"/> instance.</param>
        /// <param name="position">Postion to draw.</param>
        /// <param name="isHover">Is hover?.</param>
        /// <param name="isActive">Is active?</param>
        /// <param name="on">Is on?</param>
        /// <param name="hasKeyboardFocus">Has keyboard focus?</param>
        /// <returns>Returns true if the function can draw the style. False if not.</returns>
        public static bool SafeDraw(this GUIStyle style, Rect position, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
        {
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position, isHover, isActive, on, hasKeyboardFocus);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws this <see cref="GUIStyle"/> only when the current event is a <see cref="EventType.Repaint"/>.
        /// </summary>
        /// <param name="style"><see cref="GUIStyle"/> instance.</param>
        /// <param name="position">Postion to draw.</param>
        /// <param name="image"><see cref="Texture"/> to draw.</param>
        /// <param name="isHover">Is hover?.</param>
        /// <param name="isActive">Is active?</param>
        /// <param name="on">Is on?</param>
        /// <param name="hasKeyboardFocus">Has keyboard focus?</param>
        /// <returns>Returns true if the function can draw the style. False if not.</returns>
        public static bool SafeDraw(this GUIStyle style, Rect position, Texture image, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
        {
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position, image, isHover, isActive, on, hasKeyboardFocus);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws this <see cref="GUIStyle"/> only when the current event is a <see cref="EventType.Repaint"/>.
        /// </summary>
        /// <param name="style"><see cref="GUIStyle"/> instance.</param>
        /// <param name="position">Postion to draw.</param>
        /// <param name="content"><see cref="GUIContent"/> to draw.</param>
        /// <param name="isHover">Is hover?.</param>
        /// <param name="isActive">Is active?</param>
        /// <param name="on">Is on?</param>
        /// <param name="hasKeyboardFocus">Has keyboard focus?</param>
        /// <returns>Returns true if the function can draw the style. False if not.</returns>
        public static bool SafeDraw(this GUIStyle style, Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
        {
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position, content, isHover, isActive, on, hasKeyboardFocus);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws this <see cref="GUIStyle"/> only when the current event is a <see cref="EventType.Repaint"/>.
        /// </summary>
        /// <param name="style"><see cref="GUIStyle"/> instance.</param>
        /// <param name="position">Postion to draw.</param>
        /// <param name="text"><see cref="string"/> to draw.</param>
        /// <param name="isHover">Is hover?.</param>
        /// <param name="isActive">Is active?</param>
        /// <param name="on">Is on?</param>
        /// <param name="hasKeyboardFocus">Has keyboard focus?</param>
        /// <returns>Returns true if the function can draw the style. False if not.</returns>
        public static bool SafeDraw(this GUIStyle style, Rect position, string text, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
        {
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position, text, isHover, isActive, on, hasKeyboardFocus);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws this <see cref="GUIStyle"/> only when the current event is a <see cref="EventType.Repaint"/>.
        /// </summary>
        /// <param name="style"><see cref="GUIStyle"/> instance.</param>
        /// <param name="position">Postion to draw.</param>
        /// <param name="content"><see cref="GUIContent"/> to draw.</param>
        /// <param name="controlID">Control ID.</param>
        /// <returns>Returns true if the function can draw the style. False if not.</returns>
        public static bool SafeDraw(this GUIStyle style, Rect position, GUIContent content, int controlID)
        {
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position, content, controlID);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws this <see cref="GUIStyle"/> only when the current event is a <see cref="EventType.Repaint"/>.
        /// </summary>
        /// <param name="style"><see cref="GUIStyle"/> instance.</param>
        /// <param name="position">Postion to draw.</param>
        /// <param name="content"><see cref="GUIContent"/> to draw.</param>
        /// <param name="controlID">Control ID.</param>
        /// <param name="on">Is on?</param>
        /// <returns>Returns true if the function can draw the style. False if not.</returns>
        public static bool SafeDraw(this GUIStyle style, Rect position, GUIContent content, int controlID, bool on)
        {
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position, content, controlID, on);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws this <see cref="GUIStyle"/> only when the current event is a <see cref="EventType.Repaint"/>.
        /// </summary>
        /// <param name="style"><see cref="GUIStyle"/> instance.</param>
        /// <param name="position">Postion to draw.</param>
        /// <param name="content"><see cref="GUIContent"/> to draw.</param>
        /// <param name="controlID">Control ID.</param>
        /// <param name="on">Is on?</param>
        /// <param name="hasKeyboardFocus">Has keyboard focus?</param>
        /// <returns>Returns true if the function can draw the style. False if not.</returns>
        public static bool SafeDraw(this GUIStyle style, Rect position, GUIContent content, int controlID, bool on, bool hasKeyboardFocus)
        {
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position, content, controlID, on, hasKeyboardFocus);
                return true;
            }

            return false;
        } 
        #endregion
    } 
}
