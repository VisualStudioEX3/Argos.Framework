using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    public abstract class ArgosPropertyAttributeBase : PropertyAttribute
    {
        #region Public vars
        public readonly string tooltip;
        #endregion

        #region Constructors
        public ArgosPropertyAttributeBase(string tooltip = "")
        {
            this.tooltip = tooltip;
        } 
        #endregion
    } 
}
