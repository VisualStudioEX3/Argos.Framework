using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="MaskableGraphic"/> method extensions.
    /// </summary>
    public static class UIImageExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Returns the instantiated Material assigned to the renderer.
        /// </summary>
        /// <param name="instance"><see cref="Image"/> or any <see cref="MaskableGraphic"/> object instance.</param>
        /// <returns>Return a new instance of this main material.</returns>
        /// <remarks>This functions works like the <see cref="Renderer.material"/> property.
        /// Source: https://answers.unity.com/questions/920091/how-can-i-change-the-shader-parameters-for-an-ui-i.html </remarks>
        public static Material GetInstancedMaterial(this MaskableGraphic instance)
        {
            return Object.Instantiate(instance.material);
        } 
        #endregion
    } 
}
