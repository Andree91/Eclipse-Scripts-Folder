using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class BlockCollison : MonoBehaviour
    {
        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        void Awake() 
        {
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }
    }
}
