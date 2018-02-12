using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Shows vector input with new names for each component.
    /// </summary>
    /// <remarks>
    /// Code inspiration from https://gamedev.stackexchange.com/questions/122301/how-can-i-create-a-custom-propertydrawer-for-my-point-struct/123609
    /// </remarks>
    public class VectorRenameAttribute : PropertyAttribute
    {
        public string[] Names;

        public VectorRenameAttribute(params string[] Names)
        {
            this.Names = Names;
        }
    } 
}
