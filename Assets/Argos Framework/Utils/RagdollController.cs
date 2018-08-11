using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

namespace Argos.Framework.Utils
{
    /// <summary>
    /// Ragdoll Controller.
    /// </summary>
    /// <remarks>This component allow to enable or disabled all RigidBodies attached to this game object childs (as result to apply a ragdoll configuration).</remarks>
    [AddComponentMenu("Argos.Framework/Utils/Ragdoll Controller"), DisallowMultipleComponent]
    public class RagdollController : MonoBehaviour
    {
        #region Internal vars
        Rigidbody[] _rigidBodies;
        #endregion

        #region Serialized fields
        [Header("This field help to set a Tag in all parts of the ragdoll:"), SerializeField, Tag]
        string _collidersTag;
        [Header("This field help to set a Layer in all parts of the ragdoll:"), SerializeField, Layer]
        int _collidersLayer;
        #endregion

        #region Properties
        public bool Active
        {
            get
            {
                return this._rigidBodies[0].isKinematic;
            }
            set
            {
                foreach (var r in this._rigidBodies)
                {
                    r.isKinematic = !value;
                }
            }
        }
        #endregion

        #region Initializers
        void Awake()
        {
            this._rigidBodies = GetComponentsInChildren<Rigidbody>();

            foreach (var rigidBody in this._rigidBodies)
            {
                rigidBody.tag = this._collidersTag;
                rigidBody.gameObject.layer = this._collidersLayer;

                var characterJoint = rigidBody.GetComponent<CharacterJoint>();
                if (characterJoint)
                {
                    characterJoint.enableProjection = true;
                }
            }
        }
        #endregion
    }

}