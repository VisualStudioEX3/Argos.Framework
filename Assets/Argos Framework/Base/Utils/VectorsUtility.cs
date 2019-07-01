using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework.Utils
{
    #region Methods & Functions
    /// <summary>
    /// Vector utility methods.
    /// </summary>
    public static class VectorUtility
    {
        /// <summary>
        /// Clamps each <see cref="Vector2"/> value between a minimum <see cref="Vector2"/> and maximum <see cref="Vector2"/> value.
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> instance.</param>
        /// <param name="min">Min <see cref="Vector2"/> value.</param>
        /// <param name="max">Max <see cref="Vector2"/> value</param>
        /// <remarks>Return a clamped vector.</remarks>
        public static Vector2 Clamp(Vector2 vector, Vector2 min, Vector2 max)
        {
            for (int i = 0; i < 2; i++)
            {
                vector[i] = Mathf.Clamp(vector[i], min[i], max[i]);
            }

            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Clamps each <see cref="Vector2Int"/> value between a minimum <see cref="Vector2Int"/> and maximum <see cref="Vector2Int"/> value.
        /// </summary>
        /// <param name="vector"><see cref="Vector2Int"/> instance.</param>
        /// <param name="min">Min <see cref="Vector2Int"/> value.</param>
        /// <param name="max">Max <see cref="Vector2Int"/> value</param>
        /// <remarks>Return a clamped vector.</remarks>
        public static Vector2Int Clamp(Vector2Int vector, Vector2Int min, Vector2Int max)
        {
            for (int i = 0; i < 2; i++)
            {
                vector[i] = Mathf.Clamp(vector[i], min[i], max[i]);
            }

            return new Vector2Int(vector.x, vector.y);
        }

        /// <summary>
        /// Clamps each <see cref="Vector3"/> value between a minimum <see cref="Vector3"/> and maximum <see cref="Vector3"/> value.
        /// </summary>
        /// <param name="vector"><see cref="Vector3"/> instance.</param>
        /// <param name="min">Min <see cref="Vector3"/> value.</param>
        /// <param name="max">Max <see cref="Vector3"/> value</param>
        /// <remarks>Return a clamped vector.</remarks>
        public static Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max)
        {
            for (int i = 0; i < 3; i++)
            {
                vector[i] = Mathf.Clamp(vector[i], min[i], max[i]);
            }

            return new Vector3(vector.x, vector.y, vector.z);
        }

        /// <summary>
        /// Clamps each <see cref="Vector3Int"/> value between a minimum <see cref="Vector3Int"/> and maximum <see cref="Vector3Int"/> value.
        /// </summary>
        /// <param name="vector"><see cref="Vector3Int"/> instance.</param>
        /// <param name="min">Min <see cref="Vector3Int"/> value.</param>
        /// <param name="max">Max <see cref="Vector3Int"/> value</param>
        /// <remarks>Return a clamped vector.</remarks>
        public static Vector3Int Clamp(Vector3Int vector, Vector3Int min, Vector3Int max)
        {
            for (int i = 0; i < 3; i++)
            {
                vector[i] = Mathf.Clamp(vector[i], min[i], max[i]);
            }

            return new Vector3Int(vector.x, vector.y, vector.z);
        }

        /// <summary>
        /// Clamps each <see cref="Vector4"/> value between a minimum <see cref="Vector4"/> and maximum <see cref="Vector4"/> value.
        /// </summary>
        /// <param name="vector"><see cref="Vector4"/> instance.</param>
        /// <param name="min">Min <see cref="Vector4"/> value.</param>
        /// <param name="max">Max <see cref="Vector4"/> value</param>
        /// <remarks>Return a clamped vector.</remarks>
        public static Vector4 Clamp(this Vector4 vector, Vector4 min, Vector4 max)
        {
            for (int i = 0; i < 4; i++)
            {
                vector[i] = Mathf.Clamp(vector[i], min[i], max[i]);
            }

            return new Vector4(vector.x, vector.y, vector.z, vector.w);
        }
    } 
    #endregion
}