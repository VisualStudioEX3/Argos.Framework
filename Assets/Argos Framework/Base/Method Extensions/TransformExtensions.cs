using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Transform method extensions.
    /// </summary>
    public static class TransformExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Interpolates position, rotation and scale between a and b by t and normalizes the result afterwards. The parameter t is clamped to the range [0, 1].
        /// </summary>
        /// <param name="instance"><see cref="Transform"/> instance.</param>
        /// <param name="a"><see cref="Transform"/> a.</param>
        /// <param name="b"><see cref="Transform"/> b.</param>
        /// <param name="t">Time of interpolation, a value between 0 and 1.</param>
        /// <param name="relativeTo">If relativeTo is left out or set to <see cref="Space.Self"/> the movement is applied relative to the transform's local axes. (the x, y and z axes shown when selecting the object inside the Scene View.) If relativeTo is <see cref="Space.World"/> the movement is applied relative to the world coordinate system. By default the value is <see cref="Space.World"/>.</param>
        /// <remarks>This is faster than Slerp but looks worse if the rotations are far apart.</remarks>
        public static void Lerp(this Transform instance, Transform a, Transform b, float t, Space relativeTo = Space.World)
        {
            instance.position = Vector3.Lerp(relativeTo == Space.World ? a.position : a.localPosition, relativeTo == Space.World ? b.position : b.localPosition, t);
            instance.rotation = Quaternion.Lerp(relativeTo == Space.World ? a.rotation : a.localRotation, relativeTo == Space.World ? b.rotation : b.localRotation, t);
            instance.localScale = Vector3.Lerp(a.localScale, b.localScale, t);
        }

        /// <summary>
        /// Interpolates position, rotation and scale between a and b by t. The parameter t is not clamped.
        /// </summary>
        /// <param name="instance"><see cref="Transform"/> instance.</param>
        /// <param name="a"><see cref="Transform"/> a.</param>
        /// <param name="b"><see cref="Transform"/> b.</param>
        /// <param name="t">Time of interpolation.</param>
        /// <param name="relativeTo">If relativeTo is left out or set to <see cref="Space.Self"/> the movement is applied relative to the transform's local axes. (the x, y and z axes shown when selecting the object inside the Scene View.) If relativeTo is <see cref="Space.World"/> the movement is applied relative to the world coordinate system. By default the value is <see cref="Space.World"/>.</param>
        /// <remarks>This is faster than SlerpUnclamped but looks worse if the rotations are far apart.</remarks>
        public static void LerpUnclamped(this Transform instance, Transform a, Transform b, float t, Space relativeTo = Space.World)
        {
            instance.position = Vector3.LerpUnclamped(relativeTo == Space.World ? a.position : a.localPosition, relativeTo == Space.World ? b.position : b.localPosition, t);
            instance.rotation = Quaternion.LerpUnclamped(relativeTo == Space.World ? a.rotation : a.localRotation, relativeTo == Space.World ? b.rotation : b.localRotation, t);
            instance.localScale = Vector3.LerpUnclamped(a.localScale, b.localScale, t);
        }

        /// <summary>
        /// Spherically interpolates position, rotation and scale between a and b by t. The parameter t is clamped to the range [0, 1].
        /// </summary>
        /// <param name="instance"><see cref="Transform"/> instance.</param>
        /// <param name="a"><see cref="Transform"/> a.</param>
        /// <param name="b"><see cref="Transform"/> b.</param>
        /// <param name="t">Time of interpolation, a value between 0 and 1.</param>
        /// <param name="relativeTo">If relativeTo is left out or set to <see cref="Space.Self"/> the movement is applied relative to the transform's local axes. (the x, y and z axes shown when selecting the object inside the Scene View.) If relativeTo is <see cref="Space.World"/> the movement is applied relative to the world coordinate system. By default the value is <see cref="Space.World"/>.</param>
        public static void Slerp(this Transform instance, Transform a, Transform b, float t, Space relativeTo = Space.World)
        {
            instance.position = Vector3.Slerp(relativeTo == Space.World ? a.position : a.localPosition, relativeTo == Space.World ? b.position : b.localPosition, t);
            instance.rotation = Quaternion.Slerp(relativeTo == Space.World ? a.rotation : a.localRotation, relativeTo == Space.World ? b.rotation : b.localRotation, t);
            instance.localScale = Vector3.Slerp(a.localScale, b.localScale, t);
        }

        /// <summary>
        /// Spherically interpolates position, rotation and scale between a and b by t. The parameter t is not clamped.
        /// </summary>
        /// <param name="instance"><see cref="Transform"/> instance.</param>
        /// <param name="a"><see cref="Transform"/> a.</param>
        /// <param name="b"><see cref="Transform"/> b.</param>
        /// <param name="t">Time of interpolation.</param>
        /// <param name="relativeTo">If relativeTo is left out or set to <see cref="Space.Self"/> the movement is applied relative to the transform's local axes. (the x, y and z axes shown when selecting the object inside the Scene View.) If relativeTo is <see cref="Space.World"/> the movement is applied relative to the world coordinate system. By default the value is <see cref="Space.World"/>.</param>
        public static void SlerpUnclamped(this Transform instance, Transform a, Transform b, float t, Space relativeTo = Space.World)
        {
            instance.position = Vector3.SlerpUnclamped(relativeTo == Space.World ? a.position : a.localPosition, relativeTo == Space.World ? b.position : b.localPosition, t);
            instance.rotation = Quaternion.SlerpUnclamped(relativeTo == Space.World ? a.rotation : a.localRotation, relativeTo == Space.World ? b.rotation : b.localRotation, t);
            instance.localScale = Vector3.SlerpUnclamped(a.localScale, b.localScale, t);
        } 
        #endregion
    }
}