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
        #region Inspector fields
#pragma warning disable 0649
        [SerializeField]
        SerializableDictionary<string, InputAxis> _axes;

        [SerializeField]
        SerializableDictionary<string, InputAction> _actions;
#pragma warning restore
        #endregion

        #region Properties
        public SerializableDictionary<string, InputAxis> Axes => this._axes;
        public SerializableDictionary<string, InputAction> Actions => this._actions;
        #endregion

        #region Update logic
        /// <summary>
        /// Update the axes and actions logic.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            foreach (var axis in this._axes)
            {
                axis.Update();
            }

            foreach (var action in this._actions)
            {
                action.Update();
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
            if (this._axes.ContainsKey(name))
            {
                return this._axes[name];
            }

            throw new KeyNotFoundException($"{this.GetClassName()}: The axis '{name}' not exists.");
        }

        /// <summary>
        /// Get the action with the desired name.
        /// </summary>
        /// <param name="name">Action name.</param>
        /// <returns>Return the first ocurrence.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public InputAction GetAction(string name)
        {
            if (this._actions.ContainsKey(name))
            {
                return this._actions[name];
            }

            throw new KeyNotFoundException($"{this.GetClassName()}The action '{name}' not exists.");
        }
        #endregion
    }
}
