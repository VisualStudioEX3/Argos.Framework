using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

namespace Argos.Framework
{
    /// <summary>
    /// Ragdoll Controller.
    /// </summary>
    /// <remarks>This component allow to enable or disabled all RigidBodies attached to this game object childs (as result to apply a ragdoll configuration).</remarks>
    [AddComponentMenu("Argos.Framework/Utils/Ragdoll Controller"), DisallowMultipleComponent]
    public sealed class RagdollController : MonoBehaviour
    {
        #region Internal vars
        Rigidbody[] _rigidBodies;
        #endregion

        #region Serialized fields
#pragma warning disable 0649
        [Space, SerializeField, HelpBox("This controller set to true the property enableProjection in all CharacterJoints to improve the ragdoll stability under extreme circumstances (such as spawning partially inside a wall or pushed with a large force).", HelpBoxMessageType.Info)]
        bool _overrideCollidersTag = false;
        [SerializeField, Tag]
        string _collidersTag;
        [Space, SerializeField]
        bool _overrideCollidersLayer = false;
        [SerializeField, Layer]
        int _collidersLayer;
#pragma warning restore
        #endregion

        #region Properties
        /// <summary>
        /// Is Ragdoll components active?
        /// </summary>
        /// <remarks>This property return the <see cref="Rigidbody.isKinematic"/> value from the first <see cref="Transform"/>.</remarks>
        public bool IsRagdollActive
        {
            get
            {
                return this._rigidBodies[0].isKinematic;
            }
            private set
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

            if (this._overrideCollidersTag || this._overrideCollidersLayer)
            {
                foreach (var rigidBody in this._rigidBodies)
                {
                    if (this._overrideCollidersTag)
                    {
                        rigidBody.tag = this._collidersTag; 
                    }

                    if (this._overrideCollidersLayer)
                    {
                        rigidBody.gameObject.layer = this._collidersLayer; 
                    }

                    var characterJoint = rigidBody.GetComponent<CharacterJoint>();
                    if (characterJoint)
                    {
                        characterJoint.enableProjection = true;
                    }
                } 
            }
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Enable ragdoll.
        /// </summary>
        /// <remarks>This call setup all <see cref="Rigidbody.isKinematic"/> to true.</remarks>
        public void EnableRagdoll()
        {
            this.IsRagdollActive = true;
        }

        /// <summary>
        /// Disable ragdoll.
        /// </summary>
        /// <remarks>This call setup all <see cref="Rigidbody.isKinematic"/> to false.</remarks>
        public void DisableRagdoll()
        {
            this.IsRagdollActive = false;
        } 
        #endregion
    }
}