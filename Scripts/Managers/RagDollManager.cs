using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class RagDollManager : MonoBehaviour
    {
        public bool isRagDoll = false;
        public Animator _animator;
        public Collider _mainCollider;
        public Collider characterCollisionBlocker;
        public Rigidbody _mainBody;
        public List<Collider> _colliders = new List<Collider>();
        public List<Rigidbody> _rigidBodies = new List<Rigidbody>();

        void Start()
        {
            this.AutoGetComponents();
            _animator.transform.GetChild(0).GetComponentsInChildren(_colliders);
            _animator.transform.GetChild(0).GetComponentsInChildren(_rigidBodies);

            ToggleRagDoll(false);
        }

        private void Update() 
        {
            if (isRagDoll)
            {
                ToggleRagDoll(isRagDoll);
                isRagDoll = false;
            }
        }

        public void ToggleRagDoll(bool isRagDoll)
        {
            _animator.enabled = isRagDoll == false;
            _mainCollider.enabled = isRagDoll == false;
            _mainCollider.isTrigger = isRagDoll;
            characterCollisionBlocker.enabled = isRagDoll == false;
            characterCollisionBlocker.isTrigger = isRagDoll;
            _mainBody.isKinematic = isRagDoll;


            foreach (var collider in _colliders)
            {
                collider.enabled = isRagDoll;
            }
            foreach (var rigidBody in _rigidBodies)
            {
                rigidBody.detectCollisions = isRagDoll;
                rigidBody.solverIterations = isRagDoll ? 1 : 0;
                rigidBody.solverVelocityIterations = isRagDoll ? 1 : 0;
                rigidBody.isKinematic = isRagDoll == false;
                rigidBody.sleepThreshold = isRagDoll ? 0.5f : 50000f;
                if (!isRagDoll)
                {
                    rigidBody.Sleep();
                }
            }
        }

        void AutoGetComponents()
        {
            _animator = GetComponent<Animator>();
            _mainCollider = GetComponent<Collider>();
            _mainBody = GetComponent<Rigidbody>();
        }
    }
}
