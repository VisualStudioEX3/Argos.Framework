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
        InputAxisDictionary _axes;

        [SerializeField]
        InputActionDictionary _actions;
#pragma warning restore
        #endregion

        #region Properties
        public InputAxisDictionary Axes => this._axes;
        public InputActionDictionary Actions => this._actions;
        #endregion

        #region Update logic
        /// <summary>
        /// Update the axes and actions logic.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            this._axes.Update();
            this._actions.Update();
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Force to update the internal serialized dictionaries.
        /// </summary>
        public new void SetDirty()
        {
            this._axes.SetDirty();
            this._actions.SetDirty();
        }

        /// <summary>
        /// Get the axis with the desired name.
        /// </summary>
        /// <param name="name">Axis name.</param>
        /// <returns>Return the first ocurrence.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public InputAxis GetAxis(string name)
        {
            return this._axes[name];
        }

        /// <summary>
        /// Get the action with the desired name.
        /// </summary>
        /// <param name="name">Action name.</param>
        /// <returns>Return the first ocurrence.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public InputAction GetAction(string name)
        {
            return this._actions[name];
        }
        #endregion
    }
}
