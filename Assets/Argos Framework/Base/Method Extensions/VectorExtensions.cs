using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework.Utils;

namespace Argos.Framework
{
    /// <summary>
    /// Method extensions for vector types.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Get angle between two <see cref="Vector2"/> positions.
        /// </summary>
        /// <param name="instance"><see cref="Vector2"/> instance.</param>
        /// <param name="target">Target <see cref="Vector2"/> position.</param>
        /// <returns>Return the angle between positions.</returns>
        /// <remarks>This function calculate the angle between two vectors without care their vector directions.</remarks>
        public static float AngleBetweenPositions(this Vector2 instance, Vector2 target)
        {
            return MathUtility.GetAngle(instance, target);
        }

        /// <summary>
        /// Get angle between two <see cref="Vector2Int"/> positions.
        /// </summary>
        /// <param name="instance"><see cref="Vector2Int"/> instance.</param>
        /// <param name="target">Target <see cref="Vector2Int"/> position.</param>
        /// <returns>Return the angle between positions.</returns>
        /// <remarks>This function calculate the angle between two vectors without care their vector directions.</remarks>
        public static float AngleBetweenPositions(this Vector2Int instance, Vector2Int target)
        {
            return VectorExtensions.AngleBetweenPositions(instance, target);
        }

        /// <summary>
        /// Get angle between two <see cref="Vector3"/> positions.
        /// </summary>
        /// <param name="instance"><see cref="Vector3"/> instance.</param>
        /// <param name="target">Target <see cref="Vector3"/> position.</param>
        /// <param name="axis">World rotation axis between vectors. Only accepts <see cref="Vector3.up"/> and <see cref="Vector3.right"/>, any other value must be interpreted as <see cref="Vector3.forward"/>.</param>
        /// <returns>Return the angle in degrees between positions.</returns>
        /// <remarks>This function calculate the angle in degrees between two vectors ignoring their vector directions.</remarks>
        public static float AngleBetweenPositions(this Vector3 instance, Vector3 target, Vector3 axis)
        {
            return MathUtility.GetAngle(instance, target, axis);
        }

        /// <summary>
        /// Get angle between two <see cref="Vector3Int"/> positions.
        /// </summary>
        /// <param name="instance"><see cref="Vector3Int"/> instance.</param>
        /// <param name="target">Target <see cref="Vector3Int"/> position.</param>
        /// <param name="axis">World rotation axis between vectors. Only accepts <see cref="Vector3Int.up"/> and <see cref="Vector3Int.right"/>, any other value must be interpreted as <see cref="Vector3Int.forward"/>.</param>
        /// <returns>Return the angle in degrees between positions.</returns>
        /// <remarks>This function calculate the angle in degrees between two vectors ignoring their vector directions.</remarks>
        public static float AngleBetweenPositions(this Vector3Int instance, Vector3Int target, Vector3Int axis)
        {
            return VectorExtensions.AngleBetweenPositions(instance, target, axis);
        }
    } 
}
