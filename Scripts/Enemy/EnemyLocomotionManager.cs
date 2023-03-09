using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemy;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        public LayerMask detectionLayer;

        void Awake() 
        {
            enemy = GetComponent<EnemyManager>();
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        void Start() 
        {
            
        }

    }
}
