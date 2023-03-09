using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EventColliderBegingBossFight : MonoBehaviour
    {
        public Vector3 colliderNewPosition;
        public bool isPigBoss;

        WorldEventManager worldEventManager;
        Collider eventCollider;
        Transform colliderOriginalPosition;

        void Awake() 
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
            colliderOriginalPosition = GetComponent<Transform>();
            eventCollider = GetComponent<Collider>();
        }

        void Start() 
        {
            colliderOriginalPosition.position = transform.position;
        }

        void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Weapon") { return; }
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (other.tag == "Character")
                {
                    if (!worldEventManager.bossHasBeenAwaked)
                    {
                        eventCollider.gameObject.transform.position = colliderNewPosition;
                    }
                    worldEventManager.isPigBoss = isPigBoss;
                    worldEventManager.ActivateBossFight();
                }
            }
        }
    }
}
