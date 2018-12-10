using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Input map asset.
    /// </summary>
    /// <remarks>Defines an input map with the axes and actions bindings.</remarks>
    [CreateAssetMenu(fileName = "New Input Map", menuName = "Argos.Framework/Input/Input Map")]
    public class InputMapAsset : ScriptableObject
    {
        #region Structs
        [Serializable]
        public struct InputMapAxisData
        {
            public string Name;
            public InputAxis Data;
        }

        [Serializable]
        public struct InputMapActionData
        {
            public string Name;
            public InputAction Data;
        }
        #endregion

        #region Inspector fields
#pragma warning disable 0649
        [SerializeField]
        List<InputMapAxisData> _axes;

        [SerializeField]
        List<InputMapActionData> _actions;
#pragma warning restore
        #endregion

        #region Update logic
        /// <summary>
        /// Update the axes and actions logic.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            for (int i = 0; i < this._axes.Count; i++)
            {
                this._axes[i].Data.Update();
            }

            for (int i = 0; i < this._actions.Count; i++)
            {
                this._actions[i].Data.Update();
            }
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Get the axis with the desired name.
        /// </summary>
        /// <param name="name">Axis name.</param>
        /// <returns>Return the first ocurrence.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public InputAxis GetAxis(string name)
        {
            for (int i = 0; i < this._axes.Count; i++)
            {
                if (this._axes[i].Name == name)
                {
                    return this._axes[i].Data;
                }
            }

            throw new KeyNotFoundException($"The axis '{name}' not exists.");
        }

        /// <summary>
        /// Get the action with the desired name.
        /// </summary>
        /// <param name="name">Action name.</param>
        /// <returns>Return the first ocurrence.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public InputAction GetAction(string name)
        {
            for (int i = 0; i < this._actions.Count; i++)
            {
                if (this._actions[i].Name == name)
                {
                    return this._actions[i].Data;
                }
            }

            throw new KeyNotFoundException($"The action '{name}' not exists.");
        }
        #endregion
    }
}
