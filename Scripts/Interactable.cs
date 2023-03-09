using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;
        public bool isLootItem;

        protected virtual void Awake()
        {

        }

        protected virtual void Start() 
        {
            
        }

        void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            //Called When Player Interacts
            Debug.Log("You Interacted With An Object");
        }
    }
}
